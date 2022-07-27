using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerSystem : MonoBehaviour
{
    private List<Tower> towers = new List<Tower>();
    
    public void AddTower(Tower tower)
    {
        towers.Add(tower);
    }

    public void RemoveTower(Tower tower)
    {
        towers.Remove(tower);
    }

    public void CheckTowers()
    {
        foreach (var item in towers)
        {
            
        }
    }

    private void Update()
    {
        
    }
}
