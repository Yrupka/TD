using UnityEngine;

[CreateAssetMenu(fileName = "New TowerData", menuName = "Tower Data", order = 51)]
public class TowerData : ScriptableObject
{
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
    public int Targets {get {return targets; } }
    [SerializeField] public Texture2DArray upgrades; // картинки возможных улучшений
    [SerializeField] public int[] upgradesNum; // индексы доступных улучшений
}
