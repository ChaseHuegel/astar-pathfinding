using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Swordfish.Navigation
{

public class Actor : Body
{
    [SerializeField] protected float movementInterpolation = 1f;

    public List<Cell> currentPath = null;
    private byte pathWaitTries = 0;
    private byte pathRepathTries = 0;

    private byte tickTimer = 0;

#region getters setters

    public bool HasValidPath() { return (currentPath != null && currentPath.Count > 0); }
#endregion


#region immutable methods

    //  Pathfind to a position
    public void Goto(Direction dir, int distance) { Goto(dir.toVector3() * distance); }
    public void Goto(Vector2 vec) { Goto((int)vec.x, (int)vec.y); }
    public void Goto(Vector3 vec) { Goto((int)vec.x, (int)vec.z); }
    public void Goto(int x, int y)
    {
        PathManager.RequestPath(this, x, y);
    }
#endregion


#region monobehavior

    //  Perform ticks at a regular interval. FixedUpdate is called 60x/s
    public virtual void FixedUpdate()
    {
        tickTimer++;
        if (tickTimer >= Constants.ACTOR_TICK_RATE)
        {
            tickTimer = 0;
            Tick();

            //  If we have a valid path, move along it
            if (HasValidPath())
            {
                //  Attempt to move to the next point
                if (SetPosition( currentPath[0].x, currentPath[0].y ))
                {
                    currentPath.RemoveAt(0);

                    pathWaitTries = 0;
                    pathRepathTries = 0;
                }
                //  Unable to reach the next point
                else
                {
                    pathWaitTries++;

                    // Wait some time to see if path clears
                    if (pathWaitTries > Constants.PATH_WAIT_TRIES)
                    {
                        //  Path isn't clearing, try repathing
                        if (pathRepathTries < Constants.PATH_REPATH_TRIES)
                            Goto( currentPath[currentPath.Count - 1].x + Random.Range(-1, 1), currentPath[currentPath.Count - 1].y + Random.Range(-1, 1) );
                        //  Give up after repathing a number of times
                        else
                        {
                            currentPath = null;
                            pathWaitTries = 0;
                            pathRepathTries = 0;
                        }

                        pathRepathTries++;
                    }
                }

                //  Don't hang onto an empty path. Save a little memory
                if (currentPath != null && currentPath.Count == 0)
                    currentPath = null;
            }
        }

        //  Interpolate movement
        if (transform.position.x != gridPosition.x && transform.position.z != gridPosition.y)
        {
            transform.position = Vector3.Lerp(transform.position, new Vector3(gridPosition.x, transform.position.y, gridPosition.y) + World.GetPositionOffset(), Time.fixedDeltaTime * movementInterpolation);
        }
    }

    //  Debug drawing
    public virtual void OnDrawGizmosSelected()
    {
        if (Application.isEditor != true) return;

        if (currentPath != null)
        {
            foreach (Cell cell in currentPath)
            {
                Gizmos.color = Color.cyan;
                Gizmos.DrawSphere(new Vector3(cell.x, 0, cell.y) + World.GetPositionOffset(), 0.25f);
            }
        }
    }
#endregion
}

}