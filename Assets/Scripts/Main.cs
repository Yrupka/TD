using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main : MonoBehaviour
{    
    [SerializeField] private UIInterface mInterface;
    [SerializeField] private GridBuildSystem gridBuildSystem;
    [SerializeField] private MainCamera mCamera;
    
    private int health = 100;

    
    void Start()
    {
        GridBuildSystem.onManaChange += ChangeMana;
    }

    private void ChangeMana(int mana)
    {
        mInterface.Mana(mana);
    }
}
