using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerSystem
{
    [System.Serializable]
    private class AllTowerList
    {
        public string name;
        public int attack;
        public int range;
        public bool magic;
        public int poison;
        public int splash;
    }

    private enum Towers
    {
        poison,
        vision,
        speed,
        normal,
        splash
    }

    private List<Tower> towers;
    private List<Vector2Int> lastBuilded;
    AllTowerList[] allTowerList;

    public TowerSystem()
    {
        towers = new List<Tower>();
        lastBuilded = new List<Vector2Int>();

        var text = Resources.Load<TextAsset>("towersInfo");
        allTowerList = JsonHelper.FromJson<AllTowerList>(text.text);
    }

    private void CreateTower(Tower tower)
    {
        int towerNumber = Random.Range(0, 2);
        tower.SetStats(allTowerList[towerNumber].name, allTowerList[towerNumber].attack, 
        allTowerList[towerNumber].poison, allTowerList[towerNumber].magic);
    }

    public void AddTower(Transform body)
    {
        Tower tower = body.GetComponent<Tower>();
        CreateTower(tower);
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
}
