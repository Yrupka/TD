using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridBuildSystem : MonoBehaviour
{
    private GridMap grid;
    private TowerSystem towerSystem;
    [SerializeField] private Transform tower;
    [SerializeField] private LayerMask layerMask;
    [SerializeField] private Transform ground;
    
    private List<Vector2Int> lastBuilded;
    private int mana = 5;
    public static Action<int> onManaChange;

    private void Start()
    {
        int height = 10;
        int width = 7;
        lastBuilded = new List<Vector2Int>();

        grid = new GridMap(height, width, 1f, new Vector3(-height / 2f, 0, -width / 2f));
        ground.localScale = new Vector3(height / 10f, 1f, width / 10f);
        ground.GetComponent<Renderer>().material.mainTextureScale = new Vector2(height, width);
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit raycastHit, float.MaxValue, layerMask))
            {
                BuildTower(raycastHit.point);
            }       
        }
    }

    private void BuildTower(Vector3 globalPos)
    {
        if (mana == 0)
            return;
        if (grid.GetXZ(globalPos, out int x, out int z))
        {
            Instantiate(tower, grid.GetWorldPos(x, z), Quaternion.identity);
            onManaChange?.Invoke(mana--);
            lastBuilded.Add(new Vector2Int(x, z));
        }   
    }

    private void UpdateGrid()
    {

    }
}
