using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class EncryptionNodeScript : BasicTowerScript
{
    // Shield towers from stun
    // If tower is in range, give status of shielded

    [Header("Attributes")]
    [SerializeField] private float cooldownInterval = 6f;
    private CircleCollider2D protectRangeCollider;
    private HashSet<BasicTowerScript> towersInRange = new HashSet<BasicTowerScript>();

    private BasicTowerScript protectedTower = null;

    private bool isEncryptionActive = true;

    public override void InitialiseTower()
    {
        base.InitialiseTower();
        // Set the circle collider radius
        protectRangeCollider = gameObject.GetComponent<CircleCollider2D>();
        protectRangeCollider.radius = range;
    }

    protected override void Update()
    {
        if (upgrades[0].purchased)
        {
            // Cause a 1 second stun on the enemy! (Ransomware)



            // timeUntilFire += Time.deltaTime;
            // if (timeUntilFire >= stunInterval)
            // {
            //     StunEnemies();
            //     timeUntilFire = 0f;
            // }
        }
        if (upgrades[1].purchased)
        {
            // Decrease cooldown time
            cooldownInterval = 3f;
        }

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


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Towers"))
        {
            var tower = collision.GetComponent<BasicTowerScript>();
            if (tower != null)
            {
                towersInRange.Add(tower);
                tower.EncryptionNodeProtecting = this;
                // tower.ProtectTower(this);
                // Debug.Log("Protected tower");

            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Towers"))
        {
            BasicTowerScript tower = collision.GetComponent<BasicTowerScript>();
            if (tower != null)
            {
                // tower.ResetMovementSpeed();
                // tower.onEnemyDeath.RemoveListener(HandleBuffedEnemyDeath);

                // If Upgrade 2 has been purchased
                if (upgrades[1].purchased)
                {
                    // tower.ResetTakenDamageMultiplier();
                }
                towersInRange.Remove(tower);
                tower.EncryptionNodeProtecting = null;
            }
        }
    }

    // private void OnTriggerStay2D(Collider2D collision)
    // {
    //     // Debug.Log("Trigger Stay");
    //     if (collision.gameObject.layer == LayerMask.NameToLayer("Towers"))
    //     {
    //         BasicTowerScript tower = collision.GetComponent<BasicTowerScript>();
    //         Debug.Log(tower);
    //         if (tower != null && !towersInRange.ContainsKey(tower))
    //         {
    //             // enemy.UpdateMovementSpeed(1f - slowingFactor);
    //             if (!hasProtected)
    //             {
    //                 tower.ProtectTower(this);
    //                 Debug.Log("Protecting tower");
    //             }
    //             else
    //             {

    //                 tower.UnProtectTower();

    //             }


    //             // If Upgrade 2 has been purchased
    //             // if (upgrades[1].purchased)
    //             // {
    //             //     enemy.SetTakenDamageMultiplier(bonusDamageMultiplier);
    //             // }

    //             // slowedEnemies.Add(enemy, 0f);
    //         }
    //     }
    // }



    // // For Upgrade 2
    // private void OnCollisionStay2D(Collision2D collision)
    // {

    // }


    protected override void Action()
    {

    }

    /* Upgrades
        Upgrade 1 - Secondary Verification
        Enemies that remain inside the area for more than X seconds are stunned 
        or immobilized briefly (e.g. 1 second stun every 5 seconds spent inside).

        Upgrade 2 - Security Audit
        Enemies that pass through the gate temporarily take extra damage from 
        all sources (small % bonus damage for a few seconds).
    */

    // Security Audit
    public override void Upgrade2()
    {
        base.Upgrade2();
        // // Apply the damage multiplier to enemies already in the range
        // foreach (var enemyWithTime in slowedEnemies.ToList())
        // {
        //     BasicEnemyScript enemy = enemyWithTime.Key;
        //     if (enemy != null) // In case the enemy was destroyed
        //     {
        //         enemy.SetTakenDamageMultiplier(bonusDamageMultiplier);
        //     }
        // }
    }

    public void disableEncryptionNode(BasicTowerScript tower, BasicEnemyScript enemy)
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
        StartCoroutine(disableTemp());
    }

    protected IEnumerator disableTemp()
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
