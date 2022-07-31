using System;
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
        public float attackSpeed;
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

    private Dictionary<string, Texture2D> textures;
    private List<Tower> towers;
    private List<int> upgradeNumbers;
    AllTowerList[] allTowerList;

    public static Action<Vector3[]> makeRocks;

    public TowerSystem()
    {
        towers = new List<Tower>();
        upgradeNumbers = new List<int>();

        var images = Resources.LoadAll<Texture2D>("Thumbnails");
        textures = new Dictionary<string, Texture2D>();
        foreach (var item in images)
            textures.Add(item.name, item);
            
        var text = Resources.Load<TextAsset>("towersInfo");
        allTowerList = JsonHelper.FromJson<AllTowerList>(text.text);
    }

    private void Create(Tower tower)
    {
        int towerNumber = UnityEngine.Random.Range(0, 2);
        
        upgradeNumbers.Add(towerNumber);
        
        string newTowerName = allTowerList[towerNumber].name;
        tower.SetStats(newTowerName, allTowerList[towerNumber].attack,
            allTowerList[towerNumber].range, allTowerList[towerNumber].attackSpeed,
            allTowerList[towerNumber].poison, allTowerList[towerNumber].magic);
        
        tower.SetUpgrade(new Texture2D[] {textures[newTowerName]});
        tower.upgraded += CheckTowers;
    }

    public void Add(Transform body)
    {
        Tower tower = body.GetComponent<Tower>();
        Create(tower);
        towers.Add(tower);
    }

    public void CheckTowers()
    {
        // массив удаляемых вышек
        List<Vector3> pos = new List<Vector3>();
        for (int i = 0, j = 0; i < towers.Count; i++, j++)
        {
            if (towers[i].GetUpgradeNumber() == -2)
                continue;
            if (towers[i].GetUpgradeNumber() == -1)
            {
                pos.Add(towers[i].transform.position);
                towers.Remove(towers[i]);
                i--;
            }
            else
            {
                towers[i].SetUpgrade(null);
                towers[i].SetStats(allTowerList[upgradeNumbers[j]].name, allTowerList[upgradeNumbers[j]].attack,
                    allTowerList[upgradeNumbers[j]].range, allTowerList[upgradeNumbers[j]].attackSpeed,
                    allTowerList[upgradeNumbers[j]].poison, allTowerList[upgradeNumbers[j]].magic);
            }
        }
        makeRocks?.Invoke(pos.ToArray());
    }
}
