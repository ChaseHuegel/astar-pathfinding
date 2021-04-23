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

        Gizmos.matrix = Matrix4x4.TRS(new Vector3(-0.5f, 0f, -0.5f), Quaternion.identity, transform.lossyScale);

        Gizmos.color = Color.yellow;

        //  Bounds
        Gizmos.DrawLine( new Vector3(0, 0, 0), new Vector3(0, 0, gridSize) );
        Gizmos.DrawLine( new Vector3(0, 0, gridSize), new Vector3(gridSize, 0, gridSize));
        Gizmos.DrawLine( new Vector3(gridSize, 0, gridSize), new Vector3(gridSize, 0, 0) );
        Gizmos.DrawLine( new Vector3(gridSize, 0, 0), new Vector3(0, 0, 0) );

        //  Grid
        // for (int x = 0; x < gridSize + 1; x++)
        // {
        //     //  Draw columns
        //     Gizmos.DrawLine( new Vector3(x * gridUnit, 0, 0), new Vector3(x * gridUnit, 0, gridSize * gridUnit) );

        //     for (int z = 0; z < gridSize + 1; z++)
        //     {
        //         //  Draw rows
        //         Gizmos.DrawLine( new Vector3(0, 0, z * gridUnit), new Vector3(gridSize * gridUnit, 0, z * gridUnit) );
        //     }
        // }
    }
}

}