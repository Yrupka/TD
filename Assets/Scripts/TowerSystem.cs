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

    private Tower CreateTower(int x, int z)
    {

        int towerNumber = Random.Range(0, 1);
        Tower tower = new Tower(allTowerList[towerNumber].name, new Vector3(x, 0, z), 
            allTowerList[towerNumber].attack, allTowerList[towerNumber].poison,
            allTowerList[towerNumber].magic);
        return tower;
    }

    public void AddTower(int x, int z)
    {
        towers.Add(CreateTower(x, z));
        lastBuilded.Add(new Vector2Int(x, z));
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
