using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLevel
{
    // текущий уровень игрока
    private int playerLevel;
    // количество опыта и денег за противника
    private int expEnemy;
    private int goldEnemy;
    // количество опыта и денег за босса
    private int expBoss;
    private int goldBoss;
    // опыта до следующего уровня
    private int[] expLevelAmount;
    // текущее количество опыта
    private int expCurrent;

    public void Init(int expEnemy, int goldEnemy, int expBoss, int goldBoss)
    {
        playerLevel = 1;
        this.expEnemy = expEnemy;
        this.goldEnemy = goldEnemy;
        this.expBoss = expBoss;
        this.goldBoss = goldBoss;
        expLevelAmount = new int[4] {250, 400, 550, 700};
    }

    private void CheckExp()
    {
        if (playerLevel == 5)
            return;
        if (expCurrent >= expLevelAmount[playerLevel - 1])
        {
            expCurrent -= expLevelAmount[playerLevel - 1];
            playerLevel++;
        }
    }

    public void AddExpEnemy()
    {
        expCurrent += expEnemy;
        CheckExp();
    }

    public void AddExpBoss()
    {
        expCurrent += expBoss;
        CheckExp();
    }

    public int GetCurrentLevel()
    {
        return playerLevel;
    }
    
    public int[,] GetCurrentExp()
    {
        return new int[expCurrent, expLevelAmount[playerLevel - 1]];
    }
}
