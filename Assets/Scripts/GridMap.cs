using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridMap
{
    private int width;
    private int height;
    private float size;
    private Vector3 originPos;
    private int[,] gridMap;

    public GridMap(int width, int height, float size, Vector3 originPos)
    {
        this.width = width;
        this.height = height;
        this.size = size;
        this.originPos = originPos;

        gridMap = new int[width, height];

        // 5 поставленных башен, 4 башни станут камнями, 1 останется

        // for (int x = 0; x < width; x++)
        // {
        //     for (int z = 0; z < height; z++)
        //     {
        //         Debug.DrawLine(GetWorldPos(x, z), GetWorldPos(x, z + size), Color.black, 100f);
        //         Debug.DrawLine(GetWorldPos(x, z), GetWorldPos(x + size, z), Color.black, 100f);
        //     }
        //     Debug.DrawLine(GetWorldPos(0, height), GetWorldPos(width, height), Color.black, 100f);
        //     Debug.DrawLine(GetWorldPos(width, 0), GetWorldPos(width, height), Color.black, 100f);
        // }
    }

    // получить глобальные координаты ячейки, а также поставить значение, что ячейка занята
    public Vector3 GetWorldPos(int x, int z)
    {
        return new Vector3(x, 0, z) * size + originPos;
    }

    // получить координаты ячейки на сетке, а также значение - пуста ли она
    public bool GetXZ(Vector3 worldPos, out int x, out int z)
    {
        //if (x >= 0 && z >= 0 && x < width && z < height)
        x = Mathf.FloorToInt((worldPos - originPos).x / size);
        z = Mathf.FloorToInt((worldPos - originPos).z / size);
        
        // 2 - вышка, 1 - камень, 0 - пусто
        if (gridMap[x, z] == 2)
            return false;
        else
            return true;
            
    }

    public void UpdateGrid(Vector2Int[] objects)
    {
        for (int i = 0; i < 5; i++)
        {
            gridMap[objects[i].x, objects[i].y] = 1;
        }
    }
}
