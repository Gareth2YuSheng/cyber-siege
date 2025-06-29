using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using TMPro;
using DG.Tweening;

public class UIManager : MonoBehaviour
{
    public static UIManager main;

    [Header("References")]
    [SerializeField] private GameObject gameOverMenu;
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private Button pauseButton;
    [SerializeField] private GameObject towerMenu;
    [SerializeField] private Button startButton;
    [SerializeField] private GameObject errorPrompt;
    [SerializeField] private TextMeshProUGUI errorPromptLabel;
    [SerializeField] private GameObject towerUpgradeMenu;
    [SerializeField] private RectTransform towerUpgradeMenuTransform;
    [SerializeField] private GameObject levelEndMenu;
    [SerializeField] private GameObject levelPrompt;
    [SerializeField] private GameObject healthHUD;
    // For Scam Message
    [SerializeField] private GameObject susMessageAlertPrefab;
    // For Ransomare
    [SerializeField] private GameObject ransomwarePrompt;


    // private TowerUpgradeMenuScript upgradeMenuScript;

    private void Awake()
    {
        main = this;
    }

    private void Start()
    {
        // upgradeMenuScript = towerUpgradeMenu.GetComponent<TowerUpgradeMenuScript>();
        // Add Event Listeners
        LevelManager.main.onServerDeath.AddListener(GameOver);
        EnemyManager.main.onWaveEnd.AddListener(WaveEnded);
        // For tower upgrade menu
        BuildManager.main.onTowerSelectedForUpgrading.AddListener(ShowTowerUpgradeMenu);
        BuildManager.main.onCancelTowerUpgrading.AddListener(HideTowerUpgradeMenu);
        // For Level End Menu
        EnemyManager.main.onLevelEnd.AddListener(ShowLevelEndMenu);

        // For RansomarePrompt
        EnemyManager.main.onRansomwareClick.AddListener(ShowRansomwarePrompt);

        // Hide Tower Upgrade Menu
        // HideTowerUpgradeMenu();
    }

    public void UpdateHUDLabels()
    {
        towerMenu.GetComponent<TowerMenuScript>().UpdateCurrencyLabel();
        healthHUD.GetComponent<HealthHUDScript>().UpdateHealthLabel();
    }

    private void SetAllSelectableChildrenFromTowerMenu(bool state)
    {
        Selectable[] uiElements = towerMenu.GetComponentsInChildren<Selectable>();
        foreach (Selectable element in uiElements)
        {
            element.interactable = state;
        }
    }

    public void DisableTowerMenu()
    {
        SetAllSelectableChildrenFromTowerMenu(false);
    }

    public void EnableTowerMenu()
    {
        SetAllSelectableChildrenFromTowerMenu(true);
    }

    private void GameOver()
    {
        gameOverMenu.SetActive(true);
        SetAllSelectableChildrenFromTowerMenu(false);
        pauseButton.interactable = false;
    }

    private void WaveEnded()
    {
        if (!EnemyManager.main.HasLevelEnded())
        {
            startButton.gameObject.SetActive(true);
        }
    }

    public void DisableStartWaveButton()
    {
        startButton.interactable = false;
    }

    public void EnableStartWaveButton()
    {
        startButton.interactable = true;
    }

    public void ShowLevelPrompt()
    {
        levelPrompt.SetActive(true);
    }

    public void HideLevelPrompt()
    {
        levelPrompt.SetActive(false);
    }

    IEnumerator SetPromptTimeout(float timeoutDuration)
    {
        yield return new WaitForSeconds(timeoutDuration);
        FadeErrorPrompt(0f, 1f, () =>
        {
            errorPrompt.SetActive(false);
        });
    }

    private void FadeErrorPrompt(float endVal, float duration, TweenCallback onEnd)
    {
        errorPrompt.GetComponent<Image>().DOFade(endVal, duration).onComplete += onEnd;
        errorPromptLabel.DOFade(endVal, duration);
    }

    // Error prompt implementation
    // Shows for specific number of seconds and prompt given.
    public void ShowErrorPrompt(string prompt)
    {
        errorPromptLabel.text = prompt;
        FadeErrorPrompt(1f, 0f, () =>
        {
            errorPrompt.SetActive(true);
        });
        StartCoroutine(SetPromptTimeout(3f));  // Timeout set to 3 seconds
    }

    // For Scam Message
    public void ShowScamMessage()
    {
        // Create new alert
        GameObject susMessageAlert = Instantiate(susMessageAlertPrefab, gameObject.transform);
        // Set the position
        susMessageAlert.transform.position = GetRandomScreenPosition();
        // Populate the content
        SusMessageAlertScript msgScript = susMessageAlert.GetComponent<SusMessageAlertScript>();
        msgScript.PopulateMessage();
    }

    private Vector3 GetRandomScreenPosition()
    {
        float x = Random.Range(100f, Screen.width - 300f);
        float y = Random.Range(100f, Screen.height - 200f);
        return new Vector3(x, y, 0f);
    }

    // For Tower Upgrade Menu
    private void ShowTowerUpgradeMenu()
    {
        // Activate before tweening if not animation wont play
        towerUpgradeMenu.SetActive(true);
        MoveTowerUpgradeMenu(-762f, 0.4f, () =>
        {

        });
    }

    private void HideTowerUpgradeMenu()
    {
        MoveTowerUpgradeMenu(-1222f, 0.4f, () =>
        {
            towerUpgradeMenu.SetActive(false);
        });
    }

    private void MoveTowerUpgradeMenu(float endVal, float duration, TweenCallback onEnd)
    {
        towerUpgradeMenuTransform.DOAnchorPosX(endVal, duration).onComplete += onEnd;
    }

    // For Ransomware
    public void ShowRansomwarePrompt()
    {
        ransomwarePrompt.SetActive(true);
    }

    public void CloseRansomwarePrompt()
    {
        ransomwarePrompt.SetActive(false);
    }

    private void ShowLevelEndMenu()
    {
        // Pause the game after level ended
        Time.timeScale = 0;
        levelEndMenu.SetActive(true);
    }

    // ON CLICK METHODS

    public void StartButtonOnClick()
    {
        EnemyManager.main.StartWave();
        // Hide Start wave button
        startButton.gameObject.SetActive(false);
    }

    public void PauseButtonOnClick()
    {
        pauseMenu.gameObject.SetActive(true);

        Time.timeScale = 0;
        //Disable Tower Menu
        SetAllSelectableChildrenFromTowerMenu(false);
    }

    public void PauseMenuContinueButtonOnClick()
    {
        pauseMenu.gameObject.SetActive(false);
        Time.timeScale = 1;
        //Enable Tower Menu
        SetAllSelectableChildrenFromTowerMenu(true);
    }

    public void PauseMenuExitLevelButtonOnClick()
    {
        SceneManager.LoadSceneAsync("LevelSelectMenu");
        // Unpause the game after exiting
        Time.timeScale = 1;
    }

    public void GameOverMenuExitOnClick()
    {
        Application.Quit();
    }

}
