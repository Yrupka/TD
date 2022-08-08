using UnityEngine;

public class Main : MonoBehaviour
{    
    [SerializeField] private UIInterface mInterface;
    [SerializeField] private GridBuildSystem gridBuildSystem;
    [SerializeField] private ObjectStats objectStats;
    [SerializeField] private LevelSystem levelSystem;
    
    private int health = 100;
    private int mana = 5;

    
    void Start()
    {
        gridBuildSystem.Init(mana, 37, 37);
        GridBuildSystem.onManaChange += ChangeMana;
        MainCamera.selected += objectStats.Show;
        TowerSystem.makeRocks += gridBuildSystem.UpdateGrid;
        UIInterface.onStart += StartWave;
        levelSystem.Init();
    }

    private void ChangeMana(int mana)
    {
        mInterface.Mana(mana);
    }

    private void StartWave()
    {
        levelSystem.StartWave(gridBuildSystem.GetPath());
    }
}
