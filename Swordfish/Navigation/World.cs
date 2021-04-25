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

    public static Grid GetGrid() { return Instance.grid; }
    public static int GetGridSize() { return Instance.gridSize; }
    public static float GetGridUnit() { return Instance.gridUnit; }
    public static float GetGridScale() { return Instance.gridSize * Instance.gridUnit; }
    public static Vector3 GetGridOffset() { return new Vector3(GetGridScale() * -0.5f, 0f, GetGridScale() * -0.5f); }
    public static Vector3 GetGridOrigin() { return Instance.transform.position; }
    public static Vector3 GetPositionOffset() { return (GetGridOffset() + GetGridOrigin()) * GetGridUnit(); }

    //  Shorthand access to grid
    public static Cell at(int x, int y) { return Instance.grid.at(x, y); }

    private void Start()
    {
        grid = new Grid(gridSize);
    }

    //  Debug draw the grid
    private void OnDrawGizmos()
    {
        if (Application.isEditor != true) return;

        Gizmos.matrix = Matrix4x4.TRS(new Vector3(-0.5f, 0f, -0.5f) + GetGridOrigin() + GetGridOffset(), Quaternion.identity, transform.lossyScale);

        Gizmos.color = Color.yellow;

        //  Bounds
        Gizmos.DrawLine( new Vector3(0, 0, 0), new Vector3(0, 0, GetGridScale()) );
        Gizmos.DrawLine( new Vector3(0, 0, GetGridScale()), new Vector3(GetGridScale(), 0, GetGridScale()));
        Gizmos.DrawLine( new Vector3(GetGridScale(), 0, GetGridScale()), new Vector3(GetGridScale(), 0, 0) );
        Gizmos.DrawLine( new Vector3(GetGridScale(), 0, 0), new Vector3(0, 0, 0) );

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
                    Gizmos.DrawCube(new Vector3(x + 0.5f, 0f, y + 0.5f), new Vector3(GetGridUnit(), 0f, GetGridUnit()));
                }
            }
        }
    }
}

}