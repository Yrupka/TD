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

        allEnemyModels = Resources.LoadAll<Transform>("Prefabs");
    }

    private void Create(Enemy enemy)
    {
        int enemyNumber = UnityEngine.Random.Range(0, 2);

        string newEnemyName = allEnemyStats[enemyNumber].name;
        enemy.SetStats(newEnemyName, allEnemyStats[enemyNumber].health,
            allEnemyStats[enemyNumber].armor, allEnemyStats[enemyNumber].magicArmor,
            allEnemyStats[enemyNumber].speed);
    }

    public void Add(Transform body)
    {
        Enemy enemy = body.GetComponent<Enemy>();
        Create(enemy);
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
}
