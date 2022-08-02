using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridBuildSystem : MonoBehaviour
{
    private GridMap grid;
    private TowerSystem towerSystem;
    private EnemySystem enemySystem;
    private Transform tower;
    private Transform rock;
    private Transform point;
    [SerializeField] private Transform ground;
    
    private int mana = 5;
    public static Action<int> onManaChange;

    private void Start()
    {
        int height = 37;
        int width = 37;

        tower = Resources.Load<Transform>("Prefabs/Tower");
        rock = Resources.Load<Transform>("Prefabs/Rock");
        point = Resources.Load<Transform>("Prefabs/Point");
        Vector2Int[] pointsPos = new Vector2Int[5] {
            new Vector2Int(5, 19), new Vector2Int(32, 19),
            new Vector2Int(32, 5), new Vector2Int(19, 5),
            new Vector2Int(19, 32)
        };
        CreatePoints(pointsPos);

        grid = new GridMap(height, width, 1f, new Vector3(-height / 2f, 0, -width / 2f));
        ground.localScale = new Vector3(height / 10f, 1f, width / 10f);
        ground.GetComponent<Renderer>().material.mainTextureScale = new Vector2(height, width);
        
        towerSystem = new TowerSystem();
        enemySystem = new EnemySystem();
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

    private void CreatePoints(Vector2Int[] pointsPos)
    {
        for (int i = 0; i < 5; i++)
        {
            Transform pointObj = Instantiate(point, grid.GetWorldPos(pointsPos[i].x, pointsPos[i].y), Quaternion.identity);
            pointObj.GetComponent<TextMesh>().text = (i + 1).ToString();
            grid.BuildObject(pointObj, 2);
        }
        
    }

    private void BuildTower(Vector3 globalPos)
    {
        if (mana == 0)
            return;
        if (grid.CanBuild(globalPos))
        {
            grid.GetXZ(globalPos, out int x, out int z);
            Transform builded = Instantiate(tower, grid.GetWorldPos(x, z), Quaternion.identity);
            onManaChange?.Invoke(--mana);
            towerSystem.Add(builded);
            grid.BuildObject(builded, 2);
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
}
