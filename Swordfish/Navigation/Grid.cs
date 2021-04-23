using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Swordfish.Navigation
{

public class Grid
{
    private Cell[,] grid;
    private int size;

    public Grid(int size)
    {
        this.size = size;
        grid = new Cell[size, size];

        for (int x = 0; x < size; x++) {
        for (int y = 0; y < size; y++) {
            grid[x, y] = new Cell();
            grid[x, y].grid = this;
            grid[x, y].x = x;
            grid[x, y].y = y;
        } }
    }

    public Cell at(Vector3 pos) { return at((int)pos.x, (int)pos.z); }
    public Cell at(int x, int y) { return grid[ Mathf.Clamp(x, 0, size - 1) , Mathf.Clamp(y, 0, size - 1) ]; }

    public int GetDistance(int x, int y, int x2, int y2)
    {
        int lengthX = Mathf.Abs(x - x2);
        int lengthY = Mathf.Abs(y - y2);

        if (lengthX > lengthY) return lengthX;
        if (lengthY > lengthX) return lengthY;

        return 0;
    }
}

}