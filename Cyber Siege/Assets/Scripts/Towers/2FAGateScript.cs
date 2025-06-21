using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class TwoFA_GateScript : BasicPathTowerScript
{
    [Header("Attributes")]
    [SerializeField] private float slowingFactor;
    private CircleCollider2D slowingRangeCollider;

    private HashSet<BasicEnemyScript> slowedEnemies = new HashSet<BasicEnemyScript>();

    protected override void Start()
    {
        base.Start();
        // Set the circle collider radius
        slowingRangeCollider = gameObject.GetComponent<CircleCollider2D>();
        slowingRangeCollider.radius = range;
    }

    protected override void Update()
    {
        // Override here to stop logic in parent as it is
        // unnecessary for this tower
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            BasicEnemyScript enemy = collision.GetComponent<BasicEnemyScript>();
            if (enemy != null && !slowedEnemies.Contains(enemy))
            {
                enemy.UpdateMovementSpeed(enemy.GetBaseSpeed() * (1f - slowingFactor));
                enemy.onEnemyDeath.AddListener(HandleBuffedEnemyDeath);
                slowedEnemies.Add(enemy);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            BasicEnemyScript enemy = collision.GetComponent<BasicEnemyScript>();
            if (enemy != null && slowedEnemies.Contains(enemy))
            {
                enemy.ResetMovementSpeed();
                enemy.onEnemyDeath.RemoveListener(HandleBuffedEnemyDeath);
                slowedEnemies.Remove(enemy);
            }
        }
    }

    private void HandleBuffedEnemyDeath(BasicEnemyScript enemy)
    {
        slowedEnemies.Remove(enemy);
        enemy.onEnemyDeath.RemoveListener(HandleBuffedEnemyDeath);
    }

    protected override void Action()
    {

    }
}
