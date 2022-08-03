using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    // --- Создание противника ---

    private new string name;
    private int health; // количество здоровья
    private int currHealth; // текущее количество здоровья
    private int armor; // количество брони
    private int magicArmor; // количество магической брони
    private int speed; // скорость передвижения
    private List<Vector3> path; // путь движения
    private int currentPoint = 0; // текущая точка на поле

    private Slider healthBar;

    public static Transform Create(Transform enemy)
    {
        Transform created = Instantiate(enemy, new Vector3(-10.5f, 0f, 5.5f), Quaternion.identity);
        return created;
    }

    public void SetStats(string name, int health, int armor, int magicArmor, int speed)
    {
        healthBar = transform.Find("Canvas").Find("HealthBar").GetComponent<Slider>();
        healthBar.maxValue = health;
        healthBar.value = health;

        this.name = name;
        this.health = health;
        this.currHealth = health;
        this.armor = armor;
        this.magicArmor = magicArmor;
        this.speed = speed / 20;
    }

    public void GetStats(out string name, out int health, out int armor, out int magicArmor, out int speed)
    {
        name = this.name;
        health = this.health;
        armor = this.armor;
        magicArmor = this.magicArmor;
        speed = this.speed;
    }

    public bool IsDead()
    {
        if (currHealth <= 0)
            return true;
        else
            return false;
    }

    public void Damage(int amount)
    {
        //todo armor resist
        currHealth -= amount;
        healthBar.value = currHealth;

        if (IsDead())
            Destroy(gameObject);
    }

    public Vector3 GetPosition()
    {
        return this.transform.position;
    }

    public void SetPath(List<Vector3> path)
    {
        this.path = path;
    }

    private void Update()
    {
        Move();
    }

    private void Move()
    {
        if (path != null)
        {
            Vector3 target = path[currentPoint];
            if (Vector3.Distance(transform.position, target) > 0.1f)
            {
                Vector3 moveDir = (target - transform.position).normalized;
                float distanseBefore = Vector3.Distance(transform.position, target);
                // todo animation start
                transform.position = transform.position + moveDir * speed * Time.deltaTime;
            }
            else
            {
                currentPoint++;
                if (currentPoint >= path.Count)
                {
                    //todo animation stop
                    StopMove();
                }
            }
        }

    }

    private void StopMove()
    {
        path = null;
        currentPoint = 0;
    }
}
