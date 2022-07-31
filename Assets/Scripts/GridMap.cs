using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridMap
{
    private int width;
    private int height;
    private float size;
    private Vector3 originPos;
    private Dictionary<Vector2Int, Tile> objects;

    private struct Tile
    {
        public int type;
        public Transform obj;

        public Tile(int type, Transform obj)
        {
            this.type = type;
            this.obj = obj;
        }
    }

    public GridMap(int width, int height, float size, Vector3 originPos)
    {
        this.width = width;
        this.height = height;
        this.size = size;
        this.originPos = originPos;

        objects = new Dictionary<Vector2Int, Tile>();
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

    public bool CanBuild(Vector3 pos)
    {
        GetXZ(pos, out int x, out int z);
        if (objects.ContainsKey(new Vector2Int(x, z)))
            return false;
        else
            return true;
    }
    public void BuildObject(Transform obj, int type)
    {
        GetXZ(obj.position, out int x, out int z);
        objects.Add(new Vector2Int(x, z), new Tile(type, obj));

    }

    public void DestroyObjects(params Vector3[] objects)
    {
        for (int i = 0; i < objects.Length; i++)
        {
            GetXZ(objects[i], out int x, out int z);
            GameObject.Destroy(this.objects[new Vector2Int(x, z)].obj.gameObject);
            this.objects.Remove(new Vector2Int(x, z));
        }
    }
}
