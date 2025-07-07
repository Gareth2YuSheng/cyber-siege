using UnityEngine;

public class TestLevelManager : MonoBehaviour
{
    private void Start()
    {
        LevelManager.main.InitLevel(200, 100);
        UIManager.main.UpdateHUDLabels();
    }
}
