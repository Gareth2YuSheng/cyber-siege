using UnityEngine;

public class CryptojackingScript : SpywareEnemyScript
{
    /* When affected by cryptojacking
        - Slowly chip away at the player's money
        - Slow down the firerate of all towers

        To do:
        make cryptojacking spawn special such that
        it doesnt take up a enemy count and spawn chance
    */

    public override void DestroySelf()
    {
        isDestroyed = true;
        // Destroy(gameObject);
        ReturnPooledObject();
    }

    protected override void UpdateMovementTarget()
    {
        // Set target directly to server
        movementTarget = EnemyManager.main.enemyPath[EnemyManager.main.enemyPath.Length - 1];
    }

    protected override void ReachedServer()
    {
        ServerManager.main.AttachSpyware(this);
        hasReachedServer = true;
    }
}
