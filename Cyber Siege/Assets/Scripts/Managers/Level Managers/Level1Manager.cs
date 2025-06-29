using UnityEngine;

public class Level1Manager : MonoBehaviour
{
    [Header("Attributes")]
    [SerializeField] private int waveCount = 10;
    private void Start()
    {
        // Show the level prompt
        UIManager.main.ShowLevelPrompt();

        // Set Health and Currency
        LevelManager.main.IncreaseCurrency(100);
        LevelManager.main.HealServer(100);
        UIManager.main.UpdateHUDLabels();
        // Set Max Wave Count
        EnemyManager.main.SetMaxWaveCount(waveCount);
    }

}
