using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Events;
using System.Collections;
using TMPro;

public class UIManager : MonoBehaviour
{
    public static UIManager main;

    [Header("References")]
    [SerializeField] private GameObject gameOverMenu;
    [SerializeField] private Button gameOverExitButton;
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private Button pauseButton;
    [SerializeField] private GameObject towerMenu;
    [SerializeField] private Button startButton;
    [SerializeField] private GameObject errorPrompt;
    [SerializeField] private TextMeshProUGUI errorPromptLabel;

    // Coroutine to simulate a timeout after a specified duration
    private bool isTimedOut = false;

    IEnumerator SetPromptTimeout(float timeoutDuration)
    {
        yield return new WaitForSeconds(timeoutDuration);

        // Code after timeout
        isTimedOut = true;
        Debug.Log("Hide Prompt!");
        errorPrompt.gameObject.SetActive(false);
        // You can add additional logic here if you need to perform actions when the timeout occurs
    }
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
        //Show/Hide Start Wave Button
        //If the wave started and the butten is currently active
        // if ((EnemyManager.main.waveOngoing && startButton.gameObject.activeSelf) || gameOverMenu.activeSelf)
        // {
        //     // startButton.gameObject.SetActive(false);
        // }
        // //Else if the wave has ended and the button is still hidden
        // else if (!EnemyManager.main.waveOngoing && !startButton.gameObject.activeSelf)
        // {
        //     startButton.gameObject.SetActive(true);
        // }
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

    // Error prompt implementation
    // Shows for specific number of seconds and prompt given.
    public void ShowErrorPrompt(string prompt)
    {
        errorPromptLabel.text = prompt;
        errorPrompt.gameObject.SetActive(true);
        StartCoroutine(SetPromptTimeout(3f));  // Timeout set to 3 seconds
    }
}
