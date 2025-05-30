using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [Header("References")]
    public GameObject gameOverMenu;
    public Button startButton;

    private void Update()
    {
        if (!LevelManager.main.isServerAlive && !gameOverMenu.activeSelf)
        {
            gameOverMenu.SetActive(true);
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

    public void TowerMenuStartButtonOnClick()
    {
        Debug.Log("Start Wave");
        EnemyManager.main.StartWave();
    }
}
