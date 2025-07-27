using UnityEngine;
using System.Collections.Generic;

public class ServerManager : MonoBehaviour
{
    public static ServerManager main;

    [Header("Attributes")]
    [SerializeField] private float cryptojackingInterval = 3f;
    [SerializeField] private int cryptojackingAmt = 30;

    private int cryptojackingCounter = 0;
    private float timeUntilCryptoJacked;

    // FIFO so use Queue instead of list or hashmap
    private Queue<SpywareEnemyScript> attachedSpywareEnemies = new Queue<SpywareEnemyScript>();

    private void Awake()
    {
        if (main != null && main != this)
        {
            Destroy(this);
        }
        else
        {
            main = this;
        }
    }

    /* When affected by cryptojacking
        - Slowly chip away at the player's money
        - Slow down the firerate of all towers
    */
    private void Update()
    {
        // Assume all spyware will steal money if not use cryptojacking counter instead
        // Only run when wave is ongoing
        if (attachedSpywareEnemies.Count > 0 && EnemyManager.main.waveOngoing)
        {
            timeUntilCryptoJacked += Time.deltaTime;
            if (timeUntilCryptoJacked >= cryptojackingInterval)
            {
                CurrencyManager.main.DecreaseCurrency(cryptojackingAmt);
                timeUntilCryptoJacked = 0;
            }
        }
    }

    public void AttachSpyware(SpywareEnemyScript spyware)
    {
        // Add if not already added
        if (attachedSpywareEnemies.Contains(spyware)) return;

        attachedSpywareEnemies.Enqueue(spyware);
        string enemyName = spyware.GetEnemyName();
        Debug.Log("Attaching Spyware: " + spyware.GetEnemyName());
        if (enemyName == "Cryptojacking")
        {
            cryptojackingCounter++;
        }
    }

    public void PurgeFirstSpyware()
    {
        // Dont do anything if no spyware attached
        if (attachedSpywareEnemies.Count <= 0) return;

        SpywareEnemyScript spyware = attachedSpywareEnemies.Dequeue();
        // Destroy Spyware and adjust count
        string enemyName = spyware.GetEnemyName();
        if (enemyName == "Cryptojacking")
        {
            cryptojackingCounter--;
        }

        spyware.DestroySelf();
        Debug.Log("Spyware Purged Successfully!");
    }

    public bool HasSpywareAttached()
    {
        return attachedSpywareEnemies.Count > 0;
    }

    public int GetCryptojackingCount()
    {
        return cryptojackingCounter;
    }

    // public void AddCryptojacking(int amt)
    // {
    //     cryptojackingCounter += amt;
    // }

    // public void RemoveCryptojacking(int amt)
    // {
    //     cryptojackingCounter -= amt;
    // }
}
