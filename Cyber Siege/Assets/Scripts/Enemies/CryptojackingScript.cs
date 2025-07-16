using UnityEngine;

public class CryptojackingScript : SpywareEnemyScript
{
    // [Header("Attributes")]
    // [SerializeField] private float cryptojackingInterval = 3f;

    // private float timeUntilCryptoJacked;

    /* When affected by cryptojacking
        - Slowly chip away at the player's money
        - Slow down the firerate of all towers

        To do:
        make cryptojacking spawn special such that
        it doesnt take up a enemy count and spawn chance
    */
    // protected override void Start()
    // {
    //     base.Start();
    //     Hide();
    //     // Vanish(); //Enable later
    // }

    // protected override void Update()
    // {
    //     base.Update();
    //     if (hasReachedServer)
    //     {
    //         timeUntilCryptoJacked += Time.deltaTime;
    //         if (timeUntilCryptoJacked >= cryptojackingInterval)
    //         {
    //             CurrencyManager.main.DecreaseCurrency(30);
    //             timeUntilCryptoJacked = 0;
    //         }
    //     }
    // }

    // private void OnDestroy()
    // {
    //     ServerManager.main.RemoveCryptojacking(1);
    // }

    public override void DestroySelf()
    {
        isDestroyed = true;
        Destroy(gameObject);
    }

    protected override void UpdateMovementTarget()
    {
        // Set target directly to server
        movementTarget = EnemyManager.main.enemyPath[EnemyManager.main.enemyPath.Length - 1];
    }

    protected override void ReachedServer()
    {
        // Should only run once
        // Debug.Log("Reached");
        // ServerManager.main.AddCryptojacking(1);
        ServerManager.main.AttachSpyware(this);
        // DestroySelf();
        hasReachedServer = true;
    }
}
