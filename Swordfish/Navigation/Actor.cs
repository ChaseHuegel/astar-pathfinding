using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Swordfish.Navigation
{

public class Actor : Body
{
    protected GoalHolder goals = new GoalHolder();
    public PathfindingGoal[] GetGoals() { return goals.entries; }

    private Damageable damageable;
    public Damageable AttributeHandler { get { return damageable; } }

    [Header("Actor")]
    public Cell currentGoalTarget = null;
    public PathfindingGoal currentGoal = null;
    public byte goalSearchDistance = 20;
    public float movementSpeed = 1f;

    private float movementInterpolation;
    private bool moving = false;

    public List<Cell> currentPath = null;
    private byte pathWaitTries = 0;
    private byte pathRepathTries = 0;
    private bool frozen = false;

    private byte pathTimer = 0;
    private byte tickTimer = 0;

    public bool IsMoving() { return moving; }
    public bool HasValidPath() { return (currentPath != null && currentPath.Count > 0); }

    public bool HasValidGoal() { return (currentGoal != null && currentGoal.active); }
    public bool HasValidTarget()
    {
        return (HasValidGoal() && PathfindingGoal.CheckGoal(this, currentGoalTarget, currentGoal));
    }

    public void Freeze() { frozen = true; RemoveFromGrid(); }
    public void Unfreeze() { frozen = false; UpdatePosition(); }
    public void ToggleFreeze()
    {
        if (frozen = !frozen == false) UpdatePosition();
    }

    public override void Initialize()
    {
        base.Initialize();

        movementInterpolation = 1f - (Constants.ACTOR_PATH_RATE / 60f);
    }


#region immutable methods

    public void UpdatePosition()
    {
        HardSnapToGrid();
        ResetAI();
    }

    public void ResetGoal()
    {
        currentGoal = null;
        currentGoalTarget = null;
    }

    public void ResetPathingBrain()
    {
        pathWaitTries = 0;
        pathRepathTries = 0;
    }

    public void ResetPath()
    {
        currentPath = null;
    }

    public void ResetAI()
    {
        ResetPath();
        ResetGoal();
        ResetPathingBrain();
    }

    public void TryGoalAtHelper(int relativeX, int relativeY, PathfindingGoal goal, ref Cell current, ref Cell result, ref int currentDistance, ref int nearestDistance)
    {
        current = World.at(gridPosition.x + relativeX, gridPosition.y + relativeY);
        currentDistance = DistanceTo(current);

        if (PathfindingGoal.TryGoal(this, current, goal) && currentDistance < nearestDistance)
        {
            nearestDistance = currentDistance;
            result = current;
        }
    }

    public Cell FindNearestGoalWithPriority() { return FindNearestGoal(true); }
    public Cell FindNearestGoal(bool usePriority = false)
    {
        Cell result = null;
        Cell current = null;

        int currentDistance = 0;
        int nearestDistance = int.MaxValue;

        foreach (PathfindingGoal goal in GetGoals())
        {
            currentGoal = goal;

            //  TODO: There is a cleaner way to do this

            //  Radiate out layer by layer around the actor without searching previous layers
            for (int radius = 1; radius < goalSearchDistance; radius++)
            {
                //  Search the top/bottom rows
                for (int x = -radius; x < radius; x++)
                {
                    TryGoalAtHelper(x, radius, goal, ref current, ref result, ref currentDistance, ref nearestDistance);
                    TryGoalAtHelper(x, -radius, goal, ref current, ref result, ref currentDistance, ref nearestDistance);

                    //  Return the first match if goals are being tested in order of priority
                    if (usePriority && result != null)
                        return result;
                }

                //  Search the side columns
                for (int y = -radius; y < radius; y++)
                {
                    TryGoalAtHelper(radius, y, goal, ref current, ref result, ref currentDistance, ref nearestDistance);
                    TryGoalAtHelper(-radius, y, goal, ref current, ref result, ref currentDistance, ref nearestDistance);

                    //  Return the first match if goals are being tested in order of priority
                    if (usePriority && result != null)
                        return result;
                }
            }
        }

        //  No matching goal found
        if (result == null)
            currentGoal = null;

        return result;
    }

    public bool GotoNearestGoalWithPriority() { return GotoNearestGoal(true); }
    public bool GotoNearestGoal(bool usePriority = false)
    {
        if (!HasValidTarget() || !HasValidGoal())
            currentGoalTarget = FindNearestGoal(usePriority);

        if (HasValidTarget() && HasValidGoal())
        {
            Goto(currentGoalTarget.x, currentGoalTarget.y);
            return true;
        }

        return false;
    }

    public void Goto(Direction dir, int distance, bool ignoreActors = true) { Goto(dir.toVector3() * distance, ignoreActors); }
    public void Goto(Coord2D coord, bool ignoreActors = true) { Goto(coord.x, coord.y, ignoreActors); }
    public void Goto(Vector2 vec, bool ignoreActors = true) { Goto((int)vec.x, (int)vec.y, ignoreActors); }
    public void Goto(Vector3 vec, bool ignoreActors = true) { Goto((int)vec.x, (int)vec.z, ignoreActors); }
    public void Goto(int x, int y, bool ignoreActors = true)
    {
        if (!HasValidPath() && DistanceTo(x, y) > 1)
            PathManager.RequestPath(this, x, y, ignoreActors);
    }

    public void GotoForced(Direction dir, int distance, bool ignoreActors = true) { Goto(dir.toVector3() * distance, ignoreActors); }
    public void GotoForced(Vector2 vec, bool ignoreActors = true) { Goto((int)vec.x, (int)vec.y, ignoreActors); }
    public void GotoForced(Vector3 vec, bool ignoreActors = true) { Goto((int)vec.x, (int)vec.z, ignoreActors); }
    public void GotoForced(int x, int y, bool ignoreActors = true)
    {
        if (DistanceTo(x, y) > 1)
            PathManager.RequestPath(this, x, y, ignoreActors);
    }

    public void LookAt(float x, float y)
    {
        Vector3 temp = World.ToTransformSpace(new Vector3(x, 0, y));

        // float damping = 1.0f;
        var lookPos = temp - transform.position;
        lookPos.y = 0;
        var rotation = Quaternion.LookRotation(lookPos);
        transform.rotation = rotation;// Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * damping);
    }
#endregion


#region monobehavior

    //  Perform ticks at a regular interval. FixedUpdate is called 60x/s
    public void FixedUpdate()
    {
        //  Behavior ticking below
        tickTimer++;
        if (tickTimer >= Constants.ACTOR_TICK_RATE)
        {
            tickTimer = 0;

            //  Handle interacting with goals
            if (!moving && HasValidGoal() && HasValidTarget())
            {
                //  Check if we have reached our target, or the path ahead matches our goal
                if (DistanceTo(currentGoalTarget) <= 1 || (HasValidPath() && PathfindingGoal.CheckGoal(this, currentPath[0], currentGoal)))
                {
                    //  Assume our currentGoal is a valid match since it was found successfully.
                    //  Forcibly trigger reached under that assumption
                    PathfindingGoal.TriggerInteractGoal(this, currentGoalTarget, currentGoal);
                    ResetPathingBrain();
                    ResetPath();
                }
            }

            Tick();
        }

        //  Pathfinding and interpolation below
        //  Don't pathfind while frozen
        if (frozen) return;

        Vector3 gridTransformPos = World.ToTransformSpace(gridPosition.x, transform.position.y, gridPosition.y);
        bool reachedTarget = true;

        moving = false;

        //  Interpolate movement
        if (transform.position != gridTransformPos)
        {
            moving = true;

            transform.position = Vector3.MoveTowards
            (
                transform.position,
                gridTransformPos,
                Time.fixedDeltaTime * movementInterpolation * movementSpeed
            );

            if (World.ToWorldCoord(transform.position) != gridPosition)
                reachedTarget = false;
        }

        pathTimer++;
        if (pathTimer >= Constants.ACTOR_PATH_RATE)
            pathTimer = 0;  //  Path tick

        //  If we have a valid path, move along it
        if (HasValidPath())
        {
            // TODO: Add 'waypoints' for longer paths too big for the heap

            //  We can pass thru actors if the path ahead is clear and we are going beyond the next spot
            bool canPassThruActors = currentPath.Count > 2 ? !World.at(currentPath[1].x, currentPath[1].y).IsBlocked() : false;

            //  Attempt to move to the next point
            if ( CanSetPosition(currentPath[0].x, currentPath[0].y, canPassThruActors) )
            {
                //  If the path is clear, reset pathing logic
                ResetPathingBrain();

                //  Only move if we finished interpolating
                if (reachedTarget)
                {
                    //  Look in the direction we're going
                    LookAt(currentPath[0].x, currentPath[0].y);
                    SetPositionUnsafe(currentPath[0].x, currentPath[0].y);

                    currentPath.RemoveAt(0);
                }
            }
            //  Unable to reach the next point, handle pathing logic on tick
            else if (pathTimer == 0)
            {
                // Wait some time to see if path clears
                if (pathWaitTries > Constants.PATH_WAIT_TRIES)
                {
                    //  Path hasn't cleared, try repathing to a point near the current node or occupant of node
                    if (pathRepathTries < Constants.PATH_REPATH_TRIES)
                    {
                        int targetX, targetY;

                        if (HasValidGoal() && HasValidTarget())
                        {
                            Coord2D coord = currentGoalTarget.GetFirstOccupant().GetNearbyCoord();
                            targetX = coord.x;
                            targetY = coord.y;
                        }
                        else
                        {
                            targetX = currentPath[currentPath.Count - 1].x + UnityEngine.Random.Range(-1, 1);
                            targetY = currentPath[currentPath.Count - 1].y + UnityEngine.Random.Range(-1, 1);
                        }

                        //  false, dont ignore actors. Stuck and may need to path around them
                        GotoForced(targetX, targetY, false);

                        //  Trigger repath event
                        RepathEvent e = new RepathEvent{ actor = this };
                        OnRepathEvent?.Invoke(null, e);
                        if (e.cancel) ResetPathingBrain();   //  Reset pathing logic if cancelled
                    }
                    //  Unable to repath, resort to giving up
                    else
                    {
                        ResetAI();

                        //  Trigger repath failed event
                        RepathFailedEvent e = new RepathFailedEvent{ actor = this };
                        OnRepathFailedEvent?.Invoke(null, e);
                    }

                    pathRepathTries++;
                }

                pathWaitTries++;
            }

            //  Don't hang onto an empty path. Save a little memory
            if (currentPath != null && currentPath.Count == 0)
                ResetAI();
        }
    }

    //  Debug drawing
    public virtual void OnDrawGizmosSelected()
    {
        if (Application.isEditor != true) return;

        if (currentPath != null)
        {
            foreach (Cell cell in currentPath)
            {
                Gizmos.color = Color.cyan;
                Gizmos.DrawSphere(World.ToTransformSpace(new Vector3(cell.x, 0, cell.y)), 0.25f * World.GetUnit());
            }
        }
    }
#endregion

#region events

    public static event EventHandler<RepathEvent> OnRepathEvent;
    public class RepathEvent : Swordfish.Event
    {
        public Actor actor;
    }

    public static event EventHandler<RepathFailedEvent> OnRepathFailedEvent;
    public class RepathFailedEvent : Swordfish.Event
    {
        public Actor actor;
    }
#endregion
}

}