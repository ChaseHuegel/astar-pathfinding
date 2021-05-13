using UnityEngine;
using Swordfish.Navigation;

public class GoalBuildRepair: PathfindingGoal
{
    public override bool CheckGoal(Cell cell)
    {
        Structure structure = cell?.GetFirstOccupant<Structure>();

        if (structure != null && structure.NeedsRepairs())
            return true;

        return false;
    }
}