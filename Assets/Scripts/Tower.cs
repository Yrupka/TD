using UnityEngine;
using System;

public class Tower : MonoBehaviour
{
    // --- Создание вышки ---
    // private int attack; // размер атаки
    // public int Attack { get { return attack; } }
    // private float range; // дальность атаки
    // public float Range { get { return range / 100f; } }
    // private float attackSpeed; // скорость атак
    // public float AttackSpeed { get { return 170f / attackSpeed; } }
    // private int level; // уровень вышки (базовых типов)
    // public int Level { get { return level; } }
    // private new string name; // имя вышки
    // public string Name { get { return name; } }
    // private int targets; // количество атакуемых врагов
    // public int Targets {get {return targets; } }
    // public Texture2D[] upgrades; // картинки возможных улучшений
    // public int[] upgradesNum; // индексы доступных улучшений
    TowerData data;
    private float shootTimer;

    // обновилась конкретная вышка, номер отражает номер выбранного варианта улучшения
    public Action<Tower> upgraded;

    public static Transform Create(Transform towerPrefab, Transform model, Vector3 pos, int level, TowerData data)
    {
        Transform created = Instantiate(towerPrefab, pos, Quaternion.identity);
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

        visual.GetComponent<Animator>().runtimeAnimatorController = Resources.Load("Towers/Animations/" + model.name) as RuntimeAnimatorController;
        visual.localScale = Vector3.one * 0.5f + (Vector3.one * level / 10f);
        visual.localPosition = new Vector3(0.5f, 0.2f, 0.5f);
        
        return created;
    }

    // public void SetStats(string name, int level, int attack, float range, float attackSpeed, int targets)
    // {
    //     upgrades = null;
    //     upgradesNum = null;

    //     this.name = name;
    //     this.level = level;
    //     this.attack = attack;
    //     this.range = range;
    //     this.attackSpeed = attackSpeed;
    //     this.targets = targets;
    // }

    // --- Атака вышки ---
    private void Update()
    {
        shootTimer -= Time.deltaTime;

        if (shootTimer <= 0f)
        {
            shootTimer = 0f;
            Collider[] colliders = Physics.OverlapSphere(transform.position, data.Range);
            for (int i = 0, k = 0; i < colliders.Length && k < data.Targets; i++)
            {
                if (colliders[i].TryGetComponent<Enemy>(out Enemy enemy))
                {
                    Bullet.Create(transform.position, enemy.GetPosition());
                    enemy.Damage(data.Attack);
                    k++;
                    shootTimer = data.AttackSpeed;
                }
            }
        }
    }
}