using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSystem
{
    [SerializeField] private EnemySystem enemySystem;
    private int wavesAmount;
    private int enemyCount;
    private int waveNum;

    public LevelSystem()
    {
        wavesAmount = enemySystem.GetWavesNumber();
        enemyCount = 10;
        waveNum = 0;
    }

    public void StartWave()
    {
        for (int i = 0; i < 1; i++)
        {
            enemySystem.Spawn(waveNum);
        }
        waveNum++;
    }

}
