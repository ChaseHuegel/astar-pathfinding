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
    private bool frozen = false;

    public bool HasValidPath() { return (currentPath != null && currentPath.Count > 0); }

    public void Freeze() { frozen = true; }
    public void Unfreeze() { frozen = false; UpdatePosition(); }
    public void ToggleFreeze()
    {
        if (frozen = !frozen == false) UpdatePosition();
    }


#region immutable methods

    public void UpdatePosition()
    {
        HardSnapToGrid();
        ResetPathing();
    }

    public void ResetAI()
    {
        pathWaitTries = 0;
        pathRepathTries = 0;
    }

    public void ResetPathing()
    {
        currentPath = null;
        ResetAI();
    }

    //  Pathfind to a position
    public void Goto(Direction dir, int distance, bool ignoreActors = true) { Goto(dir.toVector3() * distance, ignoreActors); }
    public void Goto(Vector2 vec, bool ignoreActors = true) { Goto((int)vec.x, (int)vec.y, ignoreActors); }
    public void Goto(Vector3 vec, bool ignoreActors = true) { Goto((int)vec.x, (int)vec.z, ignoreActors); }
    public void Goto(int x, int y, bool ignoreActors = true)
    {
        PathManager.RequestPath(this, x, y, ignoreActors);
    }
#endregion


#region monobehavior

    //  Perform ticks at a regular interval. FixedUpdate is called 60x/s
    public void FixedUpdate()
    {
        tickTimer++;
        if (tickTimer >= Constants.ACTOR_TICK_RATE)
        {
            tickTimer = 0;
            Tick();

            //  If we have a valid path, move along it
            if (HasValidPath() && !frozen)
            {
                //  Attempt to move to the next point
                if (SetPosition( currentPath[0].x, currentPath[0].y ))
                {
                    currentPath.RemoveAt(0);

                    ResetAI();
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
                            Goto(
                                currentPath[currentPath.Count - 1].x + Random.Range(-1, 1),
                                currentPath[currentPath.Count - 1].y + Random.Range(-1, 1),
                                false    //  false, dont ignore actors. Stuck and may need to path around them
                                );
                        //  Give up after repathing a number of times
                        else
                        {
                            ResetPathing();
                        }

                        pathRepathTries++;
                    }
                }

                //  Don't hang onto an empty path. Save a little memory
                if (currentPath != null && currentPath.Count == 0)
                    ResetPathing();
            }
        }

        //  Interpolate movement as long as we're not frozen
        if (!frozen && transform.position.x != gridPosition.x && transform.position.z != gridPosition.y)
        {
            transform.position = Vector3.Lerp(transform.position, World.ToTransformSpace(new Vector3(gridPosition.x, transform.position.y, gridPosition.y)), Time.fixedDeltaTime * movementInterpolation);
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
                Gizmos.DrawSphere(World.ToTransformSpace(new Vector3(cell.x, 0, cell.y)), 0.25f * World.GetUnit());
            }
        }
    }
#endregion
}

}