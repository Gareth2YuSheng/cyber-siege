using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class TwoFA_GateScript : BasicPathTowerScript
{
    [Header("Attributes")]
    [SerializeField] private float slowingFactor;
    [SerializeField] private float stunInterval = 3f;
    [SerializeField] private float stunDuration = 1f;
    [SerializeField] private float bonusDamageMultiplier = 1.25f;
    private CircleCollider2D slowingRangeCollider;

    // private HashSet<BasicEnemyScript> slowedEnemies = new HashSet<BasicEnemyScript>();
    private Dictionary<BasicEnemyScript, float> slowedEnemies = new Dictionary<BasicEnemyScript, float>();


    protected override void Start()
    {
        base.Start();
        // Set the circle collider radius
        slowingRangeCollider = gameObject.GetComponent<CircleCollider2D>();
        slowingRangeCollider.radius = range;
    }

    protected override void Update()
    {
        if (upgrades[0].purchased)
        {
            timeUntilFire += Time.deltaTime;
            if (timeUntilFire >= stunInterval)
            {
                StunEnemies();
                timeUntilFire = 0f;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            BasicEnemyScript enemy = collision.GetComponent<BasicEnemyScript>();
            if (enemy != null && !slowedEnemies.ContainsKey(enemy))
            {
                enemy.UpdateMovementSpeed(enemy.GetBaseSpeed() * (1f - slowingFactor));
                enemy.onEnemyDeath.AddListener(HandleBuffedEnemyDeath);

                // If Upgrade 2 has been purchased
                if (upgrades[1].purchased)
                {
                    enemy.SetTakenDamageMultiplier(bonusDamageMultiplier);
                }

                slowedEnemies.Add(enemy, 0f);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            BasicEnemyScript enemy = collision.GetComponent<BasicEnemyScript>();
            if (enemy != null && slowedEnemies.ContainsKey(enemy))
            {
                enemy.ResetMovementSpeed();
                enemy.onEnemyDeath.RemoveListener(HandleBuffedEnemyDeath);

                // If Upgrade 2 has been purchased
                if (upgrades[1].purchased)
                {
                    enemy.ResetTakenDamageMultiplier();
                }

                slowedEnemies.Remove(enemy);
            }
        }
    }

    // // For Upgrade 2
    // private void OnCollisionStay2D(Collision2D collision)
    // {

    // }

    private void HandleBuffedEnemyDeath(BasicEnemyScript enemy)
    {
        slowedEnemies.Remove(enemy);
        enemy.onEnemyDeath.RemoveListener(HandleBuffedEnemyDeath);
    }

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
        // Apply the damage multiplier to enemies already in the range
        foreach (var enemyWithTime in slowedEnemies.ToList())
        {
            BasicEnemyScript enemy = enemyWithTime.Key;
            if (enemy != null) // In case the enemy was destroyed
            {
                enemy.SetTakenDamageMultiplier(bonusDamageMultiplier);
            }
        }
    }

    private void StunEnemies()
    {
        Debug.Log("Stunning Enemies");
        foreach (var enemyWithTime in slowedEnemies.ToList())
        {
            BasicEnemyScript enemy = enemyWithTime.Key;
            if (enemy != null) // In case the enemy was destroyed
            {
                StartCoroutine(enemy.Stun(stunDuration));
            }
        }
    }
}
