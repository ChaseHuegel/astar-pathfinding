using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Swordfish.Navigation
{

public class Cell
{
    public Grid grid = null;
    public int x, y;
    public byte weight = 0;
    public bool occupied = false;

    //  Pathfinding data
    //  TODO: This should not be stored in the cells
    public Cell parent;
    public byte gCost = 0;
    public byte hCost = 0;
    public byte fCost = 0;

    public Coord2D GetCoord()
    {
        return new Coord2D(x, y);
    }

    public Cell neighbor(Direction dir)
    {
        return grid.at(
            x + (int)dir.toVector3().x,
            y + (int)dir.toVector3().z
            );
    }

    public List<Cell> neighbors()
    {
        List<Cell> cells = new List<Cell>();

        cells.Add( grid.at( x + 1, y) );
        cells.Add( grid.at( x + 1, y + 1) );
        cells.Add( grid.at( x, y + 1) );
        cells.Add( grid.at( x - 1, y + 1) );
        cells.Add( grid.at( x - 1, y) );
        cells.Add( grid.at( x - 1, y - 1) );
        cells.Add( grid.at( x, y - 1) );

        return cells;
    }
}

}