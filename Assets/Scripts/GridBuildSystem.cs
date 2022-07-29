using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridBuildSystem : MonoBehaviour
{
    private GridMap grid;
    private TowerSystem towerSystem;
    [SerializeField] private Transform tower;
    [SerializeField] private Transform rock;
    [SerializeField] private Transform ground;
    
    private int mana = 5;
    public static Action<int> onManaChange;

    private void Start()
    {
        int height = 10;
        int width = 10;

        grid = new GridMap(height, width, 1f, new Vector3(-height / 2f, 0, -width / 2f));
        ground.localScale = new Vector3(height / 10f, 1f, width / 10f);
        ground.GetComponent<Renderer>().material.mainTextureScale = new Vector2(height, width);
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
        //         BuildRock(raycastHit.point);
        //     }       
        // }
    }

    private void BuildTower(Vector3 globalPos)
    {
        if (mana == 0)
            return;
        if (grid.GetXZ(globalPos, out int x, out int z))
        {
            Transform builded = Instantiate(tower, grid.GetWorldPos(x, z), Quaternion.identity);
            onManaChange?.Invoke(--mana);
            towerSystem.AddTower(builded);
        }
    }

    private void UpdateGrid()
    {

    }
}
