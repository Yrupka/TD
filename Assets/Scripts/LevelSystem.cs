using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class LevelSystem
{
    // текущий уровень игрока
    private static int playerLevel;
    // количество опыта за противника
    private static int expEnemy;
    // количество опыта за босса
    private static int expBoss;
    // опыта до следующего уровня
    private static int[] expLevelAmount;
    // текущее количество опыта
    private static int expCurrent;

    public static void Init(int eEnemy, int eBoss)
    {
        playerLevel = 1;
        expEnemy = eEnemy;
        expBoss = eBoss;
        expLevelAmount = new int[4] {250, 400, 550, 700};
    }

    private static void CheckExp()
    {
        if (playerLevel == 5)
            return;
        if (expCurrent >= expLevelAmount[playerLevel - 1])
        {
            expCurrent -= expLevelAmount[playerLevel - 1];
            playerLevel++;
        }
    }

    public static void AddExpEnemy()
    {
        expCurrent += expEnemy;
        CheckExp();
    }

    public static void AddExpBoss()
    {
        expCurrent += expBoss;
        CheckExp();
    }

    public static int GetCurrentLevel()
    {
        return playerLevel;
    }
    
    public static int[,] GetCurrentExp()
    {
        return new int[expCurrent, expLevelAmount[playerLevel - 1]];
    }
}
