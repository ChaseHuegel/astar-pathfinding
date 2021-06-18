using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Swordfish.Navigation
{

public class Path
{
    public static int Distance(Coord2D p1, Coord2D p2)
    {
        int lengthX = Mathf.Abs(p1.x - p2.x);
        int lengthY = Mathf.Abs(p1.y - p2.y);

        //  Heuristic distance math
        if (lengthX > lengthY)
            return 14*lengthY + 10*(lengthX - lengthY);
        else
            return 14*lengthX + 10*(lengthY - lengthX);
    }

    public static List<Cell> RetracePath(Cell tracingCell, Cell start)
    {
        List<Cell> path = new List<Cell>();

        //  Trace a path back to the start and reverse it
        while (tracingCell != start)
        {
            path.Add(tracingCell);
            tracingCell = tracingCell.parent;
        }

        path.Reverse();

        //  Return the path
        return path;
    }

    public static List<Cell> Find(Cell start, Cell end, PathfindingGoal[] goals = null, bool ignoreActors = true)
    {
        //  TODO: waypoint system to break down long or complex treks into multiple pathfind attempts
        //  a 'waypoint' can represent the end of this path but not the actual end goal
        //  this can be used to create a new path when a waypoint is reached
        //  This will result in long/complex treks being inaccurate, see: AoE 2 pathing

        Heap<Cell> openList = new Heap<Cell>(Constants.PATH_HEAP_SIZE); //  Cells waiting to be tested
        Heap<Cell> testedList = new Heap<Cell>(Constants.PATH_HEAP_SIZE);   //  Cells which have been tested

        //  Our starting point has to be tested...
        openList.Add(start);

        Cell current;
        while (openList.Count > 0)  //  As long as there are cells to be tested
        {
            current = openList.RemoveFirst();  //  Default to the first open cell
            testedList.Add(current);    //  Move current cell to the tested list

            //  We've reached the target, path to the current cell
            if (current == end)
                return RetracePath(current, start);

            //  Reached end of the heap without finding a path
            if (openList.IsFull() || testedList.IsFull())
                return null;

            //  Not there yet! Go through all neighbors of the current cell..
            foreach(Cell neighbor in current.neighbors())
            {
                if (neighbor != end && neighbor.canPathThru == false)
                {
                    //-------------------------------------------------------
                    // Check diagonals

                    if (neighbor == current.neighborNW())
                    {
                        if (!current.neighbor(Direction.WEST).passable &&
                            !current.neighbor(Direction.NORTH).passable)
                            continue;
                    }
                    else if (neighbor == current.neighborNE())
                    {
                        if (!current.neighbor(Direction.EAST).passable &&
                            !current.neighbor(Direction.NORTH).passable)
                            continue;
                    }
                    else if (neighbor == current.neighborSE())
                    {
                        if (!current.neighbor(Direction.EAST).passable &&
                            !current.neighbor(Direction.SOUTH).passable)
                            continue;
                    }
                    else if (neighbor == current.neighborSW())
                    {
                        if (!current.neighbor(Direction.WEST).passable &&
                            !current.neighbor(Direction.SOUTH).passable)
                            continue;
                    }

                    //  Ignore this neighbor if its solid
                    if (!neighbor.passable)
                        continue;

                    //  Are we pathing around actors? If so, ignore occupied neighbors
                    //  By default we ignore actors, otherwise we would get a slower
                    //  route just because a narrow path is blocked by another actor that may be moving
                    if (!ignoreActors && neighbor.occupied)
                        continue;
                }

                //  Last resort, ignore neighbor if it has been tested
                if (testedList.Contains(neighbor))
                    continue;

                bool neighborInOpenList = openList.Contains(neighbor);

                //  Move cost is the current cell's cost + the heuristic distance between the cell + weighted by # of occupants
                byte moveCost = (byte)((neighbor.occupants.Count*8) + current.gCost + Distance(current.GetCoord(), neighbor.GetCoord()));

                //  Update neighbor if its cost is lower than the move cost OR it isnt in the open list yet
                if (moveCost < neighbor.gCost || !neighborInOpenList)
                {
                    neighbor.gCost = (byte)moveCost;    //  Move cost
                    neighbor.hCost = (byte)Distance( neighbor.GetCoord(), end.GetCoord() ); //  Heuristic move cost
                    neighbor.fCost = (byte)(neighbor.gCost + neighbor.hCost);   //  Final move cost

                    neighbor.parent = current;  //  Assign parent for retracing a path to this neighbor

                    //  Add this neighor to the open list to be tested later
                    if (!neighborInOpenList)
                    {
                        openList.Add(neighbor);
                    }
                }
            }
        }

        //  No path found
        return null;
    }
}

}