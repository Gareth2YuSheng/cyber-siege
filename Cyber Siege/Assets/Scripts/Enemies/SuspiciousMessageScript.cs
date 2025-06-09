using UnityEngine;
using UnityEngine.UI;

public class SuspiciousMessageScript : MonoBehaviour
{
    // [Header("References")]
    // [SerializeField] private Image alert;

    [Header("Attributes")]
    [SerializeField] private float alertSpawnRate;

    private float timeSinceLastAlert = 0f;

    private void Start()
    {

    }

    private void Update()
    {
        timeSinceLastAlert += Time.deltaTime;

        if (timeSinceLastAlert >= alertSpawnRate)
        {
            // Show Alert on screen
            UIManager.main.ShowScamMessage();
            timeSinceLastAlert = 0f;
        }
    }
}
