using System;
using System.Collections.Generic;

namespace Swordfish.Navigation
{

public class GoalHolder
{
    private List<PathfindingGoal> goals = new List<PathfindingGoal>();
    public PathfindingGoal[] entries = new PathfindingGoal[0];

    public void Cycle()
    {
        //  Push the first dynamic priority to the end of the list
        for (int i = 0; i < goals.Count; i++)
        {
            if (goals[i].dynamic)
            {
                goals.Add(goals[i]);
                goals.RemoveAt(i);
            }
        }

        entries = goals.ToArray();
    }

    public void Clear()
    {
        goals.Clear();
        entries = goals.ToArray();
    }

    public T Add<T>() where T : PathfindingGoal
    {
        T goal = (T)System.Activator.CreateInstance(typeof(T));
        goals.Add(goal);

        entries = goals.ToArray();
        return goal;
    }

    public void Remove<T>() where T : PathfindingGoal
    {
        goals.Remove( goals.Find(x => x is T) );
        entries = goals.ToArray();
    }

    public void RemoveAll<T>() where T : PathfindingGoal
    {
        goals.RemoveAll(x => x is T);
        entries = goals.ToArray();
    }

    public T Get<T>() where T : PathfindingGoal
    {
        return (T)goals.Find(x => x is T);
    }

    public T Get<T>(Predicate<T> expression) where T : PathfindingGoal
    {
        foreach (T goal in GetAll<T>())
            if (expression(goal))
                return goal;

        return null;
    }

    public List<T> GetAll<T>() where T : PathfindingGoal
    {
        return goals.FindAll(x => x is T).ConvertAll(x => x as T);
    }

    public List<T> GetAll<T>(Predicate<T> expression) where T : PathfindingGoal
    {
        return goals.FindAll( (Predicate<PathfindingGoal>)expression ).ConvertAll(x => x as T);
    }
}

}