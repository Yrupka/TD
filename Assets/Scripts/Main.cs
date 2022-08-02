using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main : MonoBehaviour
{    
    [SerializeField] private UIInterface mInterface;
    [SerializeField] private GridBuildSystem gridBuildSystem;
    [SerializeField] private MainCamera mCamera;
    [SerializeField] private ObjectStats objectStats;
    //[SerializeField] private LevelSystem levelSystem;
    
    private int health = 100;

    
    void Start()
    {
        GridBuildSystem.onManaChange += ChangeMana;
        MainCamera.selected += objectStats.Show;
        TowerSystem.makeRocks += gridBuildSystem.UpdateGrid;
        //UIInterface.onStart += levelSystem.StartWave;
    }

    private void ChangeMana(int mana)
    {
        mInterface.Mana(mana);
    }
}
