using UnityEngine;
using System;

public class Tower : MonoBehaviour
{
    // --- Создание вышки ---
    private int attack; // размер атаки
    private int range; // дальность атаки
    private float attackSpeed; // скорость атак
    private int poison; // количество урона от яда, если есть
    private bool magic; // вышка магического типа?
    private int level; // уровень вышки (базовых типов)
    public int Level { get { return level; } }
    private new string name;
    public string Name { get { return name; } }
    private Texture2D[] upgrades;
    public Texture2D[] Upgrades
    {
        get { return upgrades; }
        set
        {
            upgrades = value;
            upgradeNumber = upgrades == null ? -2 : -1;
        }
    }
    private int upgradeNumber;
    public int UpgradeNumber
    {
        get { return upgradeNumber; }
        set
        {
            upgradeNumber = value;
            upgraded?.Invoke();
        }
    }
    private float shootTimer;

    public Action upgraded;

    public static Transform Create(Transform model, Vector3 pos)
    {
        Transform created = Instantiate(model, pos, Quaternion.identity);
        return created;
    }

    public void SetStats(string name, int level, int attack, int range, float attackSpeed, int poison, bool magic)
    {
        upgrades = new Texture2D[3];
        upgradeNumber = -2;

        this.name = name;
        this.level = level;
        this.attack = attack;
        this.range = range;
        this.attackSpeed = attackSpeed;
        this.poison = poison;
        this.magic = magic;
    }

    public void GetStats(out string name, out int attack, out int range, out float attackSpeed, out int poison, out bool magic)
    {
        name = this.name;
        attack = this.attack;
        range = this.range;
        attackSpeed = this.attackSpeed;
        poison = this.poison;
        magic = this.magic;
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