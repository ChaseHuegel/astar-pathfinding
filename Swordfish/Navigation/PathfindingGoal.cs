using System;

namespace Swordfish.Navigation
{

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

    public static bool CheckGoal(Actor actor, Cell cell, PathfindingGoal goal)
    {
        if (goal != null && goal.active && goal.CheckGoal(cell))
            return true;

        return false;
    }

    //  Try a set of goals
    public static bool TryGoal(Actor actor, Cell cell, PathfindingGoal[] goals)
    {
        if (goals == null) return false;

        foreach (PathfindingGoal goal in goals)
            if (TryGoal(actor, cell, goal))
                return true;

        return false;
    }

    //  Try a single goal
    public static bool TryGoal(Actor actor, Cell cell, PathfindingGoal goal)
    {
        if (goal != null && CheckGoal(actor, cell, goal))
        {
            //  Trigger found event
            GoalFoundEvent e = new GoalFoundEvent{ actor = actor, goal = goal, cell = cell };
            OnGoalFoundEvent?.Invoke(null, e);
            if (e.cancel == true) return false;   //  return if the event has been cancelled by any subscriber

            return true;
        }

        return false;
    }

    public static bool TryInteractGoal(Actor actor, Cell cell, PathfindingGoal goal)
    {
        if (goal != null && goal.active && goal.CheckGoal(cell))
        {
            //  Trigger interact event
            GoalInteractEvent e = new GoalInteractEvent{ actor = actor, goal = goal, cell = cell };
            OnGoalInteractEvent?.Invoke(null, e);
            if (e.cancel == true) return false;   //  return if the event has been cancelled by any subscriber

            return true;
        }

        return false;
    }

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
            if (e.cancel == true) return false;   //  return if the event has been cancelled by any subscriber

            return true;
        }

        return false;
    }

    public bool active = true;

    public virtual bool CheckGoal(Cell cell) { return false; }

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
#endregion
}

}