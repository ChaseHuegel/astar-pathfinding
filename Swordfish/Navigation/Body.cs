using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Swordfish.Navigation
{

public class Body : MonoBehaviour
{
    public Coord2D gridPosition = new Coord2D(0, 0);

    public virtual void Initialize() {}
    public virtual void Tick() {}

    private void Start()
    {
        //  Snap to the grid with rounding
        //  Round instead of truncate the initial position (i.e. obstacles which don't align with the grid)
        //  Rounding is more accurate but has more overhead than truncating
        //  Do NOT round unless necessary! Movement is bound to the grid, accuracy not an issue
        HardSnapToGrid();

        Initialize();
    }


#region getters setters

    public Cell GetCellAtTransform()
    {
        Vector3 pos = World.ToWorldSpace(transform.position);
        return World.Grid.at((int)pos.x, (int)pos.z);
    }

    public Cell GetCellAtGrid()
    {
        return World.Grid.at(gridPosition.x, gridPosition.y);
    }
#endregion


#region immutable methods

    //  Move relative to current position
    public bool Move(Direction dir) { return Move(dir.toVector3()); }
    public bool Move(Vector2 vec) { return Move((int)vec.x, (int)vec.y); }
    public bool Move(Vector3 vec) { return Move((int)vec.x, (int)vec.z); }
    public bool Move(int x, int y)
    {
        return SetPosition( gridPosition.x + x, gridPosition.y + y );
    }

    //  Set position snapped to the grid
    public bool SetPosition(Vector2 p) { return SetPosition((int)p.x, (int)p.y); }
    public bool SetPosition(Vector3 p) { return SetPosition((int)p.x, (int)p.z); }
    public bool SetPosition(int x, int y)
    {
        Cell to = World.at(x, y);

        if ( !to.occupied && to.passable)
        {
            Cell from = GetCellAtGrid();

            from.occupied = false;
            to.occupied = true;

            gridPosition.x = x;
            gridPosition.y = y;

            return true;    //  We were able to move
        }

        return false;   // We were unable to move
    }

    //  Force to a spot in the grid regardless of what else is there
    public void SetPositionUnsafe(int x, int y)
    {
        Cell to = World.at(x, y);
        Cell from = GetCellAtGrid();

        from.occupied = false;
        to.occupied = true;

        gridPosition.x = x;
        gridPosition.y = y;
    }

    //  Set the current cell to be occupied
    public void UpdateCell()
    {
        Cell cell = World.Grid.at(gridPosition.x, gridPosition.y);
        cell.occupied = true;
    }

    //  Perform a 'soft' snap by truncating. Inaccurate but less overhead.
    public void SnapToGrid()
    {
        Vector3 pos = World.ToWorldSpace(transform.position);
        gridPosition.x = (int)pos.x;
        gridPosition.y = (int)pos.z;

        UpdateTransform();

        UpdateCell();
    }

    //  Perform a 'hard' snap by rounding. More accurate with more overhead.
    public void HardSnapToGrid()
    {
        Vector3 pos = World.ToWorldSpace(transform.position);

        gridPosition.x = Mathf.RoundToInt(pos.x);
        gridPosition.y = Mathf.RoundToInt(pos.z);

        UpdateTransform();

        UpdateCell();
    }

    //  Force the transform to match the grid position
    public void UpdateTransform()
    {
        transform.position = World.ToTransformSpace(new Vector3(gridPosition.x, transform.position.y, gridPosition.y));
    }
#endregion
}

}