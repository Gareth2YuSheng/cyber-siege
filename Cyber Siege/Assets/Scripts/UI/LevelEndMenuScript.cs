using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelEndMenuScript : MonoBehaviour
{
    public void RetryButtonOnClick()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void BackToLevelSelectButtonOnClick()
    {
        SceneManager.LoadSceneAsync("LevelSelectMenu");
    }
}
