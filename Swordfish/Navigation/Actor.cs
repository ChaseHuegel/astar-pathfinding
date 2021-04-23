using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Swordfish.Navigation
{

public class Actor : MonoBehaviour
{
    public byte tickRate = 5;
    public byte tickTimer = 0;

    public List<Cell> currentPath = null;

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

    //  Perform ticks at a regular interval. FixedUpdate is called 60x/s
    public void FixedUpdate()
    {
        tickTimer++;

        if (tickTimer >= tickRate)
        {
            tickTimer = 0;
            Tick();

            //  If we have a valid path, move along it
            //  TODO: interpolate for smooth movement
            if (HasValidPath())
            {
                SetPosition( currentPath[0].x, currentPath[0].y );
                currentPath.RemoveAt(0);    //  Remove this point on the path

                if (currentPath.Count == 0) currentPath = null;
            }
        }
    }

    //  Debug drawing
    public void OnDrawGizmosSelected()
    {
        if (Application.isEditor != true) return;

        if (currentPath != null)
        {
            foreach (Cell cell in currentPath)
            {
                Gizmos.color = Color.cyan;
                Gizmos.DrawSphere(new Vector3(cell.x, 0, cell.y), 0.15f);
            }
        }
    }
#endregion

#region mutable methods

    public virtual void Initialize() {}
    public virtual void Tick() {}
#endregion


#region getters setters

    public bool HasValidPath()
    {
        return (currentPath != null && currentPath.Count > 0);
    }

    //  Get the currently occupied cell
    public Cell GetCell()
    {
        return World.GetGrid().at((int)transform.position.x, (int)transform.position.z);
    }
#endregion


#region immutable methods

    //  Pathfind to a position
    public void Goto(Direction dir, int distance) { Goto(dir.toVector3() * distance); }
    public void Goto(Vector2 vec) { Goto((int)vec.x, (int)vec.y); }
    public void Goto(Vector3 vec) { Goto((int)vec.x, (int)vec.z); }
    public void Goto(int x, int y)
    {
        currentPath = Path.Find( GetCell(), World.GetGrid().at(x, y) );
    }

    //  Move relative to current position
    public void Move(Direction dir) { Move(dir.toVector3()); }
    public void Move(Vector2 vec) { Move((int)vec.x, (int)vec.y); }
    public void Move(Vector3 vec) { Move((int)vec.x, (int)vec.z); }
    public void Move(int x, int y)
    {
        SetPosition( (int)transform.position.x + x, (int)transform.position.z + y );
    }

    //  Set position snapped to the grid
    public void SetPosition(Vector2 p) { SetPosition((int)p.x, (int)p.y); }
    public void SetPosition(Vector3 p) { SetPosition((int)p.x, (int)p.z); }
    public void SetPosition(int x, int y)
    {
        Cell to = World.GetGrid().at(x, y);

        if ( !to.occupied && to.weight == 0 )
        {
            //  Change occupied state of the cell we left. Do this inside the IF statement to save some overhead
            Cell from = GetCell();
            from.occupied = false;

            //  Move the transform and change occupied state of the cell we went to
            transform.position = new Vector3(to.x, 0, to.y);    //  Inherantly hard snap with no casting overhead
            to.occupied = true;
        }
    }

    //  Set the current cell to be occupied
    public void UpdateCell()
    {
        Cell cell = World.GetGrid().at((int)transform.position.x, (int)transform.position.z);
        cell.occupied = true;
    }

    //  Perform a 'soft' snap by truncating. Inaccurate but less overhead.
    public void SnapToGrid()
    {
        transform.position = new Vector3( (int)transform.position.x, transform.position.y, (int)transform.position.z );
    }

    //  Perform a 'hard' snap by rounding. More accurate with more overhead.
    public void HardSnapToGrid()
    {
        transform.position = new Vector3( Mathf.RoundToInt(transform.position.x), transform.position.y, Mathf.RoundToInt(transform.position.z) );
    }
#endregion
}

}