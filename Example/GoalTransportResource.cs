using UnityEngine;
using Swordfish.Navigation;

public class GoalTransportResource : PathfindingGoal
{
    public ResourceGatheringType type;

    public override bool CheckGoal(Cell cell)
    {
        Structure structure = cell?.GetFirstOccupant<Structure>();

        if (structure != null && structure.CanDropOff(type))
            return true;

        return false;
    }
}