using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridBuildSystem : MonoBehaviour
{
    private GridMap grid;
    private TowerSystem towerSystem;
    private Pathfind pathfind;
    private Transform tower;
    private Transform rock;
    private Transform point;
    [SerializeField] private Transform ground;

    private Vector3Int[] pointsPos;
    private List<Vector3> path;

    private int mana = 5;
    public static Action<int> onManaChange;

    private void Start()
    {
        int height = 37;
        int width = 37;

        tower = Resources.Load<Transform>("Prefabs/Tower");
        rock = Resources.Load<Transform>("Prefabs/Rock");
        point = Resources.Load<Transform>("Prefabs/Point");

        pointsPos = new Vector3Int[7] {
            new Vector3Int(4, 31), new Vector3Int(4, 18), new Vector3Int(32, 20),
            new Vector3Int(32, 32), new Vector3Int(20, 32), new Vector3Int(18, 4),
            new Vector3Int(32, 4)
        };
        path = new List<Vector3>();

        grid = new GridMap(height, width, 1f, new Vector3(-height / 2f, 0, -width / 2f));
        ground.localScale = new Vector3(height / 10f, 1f, width / 10f);
        ground.GetComponent<Renderer>().material.mainTextureScale = new Vector2(height, width);
        CreatePoints(pointsPos);

        pathfind = new Pathfind(grid);
        towerSystem = new TowerSystem();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit raycastHit, float.MaxValue, LayerMask.GetMask("Ground")))
            {
                BuildTower(raycastHit.point);
            }
        }
        // if (Input.GetMouseButtonDown(2))
        // {
        //     Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        //     if (Physics.Raycast(ray, out RaycastHit raycastHit, float.MaxValue, LayerMask.GetMask("Ground")))
        //     {
        //         Transform spawned = Instantiate(enemy, raycastHit.point, Quaternion.identity);
        //         enemySystem.Add(spawned);
        //     }       
        // }
    }

    private void CreatePoints(Vector3Int[] pointsPos)
    {
        for (int i = 1; i < 6; i++)
        {
            Transform pointObj = Instantiate(point, grid.GetWorldPos(pointsPos[i].x, pointsPos[i].y), new Quaternion(0f, 0f, 0f, 0f));
            pointObj.Find("Visual").GetComponent<TextMesh>().text = (i).ToString();
            grid.BuildObject(pointObj, 3);
        }

    }

    private void BuildTower(Vector3 globalPos)
    {
        if (mana == 0)
            return;
        if (grid.CanBuild(globalPos))
        {
            grid.GetXZ(globalPos, out int x, out int z);
            grid.TempBuild(x, z);
            
            if (UpdateGridSector(x, z))
            {
                grid.UndoBuild(x, z);
                Transform builded = Instantiate(tower, grid.GetWorldPos(x, z), Quaternion.identity);
                onManaChange?.Invoke(--mana);
                towerSystem.Add(builded);
                grid.BuildObject(builded, 2);
            }
            else
                grid.UndoBuild(x, z);
        }
        else
        {
            //todo сообщение нельзя тут строить
        }
    }

    public void UpdateGrid(params Vector3[] towersPos)
    {
        grid.DestroyObjects(towersPos);

        foreach (var item in towersPos)
        {
            grid.GetXZ(item, out int x, out int z);
            Transform builded = Instantiate(rock, grid.GetWorldPos(x, z), Quaternion.identity);
            grid.BuildObject(builded, 1);

        }
    }

    public List<Vector3> GetPath()
    {
        return path;
    }

    private bool UpdateGridSector(int x, int z)
    {
        path.Clear();
        List<Vector3>[] paths = new List<Vector3>[6];
        for (int i = 0; i < 6; i++)
        {
            paths[i] = pathfind.FindPath(pointsPos[i], pointsPos[i + 1]);
            if (paths == null)
                return false;
            path.AddRange(paths[i]);
        }
        return true;
    }
}
