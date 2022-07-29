
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour
{
    private int attack; // размер атаки
    private int poison; // количество урона от яда, если есть
    private bool magic; // вышка магического типа?
    private new string name;

    public void SetStats(string name, int attack, int poison, bool magic)
    {
        this.name = name;
        this.attack = attack;
        this.poison = poison;
        this.magic = magic;
    }
    
    public void GetStats(out string name, out int attack, out int poison, out bool magic)
    {
        name = this.name;
        attack = this.attack;
        poison = this.poison;
        magic = this.magic;
    }

    public void CanUpgrade()
    {
        
    }
}
