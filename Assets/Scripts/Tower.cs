using UnityEngine;
using System;

public class Tower : MonoBehaviour
{
    // --- Создание вышки ---
    [SerializeField] private TowerData towerData;
    private float shootTimer;

    private static Transform towerPrefab;

    // обновилась конкретная вышка, номер отражает номер выбранного варианта улучшения
    public Action<Tower> upgraded;

    public static Transform Create( Transform model, Vector3 pos, int level)
    {
        towerPrefab = Resources.Load<Transform>("Prefabs/Tower");
        Transform created = Instantiate(towerPrefab, pos, Quaternion.identity);
        Transform visual = Instantiate(model);
        visual.SetParent(created.Find("Visual"));
        visual.localPosition = Vector3.zero;
        visual.localScale = Vector3.one;
        visual.parent.GetComponent<Animator>().runtimeAnimatorController = Resources.Load("Towers/Animations/" + model.name) as RuntimeAnimatorController;

        created.Find("Visual").localScale += Vector3.one * level / 10f;
        created.Find("Visual").localPosition = new Vector3(0.5f, 0.2f, 0.5f);
        
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
                    Bullet.Create(transform.position, enemy.GetPosition());
                    enemy.Damage(attack);
                    k++;
                    shootTimer = AttackSpeed;
                }
            }
        }
    }
}