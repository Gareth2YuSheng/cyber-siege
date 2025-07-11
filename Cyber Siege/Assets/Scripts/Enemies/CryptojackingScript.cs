using UnityEngine;

public class CryptojackingScript : BasicEnemyScript
{
    [Header("Attributes")]
    [SerializeField] private float cryptojackingInterval = 3f;

    private bool hasReachedServer;
    private float timeUntilCryptoJacked;

    /* When affected by cryptojacking
        - Slowly chip away at the player's money
        - Slow down the firerate of all towers

        To do:
        either use server manager to manage cryptojacking
        OR
        make cryptojacking spawn special such that
        it doesnt take up a enemy count and spawn chance
    */
    protected override void Start()
    {
        base.Start();
        Hide();
    }

    protected override void Update()
    {
        base.Update();
        if (hasReachedServer)
        {
            timeUntilCryptoJacked += Time.deltaTime;
            if (timeUntilCryptoJacked >= cryptojackingInterval)
            {
                CurrencyManager.main.DecreaseCurrency(30);
                timeUntilCryptoJacked = 0;
            }
        }
    }

    private void OnDestroy()
    {
        Debug.Log("Help Me im dying");
        // Reset Tower Performance
    }

    protected override void UpdateMovementTarget()
    {
        movementTarget = EnemyManager.main.enemyPath[EnemyManager.main.enemyPath.Length - 1];
    }

    protected override void ReachedServer()
    {
        // ServerManager.main.AddCryptojacking(1);
        // DestroySelf();

        hasReachedServer = true;
        // Slow Tower Performance

    }
}
