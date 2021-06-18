using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Swordfish.Navigation
{

public class Obstacle : Body
{
    public bool bakeOnStart = true;
    public bool allowPathThru = false;

    public override void Initialize()
    {
        base.Initialize();

        FetchBoundingDimensions();
        if (bakeOnStart) BakeToGrid();
    }

    private void OnDestroy()
    {
        if (Application.isPlaying && gameObject.scene.isLoaded)
            UnbakeFromGrid();
    }

    public virtual void FetchBoundingDimensions() {}

    public void BakeToGrid()
    {
        Vector3 pos = World.ToWorldSpace(transform.position);

        //  Block all cells within bounds
        Cell cell;
        for (int x = -(int)(boundingDimensions.x/2); x < boundingDimensions.x/2; x++)
        {
            for (int y = -(int)(boundingDimensions.y/2); y < boundingDimensions.y/2; y++)
            {
                cell = World.at( (int)pos.x + x, (int)pos.z + y );

                cell.passable = false;
                cell.canPathThru = allowPathThru;
                cell.occupants.Add(this);
            }
        }

        UpdateTransform();

        // transform.position += new Vector3(boundingOrigin.x, 0f, boundingOrigin.y);

        //-------------------------------------------------------------------------
        // MapTools window has snap settings/tools and for some reason snapping/position here
        // doesn't line up with snapping/position setting in the scene view so just leave it
        // up to the user to snap things to the grid themselves and set the position of buildings
        // etc...

        // gridPosition.x = Mathf.RoundToInt(pos.x);
        // gridPosition.y = Mathf.RoundToInt(pos.z);
        // transform.position = World.ToTransformSpace(new Vector3(gridPosition.x, transform.position.y, gridPosition.y));

        // This should do away with the need to have boundingOrigin settings.
        // Vector3 modPos = transform.position;
        // if (boundingDimensions.x % 2 == 0)
        //     modPos.x = transform.position.x + World.GetUnit() * -0.5f;

        // if (boundingDimensions.y % 2 == 0)
        //     modPos.z = transform.position.z + World.GetUnit() * -0.5f;

        // transform.position = modPos;
    }

    public void UnbakeFromGrid()
    {
        //  Unblock all cells within bounds
        Cell cell;
        for (int x = -(int)(boundingDimensions.x/2); x < boundingDimensions.x/2; x++)
        {
            for (int y = -(int)(boundingDimensions.y/2); y < boundingDimensions.y/2; y++)
            {
                Vector3 pos = World.ToWorldSpace(transform.position);

                cell = World.at( (int)pos.x + x, (int)pos.z + y );

                cell.passable = true;
                cell.canPathThru = false;
                cell.occupants.Remove(this);
            }
        }
    }

    public void OnDrawGizmos()
    {
        if (Application.isEditor != true || Application.isPlaying) return;

        Vector3 worldPos = World.ToWorldSpace(transform.position);

        Vector3 gridPoint = World.ToTransformSpace( worldPos);
        Gizmos.matrix = Matrix4x4.TRS(gridPoint, Quaternion.identity, Vector3.one);
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(new Vector3(boundingOrigin.x, 0f, boundingOrigin.y), new Vector3(boundingOrigin.x, World.GetUnit()*1.5f, boundingOrigin.y));

        Coord2D gridPos = new Coord2D(0, 0);
        gridPos.x = Mathf.FloorToInt( worldPos.x );
        gridPos.y = Mathf.FloorToInt( worldPos.z );

        for (int x = -(int)(boundingDimensions.x/2); x < boundingDimensions.x/2; x++)
        {
            for (int y = -(int)(boundingDimensions.y/2); y < boundingDimensions.y/2; y++)
            {
                gridPoint = World.ToTransformSpace(new Vector3(
                    gridPos.x + x,
                    0f,
                    gridPos.y + y
                    ));

                Gizmos.matrix = Matrix4x4.TRS(gridPoint, Quaternion.identity, Vector3.one);

                Gizmos.color = bakeOnStart ? Color.yellow : Color.red;
                Gizmos.DrawCube(Vector3.zero, World.GetUnit() * 0.25f * Vector3.one);
            }
        }
    }
}

}