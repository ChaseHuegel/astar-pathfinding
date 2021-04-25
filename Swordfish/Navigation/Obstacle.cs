using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Swordfish.Navigation
{

public class Obstacle : Body
{
    public bool bakeOnStart = false;
    public bool rebake = false;
    public Vector2 dimensions;
    public Vector2 offset;

    public override void Initialize()
    {
        base.Initialize();

        if (bakeOnStart) BakeToGrid();
    }

    public void Update()
    {
        if (rebake)
        {
            UnbakeFromGrid();
            BakeToGrid();
            rebake = false;
        }
    }

    public void OnDestroy()
    {
        UnbakeFromGrid();
    }

    public void BakeToGrid()
    {
        //  Block all cells within bounds
        Cell cell;
        for (int x = 0; x < dimensions.x; x++)
        {
            for (int y = 0; y < dimensions.y; y++)
            {
                Vector3 pos = transform.position - World.GetPositionOffset();

                cell = World.GetGrid().at( Mathf.RoundToInt(pos.x + offset.x - Mathf.Floor(dimensions.x/2)) + x, Mathf.RoundToInt(pos.z + offset.y - Mathf.Floor(dimensions.y/2)) + y );
                cell.passable = false;
            }
        }
    }

    public void UnbakeFromGrid()
    {
        //  Unblock all cells within bounds
        Cell cell;
        for (int x = 0; x < dimensions.x; x++)
        {
            for (int y = 0; y < dimensions.y; y++)
            {
                Vector3 pos = transform.position - World.GetPositionOffset();

                cell = World.GetGrid().at( Mathf.RoundToInt(pos.x + offset.x - Mathf.Floor(dimensions.x/2)) + x, Mathf.RoundToInt(pos.z + offset.y - Mathf.Floor(dimensions.y/2)) + y );
                cell.passable = true;
            }
        }
    }

    public void OnDrawGizmos()
    {
        if (Application.isEditor != true) return;

        Gizmos.matrix = Matrix4x4.TRS(transform.position + new Vector3(offset.x / 2, 0, offset.y / 2), Quaternion.identity, Vector3.one);

        Gizmos.color = Color.red;

        //  Draw a rectangle
        Gizmos.DrawLine( new Vector3(-dimensions.x / 2, 0, -dimensions.y / 2), new Vector3(-dimensions.x / 2, 0, dimensions.y / 2) );
        Gizmos.DrawLine( new Vector3(-dimensions.x / 2, 0, -dimensions.y / 2), new Vector3(dimensions.x / 2, 0, -dimensions.y / 2) );
        Gizmos.DrawLine( new Vector3(dimensions.x / 2, 0, dimensions.y / 2), new Vector3(-dimensions.x / 2, 0, dimensions.y / 2) );
        Gizmos.DrawLine( new Vector3(dimensions.x / 2, 0, dimensions.y / 2), new Vector3(dimensions.x / 2, 0, -dimensions.y / 2) );
    }
}

}