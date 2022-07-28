using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour
{
    private int attack; // размер атаки
    private int poison; // количество урона от яда, если есть
    private bool magic; // вышка магического типа?
    public bool newPlaced; // новый объект или нет?

    public Vector3 position { get; set; }
    public new string name { get; set; }

    public Tower(string name, Vector3 position, int attack, int poison, bool magic)
    {
        this.name = name;
        this.position = position;
        this.attack = attack;
        this.poison = poison;
        this.magic = magic;
        this.newPlaced = true;
    }
}
