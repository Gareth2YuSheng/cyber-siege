using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class EncryptionNodeScript : BasicTowerScript
{
    // Shield towers from stun
    // If tower is in range, give status of shielded

    [Header("References")]
    [SerializeField] protected LayerMask towerMask;

    [Header("Attributes")]
    [SerializeField] private float cooldownInterval = 6f;
    private HashSet<BasicTowerScript> towersInRange = new HashSet<BasicTowerScript>();

    private BasicTowerScript protectedTower = null;

    private bool isEncryptionActive = true;

    protected override void Update()
    {
        Action();

        foreach (var tower in towersInRange)
        {
            // Debug.Log($"[EncryptNode {name}] Trying to protect {tower.name} with node ID {GetInstanceID()}");

            if (isEncryptionActive || tower == protectedTower)
            {
                tower.ProtectTower(this);
            }
            else
            {
                Debug.Log($"[UNPROTECT {GetInstanceID()}");

                tower.UnProtectTower();
            }
        }

    }


    // private void OnTriggerEnter2D(Collider2D collision)
    // {


    //     if (collision.gameObject.layer == LayerMask.NameToLayer("Towers"))
    //     {
    //         BasicTowerScript tower = collision.GetComponent<BasicTowerScript>();
    //         if (tower != null)
    //         {
    //             towersInRange.Add(tower);
    //             tower.SetEncryptionNode(this);
    //             // tower.ProtectTower(this);
    //             // Debug.Log("Protected tower");

    //         }
    //     }
    // }

    // private void OnTriggerExit2D(Collider2D collision)
    // {
    //     if (collision.gameObject.layer == LayerMask.NameToLayer("Towers"))
    //     {
    //         BasicTowerScript tower = collision.GetComponent<BasicTowerScript>();
    //         if (tower != null)
    //         {
    //             // tower.ResetMovementSpeed();
    //             // tower.onEnemyDeath.RemoveListener(HandleBuffedEnemyDeath);

    //             // If Upgrade 2 has been purchased
    //             if (upgrades[1].purchased)
    //             {
    //                 // tower.ResetTakenDamageMultiplier();
    //             }
    //             towersInRange.Remove(tower);
    //             tower.ResetEncryptionNode();
    //         }
    //     }
    // }

    protected override void Action()
    {
        Debug.Log("Scanning");
        RaycastHit2D[] hits = Physics2D.CircleCastAll(transform.position, range, (Vector2)transform.position, 0f, towerMask);

        //If there is a target in range
        if (hits.Length > 0)
        {
            foreach (RaycastHit2D hit in hits)
            {
                // Check if target is hidden
                BasicTowerScript tower = hit.transform.GetComponentInParent<BasicTowerScript>();
                if (tower != null && tower != this && towersInRange.Contains(tower) == false)
                {
                    // Prevent performing too many times
                    Debug.Log("ADDED TOWER");
                    towersInRange.Add(tower);
                    tower.SetEncryptionNode(this);

                    // tower.ProtectTower(this);
                    // Debug.Log("Protected tower");

                }
            }
        }
    }


    /* Upgrades
        Upgrade 1 - Secondary Verification
        Stun the Ransomware that caused cooldown via tower for 3 seconds.

        Upgrade 2 - Encryption Fortification
        Decreases the cooldown timing from 6 seconds to 3 seconds.
    */

    public override void Upgrade2()
    {
        base.Upgrade2();
        cooldownInterval = 3f;
    }

    public void DisableEncryptionNode(BasicTowerScript tower, BasicEnemyScript enemy)
    {
        if (!isEncryptionActive)
            return;
        isEncryptionActive = false;
        protectedTower = tower;

        // Check upgrade
        if (upgrades[0].purchased)
        {
            // Stun enemy
            // enemy
            if (enemy != null) // In case the enemy was destroyed
            {
                // May need to handle null exception for when it tries to switch sprite back.
                StartCoroutine(enemy.Stun(3f));
            }
        }
        Debug.Log($"Encryption node disabled. Tower {tower.name} remains protected. 6 second start");
        StartCoroutine(DisableTemp());
    }

    protected IEnumerator DisableTemp()
    {
        // Wait for 6 seconds
        yield return new WaitForSeconds(cooldownInterval);

        // Code to execute after 6 seconds
        Debug.Log("6 seconds have passed! Enabling Protection again");
        // Enable all towers
        isEncryptionActive = true;
        protectedTower = null;
    }
}
