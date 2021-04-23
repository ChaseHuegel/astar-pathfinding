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

    public static List<Cell> Find(Cell start, Cell end)
    {
        //  TODO: Pathfinding should only be out to a max distance
        //  a 'waypoint' can represent the end of this path but not the actual end goal
        //  this can be used to create a new path when a waypoint is reached to save on overhead
        //  This will result in long treks being inaccurate, see: AoE 2 pathing

        List<Cell> openList = new List<Cell>(); //  Cells waiting to be tested
        List<Cell> testedList = new List<Cell>();   //  Cells which have been tested

        //  Our starting point has to be tested...
        openList.Add(start);

        Cell current;
        while (openList.Count > 0)  //  As long as there are cells to be tested
        {
            current = openList[0];  //  Default to the first open cell

            //  Compare the current cell against any others in the open list.
            //  Find the open cell with the lowest movement cost.
            //  Use the heuristic distance as a tie breaker
            for (int i = 1; i < openList.Count; i++)
            {
                if (openList[i].fCost < current.fCost || openList[i].fCost == current.fCost)
                    if (openList[i].hCost < current.hCost)
                        current = openList[i];
            }

            //  Move current cell to the tested list
            openList.Remove(current);
            testedList.Add(current);

            //  We've reached the end point
            if (current == end)
            {
                List<Cell> path = new List<Cell>();

                //  Trace a path back to the start and reverse it
                Cell tracingNode = end;
                while (tracingNode != start)
                {
                    path.Add(tracingNode);
                    tracingNode = tracingNode.parent;
                }

                path.Reverse();

                //  Return the path
                return path;
            }

            //  Not there yet! Go through all neighbors of the current cell..
            foreach(Cell neighbor in current.neighbors())
            {
                //  Ignore this neighbor if we can't path to it or it has already been tested
                if (neighbor.occupied == true || neighbor.weight != 0 || testedList.Contains(neighbor))
                {
                    continue;
                }

                bool neighborInOpenList = openList.Contains(neighbor);

                //  Move cost is the current cell's cost + the heuristic distance between the cell + neighbor
                byte moveCost = (byte)(current.gCost + Distance(current.GetCoord(), neighbor.GetCoord()));

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