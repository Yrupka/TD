using UnityEngine;

public class Main : MonoBehaviour
{    
    [SerializeField] private UIInterface mInterface;
    [SerializeField] private GridBuildSystem gridBuildSystem;
    [SerializeField] private ObjectStats objectStats;
    [SerializeField] private WaveSystem waveSystem;
    
    private int health = 100;
    private int enemies = 10;
    private int mana = 5;

    
    void Start()
    {
        gridBuildSystem.Init(mana, 37, 37);
        GridBuildSystem.onManaChange += ChangeMana;
        MainCamera.selected += objectStats.Show;
        TowerSystem.makeRocks += gridBuildSystem.UpdateGrid;
        UIInterface.onStart += StartWave;
        mInterface.Init(mana, health);
        waveSystem.Init(enemies);
        waveSystem.waveEnded += EndWave;
        LevelSystem.Init(5, 300);
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
        gridBuildSystem.Refresh(mana);
        // todo endwave оповещение
    }
}
