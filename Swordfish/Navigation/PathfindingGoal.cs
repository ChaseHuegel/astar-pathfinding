using System;

namespace Swordfish.Navigation
{
[Serializable]
public class PathfindingGoal
{
    public static bool IsGoal(Cell cell, PathfindingGoal[] goals)
    {
        if (goals == null) return false;

        foreach (PathfindingGoal goal in goals)
            if (goal != null && goal.active && goal.CheckGoal(cell))
                return true;

        return false;
    }

    public static bool CheckGoal(Actor actor, Body target, PathfindingGoal[] goals) => CheckGoal(actor, target.GetCellAtGrid(), goals);
    public static bool CheckGoal(Actor actor, Cell cell, PathfindingGoal[] goals)
    {
        foreach (PathfindingGoal goal in goals)
            if (CheckGoal(actor, cell, goal))
                return true;

        return false;
    }

    public static bool CheckGoal(Actor actor, Body target, PathfindingGoal goal) => CheckGoal(actor, target.GetCellAtGrid(), goal);
    public static bool CheckGoal(Actor actor, Cell cell, PathfindingGoal goal)
    {
        if (goal != null && goal.active && goal.CheckGoal(cell, actor))
            return true;

        return false;
    }

    public static PathfindingGoal GetGoal(Actor actor, Cell cell, PathfindingGoal[] goals)
    {
        foreach (PathfindingGoal goal in goals)
            if (CheckGoal(actor, cell, goal))
                return goal;

        return null;
    }

    //  Try a set of goals
    public static bool TryGoal(Actor actor, Body target, PathfindingGoal[] goals) => TryGoal(actor, target.GetCellAtGrid(), goals);
    public static bool TryGoal(Actor actor, Cell cell, PathfindingGoal[] goals)
    {
        if (goals == null) return false;

        foreach (PathfindingGoal goal in goals)
            if (TryGoal(actor, cell, goal))
                return true;

        return false;
    }

    //  Try a single goal
    public static bool TryGoal(Actor actor, Body target, PathfindingGoal goal) => TryGoal(actor, target.GetCellAtGrid(), goal);
    public static bool TryGoal(Actor actor, Cell cell, PathfindingGoal goal)
    {
        if (goal != null && CheckGoal(actor, cell, goal))
        {
            //  Trigger found event
            GoalFoundEvent e = new GoalFoundEvent{ actor = actor, goal = goal, cell = cell };
            OnGoalFoundEvent?.Invoke(null, e);
            if (e.cancel) return false;   //  return if the event has been cancelled by any subscriber

            return true;
        }

        return false;
    }

    public static bool TryInteractGoal(Actor actor, Body target, PathfindingGoal goal) => TryInteractGoal(actor, target, goal);
    public static bool TryInteractGoal(Actor actor, Cell cell, PathfindingGoal goal)
    {
        if (goal != null && goal.active && goal.CheckGoal(cell, actor))
        {
            //  Trigger interact event
            GoalInteractEvent e = new GoalInteractEvent{ actor = actor, goal = goal, cell = cell };
            OnGoalInteractEvent?.Invoke(null, e);
            if (e.cancel) return false;   //  return if the event has been cancelled by any subscriber

            return true;
        }

        return false;
    }

    public static bool TryInteractGoal(Actor actor, Body target, PathfindingGoal[] goals) => TryInteractGoal(actor, target, goals);
    public static bool TryInteractGoal(Actor actor, Cell cell, PathfindingGoal[] goals)
    {
        if (goals == null) return false;

        foreach (PathfindingGoal goal in goals)
            if (TryInteractGoal(actor, cell, goal))
                return true;

        return false;
    }

    //  Trigger a reached event forcefully without checking if its a valid match
    public static bool TriggerInteractGoal(Actor actor, Cell cell, PathfindingGoal goal)
    {
        if (goal != null && goal.active)
        {
            //  Trigger interact event
            GoalInteractEvent e = new GoalInteractEvent{ actor = actor, goal = goal, cell = cell };
            OnGoalInteractEvent?.Invoke(null, e);
            if (e.cancel) return false;   //  return if the event has been cancelled by any subscriber

            return true;
        }

        return false;
    }

    public static bool TriggerGoalChanged(Actor actor, PathfindingGoal oldGoal, PathfindingGoal goal)
    {
        //  Trigger interact event
        GoalChangeEvent e = new GoalChangeEvent{ actor = actor, oldGoal = oldGoal, goal = goal };
        OnGoalChangeEvent?.Invoke(null, e);
        if (e.cancel) return false;   //  return if the event has been cancelled by any subscriber

        return true;
    }

    public bool active = true;
    public bool dynamic = true;

    public virtual bool CheckGoal(Cell cell, Actor actor = null) { return false; }

#region events

    public static event EventHandler<GoalFoundEvent> OnGoalFoundEvent;
    public class GoalFoundEvent : Swordfish.Event
    {
        public Actor actor;
        public PathfindingGoal goal;
        public Cell cell;
    }

    public static event EventHandler<GoalInteractEvent> OnGoalInteractEvent;
    public class GoalInteractEvent : Swordfish.Event
    {
        public Actor actor;
        public PathfindingGoal goal;
        public Cell cell;
    }

    public static event EventHandler<GoalChangeEvent> OnGoalChangeEvent;
    public class GoalChangeEvent : Swordfish.Event
    {
        public Actor actor;
        public PathfindingGoal oldGoal;
        public PathfindingGoal goal;
    }
#endregion
}

}