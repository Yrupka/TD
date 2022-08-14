using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveSystem : MonoBehaviour
{
    private EnemySystem enemySystem;
    private int wavesAmount;
    private int enemyCount;
    private int waveNum;
    
    public Action waveEnded;

    public void Init(int enemies)
    {
        enemySystem = new EnemySystem();
        enemySystem.allDead += EndWave;
        wavesAmount = enemySystem.GetWavesNumber();
        enemyCount = enemies;
        waveNum = 0;
    }

    public void StartWave(List<Vector3> path)
    {
        StartCoroutine(Spawn(path));
        //waveNum++;
    }

    private IEnumerator Spawn(List<Vector3> path)
    {
        for (int i = 0; i < enemyCount; i++)
        {
            enemySystem.Spawn(waveNum, path);
            yield return new WaitForSeconds(0.5f);
        }
    }

    private void EndWave()
    {
        waveEnded?.Invoke();
    }
}
