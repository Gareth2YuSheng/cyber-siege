using UnityEngine;

public class SuspiciousMessageScript : SuspiciousEnemyScript
{
    [Header("Attributes")]
    [SerializeField] private float alertSpawnRate;

    private float timeSinceLastAlert = 0f;

    protected override void Update()
    {
        base.Update();

        timeSinceLastAlert += Time.deltaTime;

        if (timeSinceLastAlert >= alertSpawnRate)
        {
            // Show Alert on screen
            UIManager.main.ShowScamMessage();
            timeSinceLastAlert = 0f;
        }
    }
}
