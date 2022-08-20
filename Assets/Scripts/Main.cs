using UnityEngine;

public class Main : MonoBehaviour
{
    [SerializeField] private UIInterface mInterface;
    [SerializeField] private GridBuildSystem gridBuildSystem;
    [SerializeField] private ObjectStats objectStats;
    [SerializeField] private WaveSystem waveSystem;
    [SerializeField] private MainCamera mainCamera;
    private PlayerLevel playerLevel;

    private int health = 100;
    private int mana = 5;


    void Start()
    {
        gridBuildSystem.Init(mana, 1, 37, 37);
        gridBuildSystem.onManaChange += ChangeMana;

        mainCamera.Init(37, 37);
        mainCamera.selected += objectStats.Show;
        
        TowerSystem.makeRocks += gridBuildSystem.UpdateGrid;
        mInterface.onStart += StartWave;
        

        mInterface.Init(mana, health);
        waveSystem.Init(10);
        waveSystem.waveEnded += EndWave;
        waveSystem.enemyDead += EnemyDead;
        playerLevel = new PlayerLevel();
        playerLevel.Init(5, 5, 300, 300);
        
    }

    private void ChangeMana(int value)
    {
        mInterface.Mana(value, mana);
        // todo сделать отдельный единичный вызов события, когда мана равно 0
        if (value != 0)
            NoMana(false);
        else
            NoMana(true);
    }
    private void NoMana(bool value)
    {
        mInterface.TowersBuilded(value);
        objectStats.ActionsVisibility(value);
    }

    private void StartWave()
    {
        waveSystem.StartWave(gridBuildSystem.GetPath());
    }

    private void EndWave()
    {
        NoMana(false);
        gridBuildSystem.Refresh(mana, playerLevel.GetCurrentLevel());
        // todo endwave оповещение
    }

    private void EnemyDead(bool isBoss)
    {
        if (!isBoss)
            playerLevel.AddExpEnemy();
        else
            playerLevel.AddExpBoss();
    }
}
