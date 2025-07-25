using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using TMPro;
using DG.Tweening;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class UIManager : MonoBehaviour
{
    public static UIManager main;

    [Header("References")]
    // For Game Over Menu
    [SerializeField] private GameObject gameOverMenu;
    // For Pause Menu
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private Button pauseButton;
    // For Tower Menu
    [SerializeField] private GameObject towerMenu;
    [SerializeField] private Button startButton;
    // For Error Prompt
    [SerializeField] private GameObject errorPrompt;
    [SerializeField] private TextMeshProUGUI errorPromptLabel;
    // For Tower Upgrade Menu
    [SerializeField] private GameObject towerUpgradeMenu;
    private RectTransform towerUpgradeMenuTransform;
    // For Level End Menu
    [SerializeField] private GameObject levelEndMenu;
    // For Level Prompt
    [SerializeField] private GameObject levelPrompt;
    [SerializeField] private TextMeshProUGUI levelPromptTitle;
    [SerializeField] private TextMeshProUGUI levelPromptBody;
    [SerializeField] private Image levelPromptImage;
    // For Health HUD
    [SerializeField] private GameObject healthHUD;
    // For Scam Message
    [SerializeField] private GameObject susMessageAlertPrefab;
    // For Ransomare
    [SerializeField] private GameObject ransomwarePrompt;
    // For Resource Monitor Prompt
    [SerializeField] private GameObject resourceMonitorPrompt;
    private RectTransform resourceMonitorPromptTransform;
    private ResourceMonitorPromptScript resourceMonitorPromptScript;

    // For blocking clicks from passing through the UI Elements
    private GraphicRaycaster rc;
    private EventSystem es;
    private PointerEventData pointerEventData;

    private void Awake()
    {
        main = this;
    }

    private void Start()
    {
        // upgradeMenuScript = towerUpgradeMenu.GetComponent<TowerUpgradeMenuScript>();
        // Add Event Listeners
        HealthManager.main.onServerDeath.AddListener(GameOver);
        EnemyManager.main.onWaveEnd.AddListener(WaveEnded);
        // For tower upgrade menu
        BuildManager.main.onTowerSelectedForUpgrading.AddListener(ShowTowerUpgradeMenu);
        BuildManager.main.onCancelTowerUpgrading.AddListener(HideTowerUpgradeMenu);
        // For Level End Menu
        EnemyManager.main.onLevelEnd.AddListener(ShowLevelEndMenu);

        // For RansomarePrompt
        EnemyManager.main.onRansomwareClick.AddListener(ShowRansomwarePrompt);

        es = EventSystem.current;
        rc = gameObject.GetComponent<GraphicRaycaster>();

        towerUpgradeMenuTransform = towerUpgradeMenu.GetComponent<RectTransform>();
        resourceMonitorPromptTransform = resourceMonitorPrompt.GetComponent<RectTransform>();
        resourceMonitorPromptScript = resourceMonitorPrompt.GetComponent<ResourceMonitorPromptScript>();
    }

    public void UpdateHUDLabels()
    {
        TowerMenuScript menuScript = towerMenu.GetComponent<TowerMenuScript>();
        menuScript.UpdateCurrencyLabel();
        // menuScript.UpdateWaveLabel();
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

    public Button GetLevelPromptButton()
    {
        return levelPrompt.GetComponentInChildren<Button>();
    }

    public void SetLevelPromptContent(string title, string body, Sprite image)
    {
        levelPromptTitle.text = title;
        levelPromptBody.text = body;
        levelPromptImage.sprite = image;
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
        MoveTowerUpgradeMenu(-1800f, 0.4f, () =>
        {
            towerUpgradeMenu.SetActive(false);
        });
    }

    private void MoveTowerUpgradeMenu(float endVal, float duration, TweenCallback onEnd)
    {
        towerUpgradeMenuTransform.DOAnchorPosX(endVal, duration).onComplete += onEnd;
    }

    // For Resource Monitor Prompt
    public void ShowResourceMonitorPrompt(ResourceMonitorScript _script)
    {
        // Set the selected Resouce Monitor Script
        resourceMonitorPromptScript.SetResourceMonitorScript(_script);
        resourceMonitorPrompt.SetActive(true);
        MoveResourceMonitorPrompt(404f, 0.4f, () =>
        {

        });
    }

    public void HideResourceMonitorPrompt()
    {
        MoveResourceMonitorPrompt(950f, 0.4f, () =>
        {
            resourceMonitorPrompt.SetActive(false);
        });
    }

    private void MoveResourceMonitorPrompt(float endVal, float duration, TweenCallback onEnd)
    {
        resourceMonitorPromptTransform.DOAnchorPosY(endVal, duration).onComplete += onEnd;
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

    private bool IsPointerOver(GameObject _object)
    {
        pointerEventData = new PointerEventData(es)
        {
            position = Input.mousePosition
        };

        List<RaycastResult> results = new List<RaycastResult>();
        rc.Raycast(pointerEventData, results);

        foreach (RaycastResult result in results)
        {
            if (result.gameObject == _object || result.gameObject.transform.IsChildOf(_object.transform))
            {
                return true; // Pointer is over upgrade menu
            }
        }

        return false;
    }

    public bool IsPointerOverUpgradeMenu()
    {
        return IsPointerOver(towerUpgradeMenu);
    }

    public bool IsPointerOverPauseButton()
    {
        return IsPointerOver(pauseButton.gameObject);
    }

    public bool IsPointerOverRMPrompt()
    {
        return IsPointerOver(resourceMonitorPrompt);
    }

    public bool IsPointerOverStartButton()
    {
        return IsPointerOver(startButton.gameObject);
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
        pauseMenu.SetActive(true);
        Time.timeScale = 0;
        // If player was building a tower, cancel it
        if (BuildManager.main.isBuilding())
        {
            BuildManager.main.DisableBuilding();
        }
    }

    public void PauseMenuContinueButtonOnClick()
    {
        pauseMenu.SetActive(false);
        Time.timeScale = 1;
    }

    public void PauseMenuExitLevelButtonOnClick()
    {
        SceneManager.LoadSceneAsync("LevelSelectMenu");
        // Unpause the game after exiting
        Time.timeScale = 1;
    }
}
