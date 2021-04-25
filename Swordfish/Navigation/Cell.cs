using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Swordfish.Navigation
{

public class Cell : IHeapItem<Cell>
{
    public Grid grid = null;
    public int x, y;
    public byte weight = 0;
    public bool occupied = false;
    public bool passable = true;

    //  Pathfinding info
    //  TODO: try to pull this out of the cell class
    //  Currently this has the least memory overhead
    public Cell parent;
    public byte gCost = 0;
    public byte hCost = 0;
    public byte fCost = 0;

    public int heapIndex = 0;
    public int HeapIndex
    {
        get { return heapIndex; }
        set { heapIndex = value; }
    }

    public int CompareTo(Cell cell)
    {
        int compare = fCost.CompareTo(cell.fCost);

        if (compare == 0)
            compare = hCost.CompareTo(cell.hCost);

        return -compare;
    }

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

        //  Neighbors are ordered counter clockwise

        //  Diagonals
        cells.Add( grid.at( x - 1, y + 1) );
        cells.Add( grid.at( x + 1, y + 1) );
        cells.Add( grid.at( x - 1, y - 1) );
        cells.Add( grid.at( x + 1, y - 1) );

        //  Direct neighbors
        cells.Add( grid.at( x + 1, y) );
        cells.Add( grid.at( x, y + 1) );
        cells.Add( grid.at( x - 1, y) );
        cells.Add( grid.at( x, y - 1) );

        return cells;
    }
}

}