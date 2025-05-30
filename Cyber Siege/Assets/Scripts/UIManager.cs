using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    [Header("References")]
    public GameObject gameOverMenu;
    public Button gameOverExitButton;
    public GameObject pauseMenu;
    public Button pauseButton;
    public GameObject towerMenu;
    public Button startButton;

    private void Update()
    {
        if (!LevelManager.main.isServerAlive && !gameOverMenu.activeSelf)
        {
            gameOverMenu.SetActive(true);
            SetAllSelectableChildrenFromTowerMenu(false);
            pauseButton.interactable = false;
        }

        //Show/Hide Start Wave Button
        //If the wave started and the butten is currently active
        if ((EnemyManager.main.waveOngoing && startButton.gameObject.activeSelf) || gameOverMenu.activeSelf)
        {
            startButton.gameObject.SetActive(false);
        }
        //Else if the wave has ended and the button is still hidden
        else if (!EnemyManager.main.waveOngoing && !startButton.gameObject.activeSelf)
        {
            startButton.gameObject.SetActive(true);
        }
    }

    private void SetAllSelectableChildrenFromTowerMenu(bool state)
    {
        Selectable[] uiElements = towerMenu.GetComponentsInChildren<Selectable>();
        foreach (Selectable element in uiElements)
        {
            element.interactable = state;
        }
    }

    public void TowerMenuStartButtonOnClick()
    {
        EnemyManager.main.StartWave();
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
