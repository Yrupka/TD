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

        public int Check(string name)
        {
            if (name == tower1)
                return 0;
            if (name == tower2)
                return 1;
            if (name == tower3)
                return 2;
            return -1;
        }
    }

    private List<Tower> towers;
    private List<Tower> newPlacedTowers;

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
        tower.upgraded += ChooseOne;
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
    private void CheckUpgrade(List<Tower> towers)
    {
        // позиция улучшаемой вышки среди списка вышек, а также есть ли необходимые вышки для улучшения
        Dictionary<int, bool[]> upgradeCheckSlots = new Dictionary<int, bool[]>();
        foreach (var tower in towers)
        {
            // найдена ли вышка в списке комбинаций
            for (int i = 0; i < allTowerCombination.Length; i++)
            {
                string towerName = tower.Name;
                if (tower.Level != 0)
                    towerName += tower.Level.ToString();
                int upgradePos = allTowerCombination[i].Check(towerName);
                // если имя вышки содержится в списке улучшений, то данный номер запомнить
                if (upgradePos != -1)
                {
                    // если записи о вышке нет, то добавить; обновить список необходимых вышек
                    if (!upgradeCheckSlots.ContainsKey(i))
                        upgradeCheckSlots.Add(i, new bool[3]);
                    upgradeCheckSlots[i][upgradePos] = true;
                }
            }
        }

        // нашлись ли вышки, для которых есть все 3 необходимых башни
        int upgradeNum = 0;
        foreach (var item in upgradeCheckSlots)
        {
            // если комбинаци собрана, то установить возможность улучшения для вышек (дать им кнопку)
            if (item.Value[0] && item.Value[1] && item.Value[2])
            {
                foreach (var tower in towers)
                {
                    for (int i = 0; i < 3; i++)
                    {
                        if (tower.upgrades == null)
                        {
                            tower.upgrades = new Texture2D[3];
                            tower.upgradesNum = new int[3];
                        }
                        if (tower.upgrades[i] == null)
                        {
                            tower.upgrades[i] = textures[allTowerCombination[item.Key].result];
                            tower.upgradesNum[i] = upgradeNum;
                            break;
                        }
                    }
                }
            }
            upgradeNum++;
        }
    }

    // для 5 созданных вышек возможно выбрать 1, которая останется,
    // также возможно собрать комбинаницию из 3 вышек, тогда также останется 1 улучшенная вышка
    public void CheckNewTowers()
    {
        CheckUpgrade(newPlacedTowers);

        foreach (var item in newPlacedTowers)
        {
            int emptySlot = 0;
            if (item.upgrades == null)
            {
                item.upgrades = new Texture2D[3];
                item.upgradesNum = new int[3];
            }
            else
            {
                // если есть улучшения, то первый слот(индекс 0) точно будет занят, ищем пусто место среди оставшихся
                if (item.upgrades[1] == null)
                    emptySlot = 1;
                else
                    emptySlot = 2;
            }
            item.upgrades[emptySlot] = textures[item.Name];
            item.upgradesNum[emptySlot] = -1;
        }
    }

    // Оставить одну, выбранную вышку, удалить остальные, имеющие возможность улучшения
    public void ChooseOne(Tower tower)
    {
        if (tower.upgradesNum[0] == -1)
        {
            int towerIndex = towers.IndexOf(tower);
            towers[towerIndex].upgrades = null;
            towers[towerIndex].upgradesNum = null;
            
            newPlacedTowers.Remove(tower);

            newTowersDelete();
            CheckUpgrade(towers);
            return;
        }

        // улучшаемая вышка удаляется, на ее место ставиться улучшенная
        Tower newTower = MakeTower(tower.transform.position, tower.upgradesNum[0], 0);
        newTower.upgraded += ChooseOne;
        towers.Add(newTower);

        // необходимо запомнить номер вышки, которая выбрана как улучшение
        // чтобы удалить 2 остальных вышки (в возможных улучшениях у них тот же номер)
        int number = tower.upgradesNum[0];
        towers.Remove(tower);
        GameObject.Destroy(tower.gameObject);


        // массив позиций удаляемых вышек
        List<Vector3> rocks = new List<Vector3>();
        // так как улучшаемую вышку уже удалили, то осталось удалить еще 2
        int towersDeleteNum = 0;
        // после удаления одной из них необходимо запомнить ее имя, чтобы не удалить больше одной вышки данного типа
        string deletedTowerName = "";
        for (int i = 0; i < towers.Count; i++)
        {
            if (tower.upgrades == null)
                return;
            for (int k = 0; k < 3; k++)
            {
                if (towers[i].upgradesNum[k] == number && towers[i].Name != deletedTowerName)
                {
                    deletedTowerName = towers[i].Name;
                    rocks.Add(towers[i].transform.position);

                    towers.Remove(towers[i]);
                    GameObject.Destroy(towers[i].gameObject);
                    i--;
                    towersDeleteNum++;
                    break;
                }
            }
            if (towersDeleteNum == 2)
                break;
        }
        makeRocks?.Invoke(rocks.ToArray());
    }

    // если не выбрано улучшение и нужно удалить стандартные вышки
    private void newTowersDelete()
    {
        List<Vector3> rocks = new List<Vector3>();
        foreach (var tower in newPlacedTowers)
        {
            rocks.Add(tower.transform.position);
            towers.Remove(tower);
            GameObject.Destroy(tower.gameObject);
        }
        newPlacedTowers = new List<Tower>();
        makeRocks?.Invoke(rocks.ToArray());
    }
}
