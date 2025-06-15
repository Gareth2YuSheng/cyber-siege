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

    // For Scam Message
    [SerializeField] private GameObject susMessageAlertPrefab;
    // [SerializeField] private GameObject susMessageAlert;
    // [SerializeField] private TextMeshProUGUI susMessageTitle;
    // [SerializeField] private TextMeshProUGUI susMessageBody;
    // [SerializeField] private TextMeshProUGUI susMessageActionButtonText;
    // [SerializeField] private Button susMessageCloseButton;

    // Coroutine to simulate a timeout after a specified duration
    // private bool isTimedOut = false;

    private void Awake()
    {
        main = this;
    }

    private void Start()
    {
        // Add Event Listeners
        LevelManager.main.onServerDeath.AddListener(GameOver);
        EnemyManager.main.onWaveEnd.AddListener(WaveEnded);
    }

    private void Update()
    {

    }

    private void SetAllSelectableChildrenFromTowerMenu(bool state)
    {
        Selectable[] uiElements = towerMenu.GetComponentsInChildren<Selectable>();
        foreach (Selectable element in uiElements)
        {
            element.interactable = state;
        }
    }

    private void GameOver()
    {
        gameOverMenu.SetActive(true);
        SetAllSelectableChildrenFromTowerMenu(false);
        pauseButton.interactable = false;
    }

    private void WaveEnded()
    {
        startButton.gameObject.SetActive(true);
    }

    IEnumerator SetPromptTimeout(float timeoutDuration)
    {
        yield return new WaitForSeconds(timeoutDuration);

        // Code after timeout
        // isTimedOut = true;
        FadeErrorPrompt(0f, 1f, () =>
        {
            errorPrompt.SetActive(false);
        });
        // You can add additional logic here if you need to perform actions when the timeout occurs
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
        //Change it later
        SceneManager.LoadSceneAsync(0);
    }

    public void GameOverMenuExitOnClick()
    {
        Application.Quit();
    }

    public void SusMessageFakeActionButtonOnClick()
    {

    }

    //     public void SusMessageCloseButtonOnClick()
    //     {
    //         susMessageAlert.SetActive(false);
    //     }
}
