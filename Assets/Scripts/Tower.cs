using UnityEngine;
using System;

public class Tower : MonoBehaviour
{
    // --- Создание вышки ---
    private Transform shootPoint;
    private int attack; // размер атаки
    public int Attack { get { return attack; } }
    private float range; // дальность атаки
    public float Range { get { return range / 100f; } }
    private float attackSpeed; // скорость атак
    public float AttackSpeed { get { return 170f / attackSpeed; } }
    private int poison; // количество урона от яда, если есть
    public int Poison { get { return poison; } }
    private int magic; // вышка магического типа?
    public int Magic { get { return magic; } }
    private int level; // уровень вышки (базовых типов)
    public int Level { get { return level; } }
    private new string name;
    public string Name { get { return name; } }
    private int targets;
    public int Targets {get {return targets; } }
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
        Vector3 shootPos = new Vector3(0.5f, 0.7f, 0.5f);
        created.Find("ShootPoint").localPosition = shootPos;
        return created;
    }

    public void SetStats(string name, int level, int attack, float range, float attackSpeed, int poison, int magic, int targets)
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
        this.targets = targets;

        shootPoint = transform.Find("ShootPoint");
    }

    // --- Атака вышки ---
    private void Update()
    {
        shootTimer -= Time.deltaTime;

        if (shootTimer <= 0f)
        {
            shootTimer = 0f;
            Collider[] colliders = Physics.OverlapSphere(transform.position, Range);
            for (int i = 0, k = 0; i < colliders.Length && k < targets; i++)
            {
                if (colliders[i].TryGetComponent<Enemy>(out Enemy enemy))
                {
                    Bullet.Create(shootPoint.position, enemy.GetPosition());
                    enemy.Damage(attack);
                    k++;
                    shootTimer = AttackSpeed;
                }
            }
        }
    }
}