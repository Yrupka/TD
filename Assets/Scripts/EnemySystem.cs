using System;
using System.Collections.Generic;
using UnityEngine;

public class EnemySystem
{
    [System.Serializable]
    private class AllEnemyList
    {
        public string name;
        public int health;
        public int armor;
        public int magicArmor;
        public int speed;
    }

    private static List<Enemy> enemies;
    private AllEnemyList[] allEnemyStats;
    private Dictionary<string, Transform> allEnemyModels;
    private Transform enemyPrefab;

    public Action allDead;
    public Action<bool> enemyDead;

    public EnemySystem()
    {
        enemies = new List<Enemy>();

        var text = Resources.Load<TextAsset>("enemiesInfo");
        allEnemyStats = JsonHelper.FromJson<AllEnemyList>(text.text);

        var models = Resources.LoadAll<Transform>("Enemies");
        allEnemyModels = new Dictionary<string, Transform>();
        foreach (var item in models)
            allEnemyModels.Add(item.name, item);
        enemyPrefab = Resources.Load<Transform>("Prefabs/Enemy");
    }

    public void Spawn(int waveNumber, List<Vector3> path)
    {
        string enemyName = allEnemyStats[waveNumber].name;
        Enemy enemy = Enemy.Create(enemyPrefab, allEnemyModels[enemyName]).GetComponent<Enemy>();

        enemy.SetStats(enemyName, allEnemyStats[waveNumber].health,
            allEnemyStats[waveNumber].armor, allEnemyStats[waveNumber].magicArmor,
            allEnemyStats[waveNumber].speed, waveNumber % 5 == 0 ? true : false);
        enemy.SetPath(path);
        enemy.isDead += EnemyDead;
        enemies.Add(enemy);
    }

    private void EnemyDead(bool isBoss)
    {
        enemyDead?.Invoke(isBoss);
        if (AllDead())
            allDead?.Invoke();
    }
    
    private bool AllDead()
    {
        foreach (var enemy in enemies)
        {
            if (!enemy.IsDead())
                return false;
        }
        return true;
    }

    public int GetWavesNumber()
    {
        return allEnemyStats.Length;
    }
}
