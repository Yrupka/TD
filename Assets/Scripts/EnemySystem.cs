using System.Collections;
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
    private Transform[] allEnemyModels;

    public EnemySystem()
    {
        enemies = new List<Enemy>();

        var text = Resources.Load<TextAsset>("enemiesInfo");
        allEnemyStats = JsonHelper.FromJson<AllEnemyList>(text.text);

        allEnemyModels = Resources.LoadAll<Transform>("Enemies");
    }

    public void Spawn(int enemyNumber, List<Vector3> path)
    {
        Enemy enemy = Enemy.Create(allEnemyModels[enemyNumber]).GetComponent<Enemy>();
        string newEnemyName = allEnemyStats[enemyNumber].name;
        enemy.SetStats(newEnemyName, allEnemyStats[enemyNumber].health,
            allEnemyStats[enemyNumber].armor, allEnemyStats[enemyNumber].magicArmor,
            allEnemyStats[enemyNumber].speed);
        enemy.SetPath(path);
        enemies.Add(enemy);
    }

    public static Enemy GetClosest(Vector3 position, int range)
    {
        Enemy closest = null;
        foreach (Enemy enemy in enemies)
        {
            if (enemy.IsDead()) continue;
            if (Vector3.Distance(position, enemy.GetPosition()) <= range)
            {
                if (closest == null)
                    closest = enemy;
                else
                    if (Vector3.Distance(position, enemy.GetPosition()) <
                        Vector3.Distance(position, closest.GetPosition()))
                    closest = enemy;
            }

        }
        return closest;
    }

    public int GetWavesNumber()
    {
        return allEnemyStats.Length;
    }
}
