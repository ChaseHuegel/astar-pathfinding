using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Swordfish.Navigation
{

public class World : Singleton<World>
{
    [SerializeField] protected int gridSize = 10;
    [SerializeField] protected float gridUnit = 1;

    private Grid grid;
    public static Grid Grid { get { return Instance.grid; } }

    //  Grid info
    public static float GetUnit() { return Instance.gridUnit; }
    public static float GetScale() { return 1f/Instance.gridUnit; }
    public static Vector3 GetGridOffset() { return new Vector3((GetSize() - 0.5f) * -0.5f, 0f, (GetSize() - 0.5f) * -0.5f); }

    //  World info
    public static float GetSize() { return Instance.gridSize * Instance.gridUnit; }
    public static Vector3 GetOrigin() { return Instance.transform.position; }

    //  Shorthand access to grid
    public static Cell at(int x, int y) { return Grid.at(x, y); }

    //  Convert from grid units to transform units
    public static Vector3 ToTransformSpace(Vector3 pos)
    {
        Vector3 result = (pos + GetOrigin()) * GetUnit() + GetGridOffset();
        result.y = pos.y;
        return result;
    }

    //  Convert from transform units to grid units
    public static Vector3 ToWorldSpace(Vector3 pos)
    {
        Vector3 result = ((pos + World.GetOrigin()) + (Vector3.one * World.GetSize()/2)) / World.GetUnit();
        result.y = pos.y;
        return result;
    }

    private void Start()
    {
        grid = new Grid(gridSize);
    }

    //  Debug draw the grid
    private void OnDrawGizmos()
    {
        if (Application.isEditor != true) return;

        //  Center at 0,0 on the grid
        Gizmos.matrix = Matrix4x4.TRS(ToTransformSpace(Vector3.zero), Quaternion.identity, Vector3.one);
        Gizmos.color = Color.yellow;

        //  Bounds
        Gizmos.DrawLine( new Vector3(0, 0, 0), new Vector3(0, 0, GetSize()) );
        Gizmos.DrawLine( new Vector3(0, 0, GetSize()), new Vector3(GetSize(), 0, GetSize()));
        Gizmos.DrawLine( new Vector3(GetSize(), 0, GetSize()), new Vector3(GetSize(), 0, 0) );
        Gizmos.DrawLine( new Vector3(GetSize(), 0, 0), new Vector3(0, 0, 0) );

        //  Center on the world origin
        Gizmos.matrix = Matrix4x4.TRS(GetOrigin(), Quaternion.identity, Vector3.one);

        //  Grid
        for (int x = 0; x < gridSize; x++)
        {
            for (int y = 0; y < gridSize; y++)
            {
                //  Create a checkered pattern
                bool upper = (x % 2 == 0 && y % 2 != 0);
                bool lower = (x % 2 != 0 && y % 2 == 0);
                Gizmos.color = (upper || lower) ? Color.gray : Color.black;

                if (grid != null && !at(x, y).passable)
                {
                    Gizmos.color = Color.yellow;
                    Gizmos.DrawCube( ToTransformSpace(new Vector3(x, 0f, y)), new Vector3(GetUnit(), 0f, GetUnit()));
                }
            }
        }
    }
}

}