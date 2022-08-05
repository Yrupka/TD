using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridMap
{
    private int width;
    public int Width { get { return width; } }
    private int height;
    public int Height { get { return height; } }
    private float size;
    public float Size { get { return size; } }
    private Vector3 originPos;
    private Dictionary<Vector2Int, int> objects;

    public GridMap(int width, int height, float size, Vector3 originPos)
    {
        this.width = width;
        this.height = height;
        this.size = size;
        this.originPos = originPos;

        objects = new Dictionary<Vector2Int, int>();
    }

    // получить глобальные координаты ячейки
    public Vector3 GetWorldPos(int x, int z)
    {
        return new Vector3(x, 0, z) * size + originPos;
    }

    // получить координаты ячейки на сетке
    public void GetXZ(Vector3 worldPos, out int x, out int z)
    {
        x = Mathf.FloorToInt((worldPos - originPos).x / size);
        z = Mathf.FloorToInt((worldPos - originPos).z / size);
    }

    public int CanBuild(Vector3 pos)
    {
        GetXZ(pos, out int x, out int z);
        if (objects.ContainsKey(new Vector2Int(x, z)))
            return objects[new Vector2Int(x, z)];
        else
            return 0;
    }

    public bool CanWalk(int x, int z)
    {
        if (objects.ContainsKey(new Vector2Int(x, z)))
        {
            if (objects[new Vector2Int(x, z)] == 3)
                return true;
            else
                return false;
        }
        else
            return true;
    }

    // type = 0 - пусто, 1 - камень, 2 - башня, 3 - точка
    public void BuildObject(Vector3 pos, int type)
    {
        GetXZ(pos, out int x, out int z);
        objects.Add(new Vector2Int(x, z), type);
    }

    public void TempBuild(int x, int z)
    {
        objects.Add(new Vector2Int(x, z), 1);
    }

    public void RemoveBuild(int x, int z)
    {
        objects.Remove(new Vector2Int(x, z));
    }

    public void RemoveObjects(Vector3[] objects)
    {
        for (int i = 0; i < objects.Length; i++)
        {
            GetXZ(objects[i], out int x, out int z);
            this.objects.Remove(new Vector2Int(x, z));
        }
    }
}
