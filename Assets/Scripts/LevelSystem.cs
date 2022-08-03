using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSystem : MonoBehaviour
{
    private EnemySystem enemySystem;
    private int wavesAmount;
    private int enemyCount;
    private int waveNum;

    public void Init()
    {
        enemySystem = new EnemySystem();
        wavesAmount = enemySystem.GetWavesNumber();
        enemyCount = 10;
        waveNum = 0;
    }

    public void StartWave(List<Vector3> path)
    {
        for (int i = 0; i < 1; i++)
        {
            StartCoroutine(Spawn(path));
        }
        //waveNum++;
    }
    public IEnumerator Spawn(List<Vector3> path)
    {
        enemySystem.Spawn(waveNum, path);
        yield return new WaitForSeconds(0.5f);
    }

}
