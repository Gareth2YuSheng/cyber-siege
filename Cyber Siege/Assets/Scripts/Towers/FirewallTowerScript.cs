using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class FirewallTowerScript : BasicTowerScript
{
    [Header("References")]
    [SerializeField] private Sprite damagedSprite;

    [Header("Attributes")]
    [SerializeField] private int health;

    private HashSet<BasicEnemyScript> enemiesInContact = new HashSet<BasicEnemyScript>();

    protected override void Update()
    {
        timeUntilFire += Time.deltaTime;
        if (timeUntilFire >= (1f / bps))
        {
            Action();
            timeUntilFire = 0f;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // check if collision object is in the enemy layer
        if (collision.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            Debug.Log("Stop");
            BasicEnemyScript enemy = collision.gameObject.GetComponent<BasicEnemyScript>();
            // Add enemy to hashset to track
            if (enemy != null)
            {
                // Add the listener before adding to hashset
                enemy.onEnemyDeath.AddListener(HandleEnemyDeath);
                enemiesInContact.Add(enemy);
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            BasicEnemyScript enemy = collision.gameObject.GetComponent<BasicEnemyScript>();
            if (enemy != null)
            {
                enemiesInContact.Remove(enemy);
                // Issue is that what if enemies die while in contant then these references become null
                // We use the event to remove dead enemies
                enemy.onEnemyDeath.RemoveListener(HandleEnemyDeath);
            }
        }
    }

    // This is for constant DoT
    // private void OnCollisionStay2D(Collision2D collision)
    // {
    //     // check if collision object is in the enemy layer
    //     if (collision.gameObject.layer == LayerMask.NameToLayer("Enemy"))
    //     {
    //         Debug.Log("Stop");
    //         BasicEnemyScript enemy = collision.gameObject.GetComponent<BasicEnemyScript>();
    //         // Do DoT
    //     }
    // }

    protected override void Action()
    {
        // Debug.Log("Flame On");
        // using .ToList() we enumerate through a copy of the hashset instead
        // this prevents C# from throwing errors of doing removal while enumerating
        // through the hashset
        foreach (BasicEnemyScript enemy in enemiesInContact.ToList())
        {
            if (enemy != null) // In case the enemy was destroyed
            {
                enemy.TakeDamage(damage);
            }
        }
    }

    // This function will be used to listen for enemy deaths and 
    // remove dead enemies from the Hashset and remove the listener
    private void HandleEnemyDeath(BasicEnemyScript enemy)
    {
        enemiesInContact.Remove(enemy);
        enemy.onEnemyDeath.RemoveListener(HandleEnemyDeath);
    }
}
