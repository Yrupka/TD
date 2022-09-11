using System;
using System.Collections;
using UnityEngine;


[Serializable]
public class EffectDescription
{
    public Texture2D icon;
    public float duration;
    public float value;
    public byte variant;
}

[Serializable]
public class Effect
{
    protected Texture2D icon;
    protected float duration;
    protected float maxDuration;

    public void Update(float val)
    {
        duration -= val;
    }
    
    public int Procent()
    {
        return (int)(duration / maxDuration * 100f);
    }
}

[Serializable]
public class EffectHealth : Effect
{
    private Enemy enemy;
    private int value;

    public EffectHealth(Enemy enemy, float duration, int value)
    {
        this.enemy = enemy;
        this.duration = duration;
        this.maxDuration = duration;
        this.value = value;
    }

    public void OneSecondUpdate()
    {
        enemy.CurrentHealth -= value;
    }

}
