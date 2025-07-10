using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public abstract class LevelManager : MonoBehaviour
{
    [Header("Base Attributes")]
    [SerializeField] protected int waveCount = 10;
    [SerializeField] protected int initialCurrency = 100;
    [SerializeField] protected int initialHealth = 100;
    protected bool hasPlayerContinued;

    protected virtual void Start()
    {
        // Make sure game is unpaused
        Time.timeScale = 1;

        // Set Health and Currency
        CurrencyManager.main.IncreaseCurrency(initialCurrency);
        HealthManager.main.InitServerHealth(initialHealth);
        UIManager.main.UpdateHUDLabels();
        // Set Max Wave Count
        EnemyManager.main.SetMaxWaveCount(waveCount);

        StartCoroutine(StartLevel());
    }

    protected abstract IEnumerator StartLevel();

    protected IEnumerator WaitForPrompt()
    {
        hasPlayerContinued = false;
        // Show the level prompt
        UIManager.main.ShowLevelPrompt();
        Button promptButton = UIManager.main.GetLevelPromptButton();
        // Listen for user to click on the continue button on the prompt
        promptButton.onClick.AddListener(OnUserContinued);
        yield return new WaitUntil(() => hasPlayerContinued);
        promptButton.onClick.RemoveListener(OnUserContinued);
    }

    protected void OnUserContinued()
    {
        hasPlayerContinued = true;
    }
}
