using UnityEngine;
using System.Collections.Generic;

public class SuspiciousCallScript : SuspiciousEnemyScript
{
    [SerializeField] private float buffRange;
    [SerializeField] private float buffAmount;

    private string phishingEnemyTag = "Phishing Enemy";
    private CircleCollider2D buffRangeCollider;

    private HashSet<BasicEnemyScript> buffedEnemies = new HashSet<BasicEnemyScript>();

    protected override void Start()
    {
        base.Start();
        // Set the circle collider radius
        buffRangeCollider = gameObject.GetComponent<CircleCollider2D>();
        buffRangeCollider.radius = buffRange * 10;
    }

    private void OnDestroy()
    {
        // Reset all buffs if this buffer dies
        foreach (BasicEnemyScript enemy in buffedEnemies)
        {
            if (enemy != null)
            {
                enemy.ResetMovementSpeed();
                enemy.onEnemyDeath.RemoveListener(HandleBuffedEnemyDeath);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(phishingEnemyTag))
        {
            BasicEnemyScript enemy = collision.GetComponent<BasicEnemyScript>();
            if (enemy != null && !buffedEnemies.Contains(enemy))
            {
                enemy.UpdateMovementSpeed(enemy.GetBaseSpeed() * buffAmount);
                enemy.onEnemyDeath.AddListener(HandleBuffedEnemyDeath);
                buffedEnemies.Add(enemy);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag(phishingEnemyTag))
        {
            BasicEnemyScript enemy = collision.GetComponent<BasicEnemyScript>();
            if (enemy != null && buffedEnemies.Contains(enemy))
            {
                enemy.ResetMovementSpeed();
                enemy.onEnemyDeath.RemoveListener(HandleBuffedEnemyDeath);
                buffedEnemies.Remove(enemy);
            }
        }
    }

    // This is added in the case that enemies die while being buffed 
    // to clean up the references in buffedEnemies
    private void HandleBuffedEnemyDeath(BasicEnemyScript enemy)
    {
        buffedEnemies.Remove(enemy);
        enemy.onEnemyDeath.RemoveListener(HandleBuffedEnemyDeath);
    }
}
