using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// способность персонажа, каждая способность имеет список эффектов, которая она накладывает на некоторых персонажей
[Serializable]
public class Spell
{
    [SerializeField]
    private Texture2D icon;
    [SerializeField]
    private string description;
    [SerializeField]
    private EffectDescription[] effects;

    public void Init()
    {
        
    }
}
