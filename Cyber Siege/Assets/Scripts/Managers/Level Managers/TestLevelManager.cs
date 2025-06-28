using UnityEngine;

public class TestLevelManager : MonoBehaviour
{
    private void Start()
    {
        LevelManager.main.IncreaseCurrency(200);
        LevelManager.main.HealServer(100);
    }
}
