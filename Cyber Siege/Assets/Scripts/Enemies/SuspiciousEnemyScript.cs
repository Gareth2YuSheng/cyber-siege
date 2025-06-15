using UnityEngine;

public class SuspiciousEnemyScript : BasicEnemyScript
{
    protected override void Start()
    {
        base.Start();
        // Hide phishing enemies first
        Hide();
    }

    protected override void Update()
    {
        base.Update();
    }
}
