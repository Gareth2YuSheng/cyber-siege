using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class FirewallTowerScript : BasicTowerScript
{
    [Header("References")]
    [SerializeField] private Sprite damagedSprite;
    [SerializeField] private SpriteRenderer mySR;

    [Header("Attributes")]
    [SerializeField] private int maxHealth;
    [SerializeField] private float damageInterval; //takes damage every X seconds
    private float timeUntilDamaged;
    private int currHealth;

    private HashSet<BasicEnemyScript> enemiesInContact = new HashSet<BasicEnemyScript>();

    protected override void Start()
    {
        base.Start();
        currHealth = maxHealth;
    }

    protected override void Update()
    {
        // Reason for not combining both into 1 action is for more
        // customisable behavior
        timeUntilFire += Time.deltaTime;
        timeUntilDamaged += Time.deltaTime;
        // Deal Damage
        if (timeUntilFire >= (1f / bps))
        {
            Action();
            timeUntilFire = 0f;
        }
        // Absorb Damage
        if (timeUntilDamaged >= damageInterval)
        {
            TakeDamageFromEnemiesInContact();
            timeUntilDamaged = 0f;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // check if collision object is in the enemy layer
        if (collision.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            // Debug.Log("Stop");
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
        // Dont run if no enemies
        if (enemiesInContact.Count < 1) return;

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

    private void TakeDamageFromEnemiesInContact()
    {
        // Dont run if no enemies
        if (enemiesInContact.Count < 1) return;

        // Debug.Log("Ouch");
        foreach (BasicEnemyScript enemy in enemiesInContact.ToList())
        {
            if (enemy != null) // In case the enemy was destroyed
            {
                TakeDamage(enemy.GetDamageDealtToServer());
            }
        }
    }

    private void TakeDamage(int _damage)
    {
        Debug.Log($"Took {_damage} damage");
        currHealth -= _damage;
        // If health drops below 0, firewall is destroyed
        if (currHealth <= 0)
        {
            Destroy(gameObject);
        }
        // If health drops below 50%, then show damaged sprite
        else if (currHealth <= (maxHealth / 2))
        {
            Debug.Log("Changing");
            mySR.sprite = damagedSprite;
        }
    }
}
