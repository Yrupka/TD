using UnityEngine;
using System;

public class Tower : MonoBehaviour
{
    // --- Создание вышки ---
    private int attack; // размер атаки
    public int Attack { get { return attack; } }
    private int range; // дальность атаки
    public int Range { get { return range; } }
    private float attackSpeed; // скорость атак
    public float AttackSpeed { get { return attackSpeed; } }
    private int poison; // количество урона от яда, если есть
    public int Poison { get { return poison; } }
    private int magic; // вышка магического типа?
    public int Magic { get { return magic; } }
    private int level; // уровень вышки (базовых типов)
    public int Level { get { return level; } }
    private new string name;
    public string Name { get { return name; } }
    public Texture2D[] upgrades;
    public int[] upgradesNum;

    private float shootTimer;

    // обновилась конкретная вышка, номер отражает номер выбранного варианта улучшения
    public Action<Tower> upgraded;

    public static Transform Create(Transform model, Vector3 pos, int level)
    {
        Transform created = Instantiate(model, pos, Quaternion.identity);
        created.Find("Visual").localScale += Vector3.one * level / 10f;
        created.Find("Visual").localPosition = Vector3.one * 0.5f;
        return created;
    }

    public void SetStats(string name, int level, int attack, int range, float attackSpeed, int poison, int magic)
    {
        upgrades = null;
        upgradesNum = null;

        this.name = name;
        this.level = level;
        this.attack = attack;
        this.range = range;
        this.attackSpeed = attackSpeed;
        this.poison = poison;
        this.magic = magic;
    }

    // --- Атака вышки ---
    private void Update()
    {
        shootTimer -= Time.deltaTime;

        if (shootTimer <= 0f)
        {
            shootTimer = attackSpeed;
            Enemy enemy = GetClosestEnemy();
            if (enemy != null)
            {
                Bullet.Create(transform.position, enemy.GetPosition());
                enemy.Damage(attack);
            }
        }
    }

    private Enemy GetClosestEnemy()
    {
        return EnemySystem.GetClosest(transform.position, range);
    }
}