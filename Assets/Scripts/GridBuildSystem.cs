using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GridBuildSystem : MonoBehaviour
{
    private GridMap grid;
    private TowerSystem towerSystem;
    private Pathfind pathfind;
    private Transform rock;
    private Transform point;
    [SerializeField] private Transform ground;

    private Vector3Int[] pointsPos;
    private List<Vector3> path;

    private int mana;
    private int level;
    public Action<int> onManaChange;

    public void Init(int mana, int level, int height, int width)
    {
        this.mana = mana;
        this.level = level;
        rock = Resources.Load<Transform>("Prefabs/Rock");
        point = Resources.Load<Transform>("Prefabs/Point");

        pointsPos = new Vector3Int[7] {
            new Vector3Int(4, 32), new Vector3Int(4, 18), new Vector3Int(32, 18),
            new Vector3Int(32, 32), new Vector3Int(18, 32), new Vector3Int(18, 4),
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

    public void Refresh(int mana, int level)
    {
        this.mana = mana;
        this.level = level;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit raycastHit, float.MaxValue, LayerMask.GetMask("Ground", "Rock")))
            {
                BuildTower(raycastHit.point);
                // вышки можно ставить вместо камней
                if (raycastHit.transform.gameObject.layer == LayerMask.GetMask("Rock"))
                    GameObject.Destroy(raycastHit.transform.gameObject);
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
            pointObj.Find("Visual").GetComponent<TextMeshPro>().text = (i).ToString();
            grid.BuildObject(grid.GetWorldPos(pointsPos[i].x, pointsPos[i].y), 3);
        }

    }

    private void BuildTower(Vector3 globalPos)
    {
        if (mana == 0)
            return;
        // 0 - пусто, 1 - камень, 2 - башня, 3 - точка
        int gridObject = grid.CanBuild(globalPos);
        grid.GetXZ(globalPos, out int x, out int z);

        switch (gridObject)
        {
            case 0:
                grid.TempBuild(x, z);
                if (UpdatePath(x, z))
                {
                    onManaChange?.Invoke(--mana);
                    towerSystem.Create(grid.GetWorldPos(x, z), level);
                    grid.RemoveBuild(x, z);
                    grid.BuildObject(globalPos, 2);
                }
                else
                    grid.RemoveBuild(x, z);
                break;
            case 1:
                onManaChange?.Invoke(--mana);
                towerSystem.Create(grid.GetWorldPos(x, z), level);
                grid.RemoveBuild(x, z);
                grid.BuildObject(globalPos, 2);
                break;
            default:
                PopUpDialog.Create(globalPos, "Тут нельзя строить!");
                break;
        }
        if (mana == 0)
            towerSystem.CheckNewTowers();
    }

    public void UpdateGrid(Vector3[] towers)
    {
        grid.RemoveObjects(towers);

        foreach (var tower in towers)
        {
            grid.GetXZ(tower, out int x, out int z);
            Transform builded = Instantiate(rock, grid.GetWorldPos(x, z), Quaternion.identity);
            grid.BuildObject(tower, 1);
        }
    }

    public List<Vector3> GetPath()
    {
        return path;
    }

    private bool UpdatePath(int x, int z)
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
