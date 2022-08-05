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
        public int magic;
        public int poison;
        public int splash;
    }

    [System.Serializable]
    private class AllTowerCombination
    {
        public string tower1;
        public string tower2;
        public string tower3;
        public string result;
    }

    private List<Tower> towers;
    private List<Tower> newPlacedTowers;
    private List<
    // массивы текстур, моделей, параметров вышек
    private AllTowerList[] allTowerStats;
    private Transform[] allTowerModels;
    private AllTowerCombination[] allTowerCombination;
    private Dictionary<string, Texture2D> textures;

    public static Action<Vector3[]> makeRocks;

    public TowerSystem()
    {
        towers = new List<Tower>();
        newPlacedTowers = new List<Tower>();

        var images = Resources.LoadAll<Texture2D>("Thumbnails");
        textures = new Dictionary<string, Texture2D>();
        foreach (var item in images)
            textures.Add(item.name, item);

        var info = Resources.Load<TextAsset>("towersInfo");
        allTowerStats = JsonHelper.FromJson<AllTowerList>(info.text);
        allTowerModels = Resources.LoadAll<Transform>("Towers");
        var combination = Resources.Load<TextAsset>("towersCombination");
        allTowerCombination = JsonHelper.FromJson<AllTowerCombination>(combination.text);
    }

    public Transform Create(Vector3 pos)
    {
        int towerNumber = UnityEngine.Random.Range(0, allTowerStats.Length); // тип вышки
        int towerLevel = UnityEngine.Random.Range(1, 6); // уровень от 1 до 5
        Tower tower = MakeTower(pos, towerNumber, towerLevel);
        // можно создать 5 вышек за раз
        newPlacedTowers.Add(tower);
        towers.Add(tower);

        return tower.transform;
    }

    private Tower MakeTower(Vector3 pos, int towerNumber, int level)
    {
        Tower tower = Tower.Create(allTowerModels[towerNumber], pos, level).GetComponent<Tower>();
        tower.SetStats(allTowerStats[towerNumber].name, level,
            allTowerStats[towerNumber].attack * level, allTowerStats[towerNumber].range,
            allTowerStats[towerNumber].attackSpeed,
            allTowerStats[towerNumber].poison, allTowerStats[towerNumber].magic);
        return tower;
    }

    // проверка массива вышек на возможные комбинации
    // UpgradeNumber = -2 - только создана, -1 - нет комбинации, номер - номер улучшения из списка
    private Texture2D[] CheckUpgrage(List<Tower> towers)
    {
        Texture2D[] textures;
        foreach (var tower in towers)
        {
            
        }
        return textures;
    }

    // для 5 созданных вышек возможно выбрать 1, которая останется,
    // также возможно собрать комбинаницию из 3 вышек, тогда также останется 1 улучшенная вышка
    public void CheckNewTowers()
    {
        
        // for (int i = 0, j = 0; i < towers.Count; i++, j++)
        // {
        //     Debug.Log(towers[i].UpgradeNumber);
        //     if (towers[i].UpgradeNumber == -2)
        //         continue;
        //     if (towers[i].UpgradeNumber == -1)
        //     {
        //         rocks.Add(towers[i].transform.position);
        //         GameObject.Destroy(towers[i].gameObject);
        //         towers.Remove(towers[i]);
        //         i--;
        //     }
        //     else
        //     {
        //         int level = towers[i].Level;
        //         Tower tower = MakeTower(towers[i].transform.position, towers[i].UpgradeNumber, level);
        //         GameObject.Destroy(towers[i].gameObject);
        //         towers.Remove(towers[i]);
        //         towers.Add(tower);
        //         i++;
        //     }
        // }
        
    }

    // Оставить одну, выбранную вышку, удалить остальные, имеющие возможность улучшения
    public void ChooseOne()
    {
        // массив удаляемых вышек
        List<Vector3> rocks = new List<Vector3>();
        makeRocks?.Invoke(rocks.ToArray());
    }
}
