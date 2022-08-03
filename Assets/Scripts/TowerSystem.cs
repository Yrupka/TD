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

    private List<Tower> towers;
    private List<int> upgradeNumbers;
    // массивы текстур, моделей, параметров вышек
    private AllTowerList[] allTowerStats;
    private Transform[] allTowerModels;
    private Dictionary<string, Texture2D> textures;

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
        allTowerStats = JsonHelper.FromJson<AllTowerList>(text.text);
        allTowerModels = Resources.LoadAll<Transform>("Towers");
    }

    public Transform Create(Vector3 pos)
    {
        int towerNumber = UnityEngine.Random.Range(0, 2); // тип вышки
        int towerLevel = UnityEngine.Random.Range(1, 6); // уровень от 1 до 5
        Tower tower = MakeTower(pos, towerNumber, towerLevel);

        upgradeNumbers.Add(towerNumber);

        tower.Upgrades = new Texture2D[] { textures[tower.Name] };
        tower.upgraded += CheckTowers;
        towers.Add(tower);
        
        return tower.transform;
    }

    private Tower MakeTower(Vector3 pos, int towerNumber, int level)
    {
        Tower tower = Tower.Create(allTowerModels[towerNumber], pos).GetComponent<Tower>();
        tower.SetStats(allTowerStats[towerNumber].name, allTowerStats[towerNumber].attack, level,
            allTowerStats[towerNumber].range, allTowerStats[towerNumber].attackSpeed,
            allTowerStats[towerNumber].poison, allTowerStats[towerNumber].magic);
        return tower;
    }

    private void CheckUpgrage()
    {

    }

    public void CheckTowers()
    {
        // массив удаляемых вышек
        List<Vector3> rocks = new List<Vector3>();
        for (int i = 0, j = 0; i < towers.Count; i++, j++)
        {
            if (towers[i].UpgradeNumber == -2)
                continue;
            if (towers[i].UpgradeNumber == -1)
            {
                rocks.Add(towers[i].transform.position);
                GameObject.Destroy(towers[i].gameObject);
                towers.Remove(towers[i]);
                i--;
            }
            else
            {
                int level = towers[i].Level;
                Tower tower = MakeTower(towers[i].transform.position, upgradeNumbers[j], level);
                GameObject.Destroy(towers[i].gameObject);
                towers.Remove(towers[i]);
                towers.Add(tower);
            }
        }
        makeRocks?.Invoke(rocks.ToArray());
    }
}
