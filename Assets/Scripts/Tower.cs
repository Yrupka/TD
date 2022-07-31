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
    private new string name;
    private Texture2D[] upgrades;
    private int upgradeNumber;
    private float shootTimer;

    public Action upgraded;

    public void SetStats(string name, int attack, int range, float attackSpeed, int poison, bool magic)
    {
        upgrades = new Texture2D[3];
        upgradeNumber = -2;

        this.name = name;
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

    public void SetUpgrade(Texture2D[] upgrades)
    {
        this.upgrades = upgrades;
        upgradeNumber = upgrades == null ? -2 : -1;
    }

    public Texture2D[] GetUpgrade()
    {
        return upgrades;
    }

    public void UpgradeChoice(int num)
    {
        upgradeNumber = num;
        upgraded?.Invoke();
    }

    public int GetUpgradeNumber()
    {
        return upgradeNumber;
    }
    // --- Атака вышки ---
    private void Update()
    {
        // if (Input.GetMouseButtonDown(0))
        // {
        //     Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        //     if (Physics.Raycast(ray, out RaycastHit raycastHit, float.MaxValue))
        //     {
        //         Bullet.Create(transform.position, raycastHit.point);
        //     }
        // }
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