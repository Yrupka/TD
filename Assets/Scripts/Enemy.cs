using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    // --- Создание противника ---
    private new string name;
    public string Name {get { return name; } }
    private int health; // количество здоровья
    public int Health {get { return health; } }
    private int currHealth; // текущее количество здоровья
    public int CurrentHealth {get { return currHealth; } set { currHealth = value; } }
    private int armor; // количество брони
    public int Armor {get { return armor; } }
    private int magicArmor; // количество магической брони
    public int MagicArmor {get { return magicArmor; } }
    private int speed; // скорость передвижения
    public int Speed {get { return speed; } }
    private List<Vector3> path; // путь движения
    private int currentPoint = 0; // текущая точка на поле
    private bool isBoss;

    private Slider healthBar;
    public Action<bool> isDead;

    public static Transform Create(Transform enemyPrefab, Transform model)
    {
        Transform created = Instantiate(enemyPrefab, new Vector3(-14.5f, 0f, 13.5f), Quaternion.identity);
        Transform visual = created.Find("Visual");
        
        Transform visualModel = Instantiate(model);
        for (int i = 0; i < visualModel.childCount; i++)
        {
            Transform child = visualModel.GetChild(i);
            child.SetParent(visual);
            child.localPosition = Vector3.zero;
            i--;
        }
        GameObject.Destroy(visualModel.gameObject);

        visual.GetComponent<Animator>().runtimeAnimatorController = Resources.Load("Enemies/Animations/" + model.name) as RuntimeAnimatorController;
        visual.localPosition = new Vector3(0.5f, 0.2f, 0.5f);
        
        return created;
    }

    public void SetStats(string name, int health, int armor, int magicArmor, int speed, bool isBoss)
    {
        healthBar = transform.Find("Canvas").Find("HealthBar").GetComponent<Slider>();
        healthBar.maxValue = health;
        healthBar.value = health;

        this.name = name;
        this.health = health;
        this.currHealth = health;
        this.armor = armor;
        this.magicArmor = magicArmor;
        this.speed = speed;
        this.isBoss = isBoss;
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
        {
            isDead?.Invoke(isBoss);
            Destroy(gameObject);
        }
            
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
                transform.position = transform.position + moveDir * (speed / 100f) * Time.deltaTime;
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
