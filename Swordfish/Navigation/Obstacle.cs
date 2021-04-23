using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Swordfish.Navigation
{

public class Obstacle : Actor
{
    public Vector2 dimensions;
    public Vector2 offset;

    public override void Initialize()
    {
        base.Initialize();

        //  Block all cells within bounds
        Cell cell;
        for (int x = 0; x < dimensions.x; x++)
        {
            for (int y = 0; y < dimensions.y; y++)
            {
                cell = World.GetGrid().at( (int)(transform.position.x + offset.x - Mathf.Floor(dimensions.x/2)) + x, (int)(transform.position.z + offset.y - Mathf.Floor(dimensions.y/2)) + y );
                cell.weight = 10;
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