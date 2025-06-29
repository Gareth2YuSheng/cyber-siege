using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TutorialManager : MonoBehaviour
{
    public static TutorialManager main;

    [Header("References")]
    [SerializeField] private GameObject welcomePrompt;
    [SerializeField] private GameObject prompt;
    [SerializeField] private GameObject promptWithButton;
    [SerializeField] private Image[] arrows;

    private Button welcomePromptButton;
    private TextMeshProUGUI promptMessage;
    private TextMeshProUGUI promptWithButtonMessage;
    private Button promptButton;

    private bool hasPlayerContinued;
    private bool hasSelectedTower;
    private bool hasPlacedTower;
    private bool waveStarted;
    // private bool waveCleared;
    private int currentActiveArrowIdx = -1;
    private bool hasSeenEnemy;
    private bool hasEarnedCurrency;

    private void Awake()
    {
        main = this;
    }

    private void Start()
    {
        // Make sure to hide the prompts
        // welcomePrompt.SetActive(false);
        // prompt.SetActive(false);
        // promptWithButton.SetActive(false);

        // Get Components
        welcomePromptButton = welcomePrompt.GetComponentInChildren<Button>();
        promptMessage = prompt.GetComponentInChildren<TextMeshProUGUI>();
        promptWithButtonMessage = promptWithButton.GetComponentInChildren<TextMeshProUGUI>();
        promptButton = promptWithButton.GetComponentInChildren<Button>();

        // Set Health and Currency
        LevelManager.main.IncreaseCurrency(50);
        LevelManager.main.HealServer(1);
        UIManager.main.UpdateHUDLabels();
        // Set Max Wave Count
        EnemyManager.main.SetMaxWaveCount(3);

        StartCoroutine(StartTutorial());
    }

    private IEnumerator StartTutorial()
    {
        // Disable Tower Menu until wave started
        UIManager.main.DisableTowerMenu();
        UIManager.main.DisableStartWaveButton();
        // Welcome Message
        yield return WelcomeMessage();
        UIManager.main.EnableStartWaveButton();
        // Click Start Wave button
        yield return StartWaveInstructions();
        // See Enemy
        yield return WaitForEnemySeen();
        // Build Tower
        UIManager.main.EnableTowerMenu();
        yield return TowerBuildInstructions();
        // Disable start wave button so that people can follow the tutorial
        UIManager.main.DisableStartWaveButton();
        // Earn Currency
        yield return EarnCurrencyExplanation();
        // Other stuff
        yield return RestOfTheInstructions();
        // Enable it back so we can continue with the tutorial
        UIManager.main.EnableStartWaveButton();
    }

    private IEnumerator WelcomeMessage()
    {
        hasPlayerContinued = false;
        welcomePrompt.SetActive(true);
        welcomePromptButton.onClick.RemoveAllListeners();
        welcomePromptButton.onClick.AddListener(() => hasPlayerContinued = true);

        yield return new WaitUntil(() => hasPlayerContinued);

        welcomePrompt.SetActive(false);
    }

    private IEnumerator StartWaveInstructions()
    {
        ShowPromptWithArrow("Click on the Start button to start the wave.", NextArrow());
        waveStarted = false;
        EnemyManager.main.onWaveStart.AddListener(OnWaveStart);
        yield return new WaitUntil(() => waveStarted);
        EnemyManager.main.onWaveStart.RemoveListener(OnWaveStart);
        ClosePromptWithArrow();
    }

    private IEnumerator WaitForEnemySeen()
    {
        hasSeenEnemy = false;
        yield return new WaitUntil(() => hasSeenEnemy);

        // Pause game and show prompt
        Time.timeScale = 0;
        hasPlayerContinued = false;
        ShowPromptWithButtonWithArrow("Oh no! Looks like some malware has entered your network. Build towers to neutralize these threats before they reach your server.", NextArrow());
        yield return new WaitUntil(() => hasPlayerContinued);
        ClosePromptWithArrow();
    }

    private IEnumerator TowerBuildInstructions()
    {
        // Instruct Player to select tower
        hasSelectedTower = false;
        ShowPromptWithArrow("This is the Antivirus Tower, let's build it to stop that malware. Look's like we have enough money to afford it. Click on the Antivirus Tower.", NextArrow());
        BuildManager.main.onTowerSelectedForBuilding.AddListener(OnTowerSelected);
        yield return new WaitUntil(() => hasSelectedTower);
        BuildManager.main.onTowerSelectedForBuilding.RemoveListener(OnTowerSelected);
        ClosePromptWithArrow();

        // Instuct Player to place tower
        ShowPromptWithArrow("Now let's build the tower by placing it on this tile here.", NextArrow());
        hasPlacedTower = false;
        BuildManager.main.onTowerBuilt.AddListener(OnTowerBuilt);
        yield return new WaitUntil(() => hasPlacedTower);
        BuildManager.main.onTowerBuilt.RemoveListener(OnTowerBuilt);
        ClosePromptWithArrow();
        Time.timeScale = 1;
    }

    private IEnumerator EarnCurrencyExplanation()
    {
        hasEarnedCurrency = false;
        LevelManager.main.onCurrencyChange.AddListener(OnEarnCurrency);
        yield return new WaitUntil(() => hasEarnedCurrency);
        LevelManager.main.onCurrencyChange.RemoveListener(OnEarnCurrency);
        hasPlayerContinued = false;
        ShowPromptWithButtonWithArrow("Towers cost money to build. We can earn more money by eliminating threats, and using that money to build more towers.", NextArrow());
        yield return new WaitUntil(() => hasPlayerContinued);
        ClosePromptWithArrow();
    }

    private IEnumerator RestOfTheInstructions()
    {
        // This clashes with the earn currency listener so we can just assume the player is done
        // waveCleared = false;
        // EnemyManager.main.onWaveEnd.AddListener(OnWaveEnd);
        // yield return new WaitUntil(() => waveCleared);
        // EnemyManager.main.onWaveEnd.RemoveListener(OnWaveEnd);
        hasPlayerContinued = false;
        ShowPromptWithButton("Congratulations! You have successfully fended off the malware, now keep going and good luck.");
        yield return new WaitUntil(() => hasPlayerContinued);
        ClosePrompt();
    }

    private void ShowPromptWithButton(string message)
    {
        promptWithButtonMessage.text = message;
        promptWithButton.SetActive(true);
        promptButton.onClick.RemoveAllListeners();
        promptButton.onClick.AddListener(() => hasPlayerContinued = true);
    }

    private void ShowPromptWithArrow(string message, int arrowIdx)
    {
        promptMessage.text = message;
        prompt.SetActive(true);
        arrows[arrowIdx].gameObject.SetActive(true);
    }

    private void ShowPromptWithButtonWithArrow(string message, int arrowIdx)
    {
        promptWithButtonMessage.text = message;
        promptWithButton.SetActive(true);
        arrows[arrowIdx].gameObject.SetActive(true);
        promptButton.onClick.RemoveAllListeners();
        promptButton.onClick.AddListener(() => hasPlayerContinued = true);
    }

    private void ClosePrompt()
    {
        if (prompt.activeSelf) prompt.SetActive(false);
        if (promptWithButton.activeSelf) promptWithButton.SetActive(false);
    }

    private void ClosePromptWithArrow()
    {
        ClosePrompt();
        arrows[currentActiveArrowIdx].gameObject.SetActive(false);
    }

    private int NextArrow()
    {
        currentActiveArrowIdx++;
        return currentActiveArrowIdx;
    }

    private void OnTowerBuilt()
    {
        hasPlacedTower = true;
    }

    private void OnWaveStart()
    {
        waveStarted = true;
    }

    // private void OnWaveEnd()
    // {
    //     waveCleared = true;
    // }

    public void SeenEnemy()
    {
        hasSeenEnemy = true;
    }

    private void OnTowerSelected()
    {
        hasSelectedTower = true;
    }

    private void OnEarnCurrency()
    {
        hasEarnedCurrency = true;
    }
}
