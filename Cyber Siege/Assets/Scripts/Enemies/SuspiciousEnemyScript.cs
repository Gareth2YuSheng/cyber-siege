using UnityEngine;

public class SuspiciousEnemyScript : BasicEnemyScript
{
    protected override void Start()
    {
        base.Start();
        // Hide phishing enemies first
        // Set opacity to 20%
        changeOpacity(0.2f);
        Hide();
    }

    protected override void Update()
    {
        base.Update();
    }
}
