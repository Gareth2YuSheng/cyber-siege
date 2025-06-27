using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class FirewallTowerScript : BasicPathTowerScript
{
    [Header("References")]
    [SerializeField] private Sprite damagedSprite;
    [SerializeField] private SpriteRenderer mySR;

    [Header("Attributes")]
    [SerializeField] private int maxHealth = 20;
    [SerializeField] private float damageInterval = 2f; //takes damage every X seconds
    [SerializeField] private int shieldHealth = 20;
    [SerializeField] private float shieldRegenAfterDuration = 3f; //Starts healing after X seconds
    [SerializeField] private float shieldRegenInterval = 2f; //Regens every X second interval
    [SerializeField] private int regenAmount = 1; //Regens X amount of hp
    [SerializeField] private int burnAmplificationRate = 2;

    private Sprite healthySprite;
    private float timeUntilDamaged;
    private float timeUntilHeal;
    private bool regenEnabled = false;
    private int currHealth;

    // private HashSet<BasicEnemyScript> enemiesInContact = new HashSet<BasicEnemyScript>();
    private Dictionary<BasicEnemyScript, float> enemiesInContact = new Dictionary<BasicEnemyScript, float>();

    public override void InitialiseTower()
    {
        base.InitialiseTower();
        currHealth = maxHealth;
        healthySprite = mySR.sprite;
    }

    protected override void Update()
    {
        // Dont do things if the wave is not ongoing
        if (!EnemyManager.main.waveOngoing) return; //Should be ok since this is not UI related

        // Ransomware handling - Derived from BasicTowerScript

        FindRansomwareScript();
        // Reason for not combining both into 1 action is for more
        // customisable behavior
        if (!disabled)
        {
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

            // To keep track how long each enemy has been in contact with this
            foreach (var enemyAndTime in enemiesInContact.ToList())
            {
                enemiesInContact[enemyAndTime.Key] += Time.deltaTime;
            }

            // Regen HP
            // If upgrade has not been purchased, dont heal
            if (!upgrades[1].purchased) return;
            timeUntilHeal += Time.deltaTime;
            if (regenEnabled && timeUntilHeal >= shieldRegenInterval)
            {
                // Heal only if not max hp
                if (currHealth < maxHealth) RestoreHealth(regenAmount);
                timeUntilHeal = 0f;
                return;
            }
            // Wait for firewall to not be attacked for X seconds before enabling regen
            if (!regenEnabled && timeUntilHeal >= shieldRegenAfterDuration)
            {
                regenEnabled = true;
                timeUntilHeal = 0f;
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Debug.Log("Oncollisionenter");
        // check if collision object is in the enemy layer
        if (!disabled && collision.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            Debug.Log("Stop");
            BasicEnemyScript enemy = collision.gameObject.GetComponent<BasicEnemyScript>();
            // Add enemy to hashset to track
            if (enemy != null)
            {
                // Add the listener before adding to hashset
                enemy.onEnemyDeath.AddListener(HandleEnemyDeath);
                // enemiesInContact.Add(enemy);
                if (!enemiesInContact.ContainsKey(enemy))
                {
                    // Play shield
                    SoundManager.main.PlaySoundFXClip(effectAudio, 0.5f);

                    enemiesInContact.Add(enemy, 0f);
                }
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        Debug.Log("OnCollisionExit");
        if (!disabled && collision.gameObject.layer == LayerMask.NameToLayer("Enemy"))
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
        else if (disabled && collision.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            BasicEnemyScript enemy = collision.gameObject.GetComponent<BasicEnemyScript>();
            enemy.isBlocked = false;
        }

    }

    // This is for constant DoT against the enemies
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
        foreach (var enemyWithTime in enemiesInContact.ToList())
        {
            BasicEnemyScript enemy = enemyWithTime.Key;
            float timeInContact = enemyWithTime.Value;
            if (enemy != null) // In case the enemy was destroyed
            {
                // If Upgrade 1 was purchased, do the amplified dmg
                if (upgrades[0].purchased)
                {
                    int amplifiedDamage = towerDamage + Mathf.FloorToInt(timeInContact * burnAmplificationRate);
                    enemy.TakeDamage(amplifiedDamage);
                }
                // Else do standard tower damage
                else
                {
                    enemy.TakeDamage(towerDamage);
                }
            }
        }
    }

    /* Upgrades
        Upgrade 1 - Adaptive Burn
        The longer an enemy remains in contact with the Firewall, 
        the more damage it takes (stacking DoT).

        Upgrade 2 - Resilience Module
        The Firewall gains an additional shield or armor layer that 
        regenerates slowly over time if not attacked for X seconds.

        For example: +500 shield HP, regenerates 50 HP/sec after 
        3 seconds without taking damage.
    */

    // Resilience Module
    public override void Upgrade2()
    {
        base.Upgrade2();
        maxHealth += shieldHealth;
        currHealth += shieldHealth;
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

        foreach (var enemyWithTime in enemiesInContact.ToList())
        {
            BasicEnemyScript enemy = enemyWithTime.Key;
            if (enemy != null) // In case the enemy was destroyed
            {
                TakeDamage(enemy.GetDamageDealtToServer());
            }
        }
    }

    private void TakeDamage(int _damage)
    {
        // Debug.Log($"Firewall Took {_damage} damage");
        currHealth -= _damage;
        // Stop regen if was
        if (regenEnabled)
        {
            regenEnabled = false;
            timeUntilHeal = 0f; //Reset time until heal so it doesnt accidentally start healing immediately
        }
        // If health drops below 0, firewall is destroyed
        if (currHealth <= 0)
        {
            Destroy(gameObject);
        }
        // If health drops below 50%, then show damaged sprite
        else if (currHealth <= (maxHealth / 2))
        {
            mySR.sprite = damagedSprite;
        }
    }

    private void RestoreHealth(int _health)
    {
        Debug.Log($"Firewall healed {_health} hp");
        // If curr health is already full, do nothing
        if (currHealth >= maxHealth) return;
        // Else, heal
        currHealth += _health;
        // Prevent healing over max hp
        if (currHealth > maxHealth) currHealth = maxHealth;
        // If health heals back over 50%, restore healthy sprite
        if (currHealth > (maxHealth / 2))
        {
            mySR.sprite = healthySprite;
        }
    }

}
