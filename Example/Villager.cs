using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Swordfish;
using Swordfish.Navigation;
using Valve.VR.InteractionSystem;

public enum UnitState
{
    IDLE,
    ROAMING,
    GATHERING,
    TRANSPORTING,
    BUILDANDREPAIR,
}

[RequireComponent(typeof(Damageable))]
public class Villager : Unit
{
    [Header("AI")]
    public UnitState state;

    [Header("Villager")]
    public ResourceGatheringType currentResource;
    public GoalTransportResource transportGoal;

    public int maxCargo = 20;
    public float currentCargo = 0;
    public int workRate = 3;
    public int buildRate = 6;
    public int repairRate = 3;

    public bool IsCargoFull() { return currentCargo >= maxCargo; }
    public bool HasCargo() { return currentCargo > 0; }

    bool isHeld;

    public void HookIntoEvents()
    {
        PathfindingGoal.OnGoalFoundEvent += OnGoalFound;
        PathfindingGoal.OnGoalInteractEvent += OnGoalInteract;

        Actor.OnRepathFailedEvent += OnRepathFailed;
    }

    public void CleanupEvents()
    {
        PathfindingGoal.OnGoalFoundEvent -= OnGoalFound;
        PathfindingGoal.OnGoalInteractEvent -= OnGoalInteract;

        Actor.OnRepathFailedEvent -= OnRepathFailed;
    }

    public override void Initialize()
    {
        base.Initialize();

        HookIntoEvents();

        //  Add goals in order of priority
        goals.Add<GoalBuildRepair>();
        goals.Add<GoalGatherResource>().type = ResourceGatheringType.Grain;
        goals.Add<GoalGatherResource>().type = ResourceGatheringType.Gold;
        goals.Add<GoalGatherResource>().type = ResourceGatheringType.Stone;
        goals.Add<GoalGatherResource>().type = ResourceGatheringType.Wood;
        transportGoal = goals.Add<GoalTransportResource>();
    }

    public void OnDestroy()
    {
        CleanupEvents();
    }

    public void OnAttachedToHand(Hand hand)
    {
        isHeld = true;
        Freeze();
    }

    public void OnDetachedFromHand(Hand hand)
    {
        isHeld = false;
        ResetAI();
        Unfreeze();
    }

    public override void Tick()
    {
        if (isHeld)
            return;

        base.Tick();

        //  Transport type always matches what our current resource is
        transportGoal.type = currentResource;

        //  Default to idling if we don't have a goal to goto or interact with
        if (!GotoNearestGoalWithPriority())
            state = UnitState.IDLE;

        switch (state)
        {
            case UnitState.ROAMING:
                Goto(
                    UnityEngine.Random.Range(gridPosition.x - 4, gridPosition.x + 4),
                    UnityEngine.Random.Range(gridPosition.x - 4, gridPosition.x + 4)
                );
            break;

            case UnitState.GATHERING:
                if (IsCargoFull())  state = UnitState.TRANSPORTING;
                else if (!HasValidTarget()) state = UnitState.IDLE;
            break;

            case UnitState.TRANSPORTING:
                if (!HasCargo()) state = UnitState.IDLE;
            break;

            case UnitState.BUILDANDREPAIR:
                if (!HasValidTarget()) state = UnitState.IDLE;
            break;

            case UnitState.IDLE:
                //  Some sort of logic to decide if we want to roam?
            break;
        }
    }

    public void OnRepathFailed(object sender, Actor.RepathFailedEvent e)
    {
        if (e.actor != this) return;

        //  If unable to find a path, reorder priorities
        //  It is likely we can't reach our current goal
        //  This gives a simple decision making behavior
        if (state == UnitState.GATHERING)
            goals.Cycle();
    }

