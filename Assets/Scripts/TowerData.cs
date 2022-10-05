using UnityEngine;

[CreateAssetMenu(fileName = "New TowerData", menuName = "Tower Data", order = 51)]
public class TowerData : ScriptableObject
{
    [SerializeField] private Transform model; // модель
    public Transform Model { get { return model; } }
    [SerializeField] private int attack; // размер атаки
    public int Attack { get { return attack; } }
    [SerializeField] private float range; // дальность атаки
    public float Range { get { return range / 100f; } }
    [SerializeField] private float attackSpeed; // скорость атак
    public float AttackSpeed { get { return 170f / attackSpeed; } }
    [SerializeField] private int level; // уровень вышки (базовых типов)
    public int Level { get { return level; } }
    [SerializeField] private new string name; // имя вышки
    public string Name { get { return name; } }
    [SerializeField] private int targets; // количество атакуемых врагов
    public int Targets { get { return targets; } }
    public Texture2D[] upgrades; // картинки возможных улучшений
    public int[] upgradesNum; // индексы доступных улучшений

    [SerializeField] private Spell[] spells; // способности вышки
    public Spell[] Spells { get {return spells; } }
}