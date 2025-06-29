using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuScript : MonoBehaviour
{
    public void PlayButtonOnClick()
    {
        SceneManager.LoadSceneAsync("LevelSelectMenu");
    }

    public void QuitButtonOnClick()
    {
        Application.Quit();
    }
}
