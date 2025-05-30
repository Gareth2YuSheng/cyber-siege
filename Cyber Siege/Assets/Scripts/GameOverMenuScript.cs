using UnityEngine;
using UnityEngine.UI;

public class GameOverMenuScript : MonoBehaviour
{
    [Header("References")]
    public Button exit;

    public void exitButtonOnClick()
    {
        Application.Quit();
    }
}
