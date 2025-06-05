using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Events;

public class UIManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject gameOverMenu;
    [SerializeField] private Button gameOverExitButton;
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private Button pauseButton;
    [SerializeField] private GameObject towerMenu;
    [SerializeField] private Button startButton;

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
}
