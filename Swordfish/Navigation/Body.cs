using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Swordfish.Navigation
{

public class Body : MonoBehaviour
{
    public Coord2D gridPosition = new Coord2D(0, 0);

#region monobehavior

    private void Start()
    {
        //  Snap to the grid with rounding
        //  Round instead of truncate the initial position (i.e. obstacles which don't align with the grid)
        //  Rounding is more accurate but has more overhead than truncating
        //  Do NOT round unless necessary! Movement is bound to the grid, accuracy not an issue
        HardSnapToGrid();

        UpdateCell();   //  Tell the actor's cell that it's occupied

        Initialize();
    }
#endregion

#region mutable methods

    public virtual void Initialize() {}
    public virtual void Tick() {}
#endregion


#region getters setters

    public Cell GetCellAtTransform()
    {
        Vector3 pos = transform.position - World.GetPositionOffset();
        return World.GetGrid().at((int)pos.x, (int)pos.z);
    }

    public Cell GetCellAtGrid()
    {
        return World.GetGrid().at(gridPosition.x, gridPosition.y);
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
        Cell to = World.GetGrid().at(x, y);

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

    //  Set the current cell to be occupied
    public void UpdateCell()
    {
        Cell cell = World.GetGrid().at(gridPosition.x, gridPosition.y);
        cell.occupied = true;
    }

    //  Perform a 'soft' snap by truncating. Inaccurate but less overhead.
    public void SnapToGrid()
    {
        transform.position = new Vector3( (int)transform.position.x, transform.position.y, (int)transform.position.z );

        Vector3 pos = transform.position - World.GetPositionOffset();
        gridPosition.x = (int)pos.x;
        gridPosition.y = (int)pos.z;
    }

    //  Perform a 'hard' snap by rounding. More accurate with more overhead.
    public void HardSnapToGrid()
    {
        transform.position = new Vector3( Mathf.RoundToInt(transform.position.x), transform.position.y, Mathf.RoundToInt(transform.position.z) );

        Vector3 pos = transform.position - World.GetPositionOffset();
        gridPosition.x = Mathf.RoundToInt(pos.x);
        gridPosition.y = Mathf.RoundToInt(pos.z);
    }
#endregion
}

}