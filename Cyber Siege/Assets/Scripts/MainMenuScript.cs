using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuScript : MonoBehaviour
{
    public void PlayButtonOnClick()
    {
        SceneManager.LoadSceneAsync(1);
    }

    public void QuitButtonOnClick()
    {
        Application.Quit();
    }
}
