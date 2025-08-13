using UnityEngine;

public class SuspiciousEnemyScript : BasicEnemyScript
{
    protected override void ResetEnemy()
    {
        base.ResetEnemy();
        // Hide phishing enemies first
        // Set opacity to 20%
        changeOpacity(0.2f);
        Hide();
    }
}