    public void OnGoalFound(object sender, PathfindingGoal.GoalFoundEvent e)
    {
        if (e.actor != this) return;

        Villager villager = (Villager)e.actor;

        //  Need C# 7 in Unity for switching by type!!!
        if (e.goal is GoalGatherResource && !villager.IsCargoFull())
        {
            villager.state = UnitState.GATHERING;
            villager.currentResource = ((GoalGatherResource)e.goal).type;
            return;
        }
        else if (e.goal is GoalTransportResource && villager.HasCargo())
        {
            villager.state = UnitState.TRANSPORTING;
            return;
        }
        else if (e.goal is GoalBuildRepair)
        {
            villager.state = UnitState.BUILDANDREPAIR;
            return;
        }

        //  default cancel the goal so that another can take priority
        ResetGoal();
        e.Cancel();
    }

    public void OnGoalInteract(object sender, PathfindingGoal.GoalInteractEvent e)
    {
        if (e.actor != this) return;

        Villager villager = (Villager)e.actor;
        Resource resource = e.cell.GetOccupant<Resource>();
        Structure structure = e.cell.GetOccupant<Structure>();

        if  (e.goal is GoalGatherResource && villager.TryGather(resource)) return;
        else if (e.goal is GoalTransportResource && villager.TryDropoff(structure)) return;
        else if (e.goal is GoalBuildRepair && villager.TryBuildRepair(structure)) return;

        //  default cancel the interaction
        ResetGoal();
        e.Cancel();
    }

    public bool TryDropoff(Structure structure)
    {
        if (!structure || !HasCargo() || !structure.IsBuilt())
            return false;

        //  Trigger a dropoff event
        DropoffEvent e = new DropoffEvent{ villager = this, structure = structure, resourceType = currentResource, amount = currentCargo };
        OnDropoffEvent?.Invoke(null, e);
        if (e.cancel) return false;   //  return if the event has been cancelled by any subscriber

        currentCargo -= e.amount;
        PlayerManager.instance.AddResourceToStockpile(currentResource, (int)e.amount);

        return true;
    }

    public bool TryGather(Resource resource)
    {
        if (!resource || IsCargoFull())
            return false;

        //  Convert per second to per tick and clamp to how much cargo space we have
        float amount = ((float)workRate / (60/Constants.ACTOR_TICK_RATE));
        amount = Mathf.Clamp(maxCargo - currentCargo, 0, amount);
        amount = resource.GetRemoveAmount(amount);

        //  Trigger a gather event
        GatherEvent e = new GatherEvent{ villager = this, resource = resource, resourceType = currentResource, amount = amount };
        OnGatherEvent?.Invoke(null, e);
        if (e.cancel) return false;   //  return if the event has been cancelled by any subscriber

        //  Remove from the resource and add to cargo
        amount = resource.TryRemove(e.amount);
        currentCargo += amount;

        return true;
    }

    public bool TryBuildRepair(Structure structure)
    {
        if (!structure || structure.AttributeHandler.GetAttributePercent(Attributes.HEALTH) >= 1f)
            return false;

        // Use the repair rate unless the building hasn't been constructed.
        int rate = structure.IsBuilt() ? buildRate : repairRate;

        //  Convert per second to per tick
        int amount = (int)(rate / (60/Constants.ACTOR_TICK_RATE));

        //  Trigger a build/repair event
        BuildRepairEvent e = new BuildRepairEvent{ villager = this, structure = structure, amount = amount };
        OnBuildRepairEvent?.Invoke(null, e);
        if (e.cancel) return false;   //  return if the event has been cancelled by any subscriber

        structure.TryRepair(e.amount, this);

        return true;
    }

    public static event EventHandler<GatherEvent> OnGatherEvent;
    public class GatherEvent : Swordfish.Event
    {
        public Villager villager;
        public Resource resource;
        public ResourceGatheringType resourceType;
        public float amount;
    }

    public static event EventHandler<DropoffEvent> OnDropoffEvent;
    public class DropoffEvent : Swordfish.Event
    {
        public Villager villager;
        public Structure structure;
        public ResourceGatheringType resourceType;
        public float amount;
    }

    public static event EventHandler<BuildRepairEvent> OnBuildRepairEvent;
    public class BuildRepairEvent : Swordfish.Event
    {
        public Villager villager;
        public Structure structure;
        public int amount;
    }
}
