using UnityEngine;
using System.Collections.Generic;

public class SuspiciousCallScript : MonoBehaviour
{
    [SerializeField] private float buffRange;
    [SerializeField] private float buffAmount;

    private string phishingEnemyTag = "Phishing Enemy";
    private CircleCollider2D buffRangeCollider;

    private List<BasicEnemyScript> buffedEnemies = new List<BasicEnemyScript>();

    private void Start()
    {
        // Set the circle collider radius
        buffRangeCollider = gameObject.GetComponent<CircleCollider2D>();
        buffRangeCollider.radius = buffRange * 10;
    }

    private void Update()
    {

    }

    private void OnDestroy()
    {
        // Reset all buffs if this buffer dies
        foreach (BasicEnemyScript enemy in buffedEnemies)
        {
            if (enemy != null)
            {
                enemy.ResetMovementSpeed();
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(phishingEnemyTag))
        {
            Debug.Log($"Buffing {collision.name}");
            BasicEnemyScript enemy = collision.GetComponent<BasicEnemyScript>();
            if (enemy != null && !buffedEnemies.Contains(enemy))
            {
                enemy.UpdateMovementSpeed(enemy.GetBaseSpeed() * buffAmount);
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
                Debug.Log($"Stopped Buffing {collision.name}");
                enemy.ResetMovementSpeed();
                buffedEnemies.Remove(enemy);
            }
        }
    }

}
