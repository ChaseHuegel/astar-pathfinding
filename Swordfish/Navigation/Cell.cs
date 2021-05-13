using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Swordfish.Navigation
{

public class Cell : IHeapItem<Cell>
{
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

    //  Cell info
    public Grid grid = null;
    public int x, y;
    public byte weight = 0;
    public bool passable = true;

    //  Occupants
    public List<Body> occupants = new List<Body>();
    public bool occupied { get { return occupants.Count > 0; } }

    public Body GetFirstOccupant()
    {
        return occupants[0];
    }

    public T GetFirstOccupant<T>() where T : Body
    {
        if (occupied && occupants[0] is T)
            return (T)occupants[0];

        return null;
    }

    public T GetOccupant<T>() where T : Body
    {
        return (T)occupants.Find(x => x is T);
    }

    public int CompareTo(Cell cell)
    {
        int compare = fCost.CompareTo(cell.fCost);

        if (compare == 0)
            compare = hCost.CompareTo(cell.hCost);

        return -compare;
    }

    public bool IsBlocked()
    {
        return (occupied || !passable);
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

        //  Diagonals
        // cells.Add( grid.at( x - 1, y + 1) );
        // cells.Add( grid.at( x + 1, y + 1) );
        // cells.Add( grid.at( x - 1, y - 1) );
        // cells.Add( grid.at( x + 1, y - 1) );

        //  Direct neighbors
        // cells.Add( grid.at( x + 1, y) );
        // cells.Add( grid.at( x, y + 1) );
        // cells.Add( grid.at( x - 1, y) );
        // cells.Add( grid.at( x, y - 1) );

        //  ordered counter clockwise starting from right
        cells.Add( grid.at( x + 1, y) );
        cells.Add( grid.at( x + 1, y + 1) );
        cells.Add( grid.at( x, y + 1) );
        cells.Add( grid.at( x - 1, y + 1) );
        cells.Add( grid.at( x - 1, y) );
        cells.Add( grid.at( x - 1, y - 1) );
        cells.Add( grid.at( x, y - 1) );
        cells.Add( grid.at( x + 1, y - 1) );

        return cells;
    }
}

}