using UnityEngine;

public class SpywareEnemyScript : BasicEnemyScript
{
    protected override void ResetEnemy()
    {
        base.ResetEnemy();
        Hide();
        if (EnemyManager.main != null && !EnemyManager.main.testingMode) Vanish();
    }
}
