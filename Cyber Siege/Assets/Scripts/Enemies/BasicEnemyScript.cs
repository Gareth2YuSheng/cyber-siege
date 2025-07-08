using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class BasicEnemyScript : MonoBehaviour
{
    [Header("Base References")]
    [SerializeField] protected ScriptableEnemy enemy;
    [SerializeField] protected AudioClip audioClipDestroy;
    [SerializeField] protected Sprite stunnedSprite;
    [SerializeField] protected GameObject stunEffect;
    [SerializeField] protected GameObject slowEffect;

    [Header("Base Attributes")]
    public bool isHidden = false;

    [Header("Base Events")]

    public UnityEvent onTakeDamage = new UnityEvent();
    public UnityEvent<BasicEnemyScript> onEnemyDeath = new UnityEvent<BasicEnemyScript>();
    public UnityEvent onEnemyReveal = new UnityEvent();

    // Attributes
    protected int health;
    protected float moveSpeed;
    protected int currencyValue;
    protected int damageDealtToServer;
    protected bool isDestroyed = false;

    // For Sprite Changes
    protected Sprite baseSprite;
    protected SpriteRenderer sr;

    // For Pathing
    protected int pathIndex = 0;
    protected Transform movementTarget;
    protected Rigidbody2D rb;
    public bool isBlocked = false;

    // For Modifiers
    protected float baseMoveSpeed;
    protected float baseHealth;

    // For Debuffs
    protected float damageTakenMultiplier = 1f;
    protected bool isSlowed;
    protected int slowStackCounter;

    protected virtual void Start()
    {
        health = enemy.health;
        moveSpeed = enemy.moveSpeed;
        currencyValue = enemy.currencyValue;
        damageDealtToServer = enemy.damageDealtToServer;

        baseMoveSpeed = moveSpeed;
        baseHealth = health;

        sr = gameObject.GetComponent<SpriteRenderer>();
        rb = gameObject.GetComponent<Rigidbody2D>();

        baseSprite = sr.sprite;

        //Start moving
        UpdateMovementTarget();
    }

    // Allow Update to be overridable by children to add logic and behavior
    protected virtual void Update()
    {
        //For Basic Movement
        //Check if enemy is close to target
        // Debug.Log(Vector2.Distance(movementTarget.position, transform.position));
        // if (Vector2.Distance(movementTarget.position, transform.position) <= 0.1f)
        // {
        //     //Incement pathIndex
        //     pathIndex++;
        //     Debug.Log($"New Path Index: {pathIndex}");
        //     //If no more points / reached the end of the path
        //     if (pathIndex >= LevelManager.main.enemyPath.Length)
        //     {
        //         //Damange the server
        //         LevelManager.main.DamageServer(damageDealtToServer);
        //         DestroySelf();
        //         return;
        //     }
        //     //Else if the path as not ended, update the target to the next point
        //     else
        //     {
        //         UpdateMovementTarget();
        //     }
        // }
    }

    private void FixedUpdate()
    {
        // If Enemy is currently blocked, stop moving
        if (isBlocked)
        {
            // Reset linearVelocity if havent
            if (rb.linearVelocity.sqrMagnitude > 0.001f)
            {
                rb.linearVelocity = Vector2.zero;
            }
            return;
        }
        // For Movement
        // Direction cannot be calculated based on current enemy position due to 
        // the off-sync of the Unity Engine's calls for Update and FixedUpdate
        // this causes the direction vector calculated using the above formula to 
        // calculate a zero vector, causing the enemy to slow down at corners
        // Vector2 direction = GetMovementDirection();
        // rb.linearVelocity = direction * moveSpeed;

        // All movement to now be controlled in Fixed Update due to the off-sync of 
        // the Update and FixedUpdate method causing probelms

        Vector2 distToTarget = movementTarget.position - transform.position;

        // Use distance check to determine if enemy reached the point
        if (distToTarget.sqrMagnitude < 0.1f * 0.1f) // Adjust threshold as needed
        {
            pathIndex++;
            if (pathIndex >= LevelManager.main.enemyPath.Length)
            {
                LevelManager.main.DamageServer(damageDealtToServer);
                DestroySelf();
                return;
            }

            UpdateMovementTarget();
            distToTarget = movementTarget.position - transform.position;
        }

        rb.linearVelocity = distToTarget.normalized * moveSpeed;
    }

    // Stopping when collide with obstacle behavior
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Path Obstacle"))
        {
            BasicTowerScript towerScript = collision.gameObject.GetComponent<BasicTowerScript>();
            if (!towerScript.isTowerDisabled())
            {
                isBlocked = true;
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Path Obstacle"))
        {
            isBlocked = false;
        }
    }

    //Movement Related Functions
    protected void UpdateMovementTarget()
    {
        movementTarget = LevelManager.main.enemyPath[pathIndex];
    }

    public void UpdatePathIndex(int _pathIndex)
    {
        pathIndex = _pathIndex;
    }

    public int GetCurrentPathIndex()
    {
        return pathIndex;
    }

    public float GetBaseSpeed()
    {
        return baseMoveSpeed;
    }

    public void UpdateMovementSpeed(float amt)
    {
        // If we are going to be slowed, show the slow effect
        if (amt < 1f)
        {
            isSlowed = true;
            slowEffect.SetActive(true);
            // Add 1 to the slow stack, this is used 
            // to track how many of the slow effects is currently being applied to the enemy
            slowStackCounter++;
        }
        moveSpeed = baseMoveSpeed * amt;
    }

    public IEnumerator UpdateMovementSpeed(float amt, float duration)
    {
        UpdateMovementSpeed(amt);
        yield return new WaitForSeconds(duration);
        ResetMovementSpeed();
    }

    public void ResetMovementSpeed()
    {
        slowStackCounter--;
        // if the number of slow stacks is > 0, 
        // means we are still in a debuff zone, 
        // dont reset the speed
        if (slowStackCounter < 1)
        {
            isSlowed = false;
            slowEffect.SetActive(false);
            moveSpeed = baseMoveSpeed;
        }
    }

    public Vector3 GetMovementDirection()
    {
        // If pathIndex is 0, meaning we shld take enemyPath[0] - startPoint
        if (pathIndex == 0)
        {
            Vector3 firstTarget = LevelManager.main.enemyPath[0].position;
            Vector3 startPoint = LevelManager.main.startPoint.position;
            return (firstTarget - startPoint).normalized;
        }

        // Else, Calculate direction moving based on current and previous pathIndex points
        Vector3 currentMovementTarget = LevelManager.main.enemyPath[pathIndex].position;
        Vector3 prevMovementTarget = LevelManager.main.enemyPath[pathIndex - 1].position;

        return (currentMovementTarget - prevMovementTarget).normalized;
    }

    // public void Blocked()
    // {
    //     isBlocked = true;
    // }

    // public void Unblocked()
    // {
    //     isBlocked = false;
    // }

    protected virtual void ToggleStunnedSprite(bool stun)
    {
        if (stunnedSprite != null)
        {
            // if slow effect is currently turned on, turn off temporarily
            if (isSlowed && slowEffect != null) slowEffect.SetActive(!stun);
            // toggle stunned sprite and stun effect
            sr.sprite = stun ? stunnedSprite : baseSprite;
            stunEffect.SetActive(stun);
            // Turn slow back on
            // if (isSlowed && slowEffect != null && stun) slowEffect.SetActive(true);
        }
    }

    public IEnumerator Stun(float duration)
    {
        isBlocked = true;
        // Switch to stunned sprite (if available)
        ToggleStunnedSprite(true);
        yield return new WaitForSeconds(duration);
        // Switch back to original sprite
        ToggleStunnedSprite(false);
        isBlocked = false;
    }

    // Health Related Functions
    public virtual void TakeDamage(int dmg)
    {
        // If damage multiplier applied, include in damage calculation
        health -= (int)(dmg * damageTakenMultiplier);
        Debug.Log($"Damage taken {dmg * damageTakenMultiplier}");
        onTakeDamage.Invoke();

        if (health <= 0 && !isDestroyed)
        {
            // Invoke the Event
            onEnemyDeath.Invoke(this);
            // Increase player money
            LevelManager.main.IncreaseCurrency(currencyValue);
            // Destroy Game Object
            DestroySelf();
        }
    }

    public void DestroySelf()
    {
        isDestroyed = true;
        Destroy(gameObject);
        EnemyManager.main.EnemyDestroyed();
        // Play Enemy Death Sound Effect
        if (audioClipDestroy != null)
        {
            SoundManager.main.PlaySoundFXClip(audioClipDestroy, 1f);
        }
    }

    // Hidden Enemy Related Functions
    public void Reveal()
    {
        isHidden = false;
        // Increase opacity
        changeOpacity(1f);
        onEnemyReveal.Invoke();
    }

    public void Hide()
    {
        isHidden = true;
    }

    public int GetDamageDealtToServer()
    {
        return damageDealtToServer;
    }

    // Debuff Related Functions
    public void SetTakenDamageMultiplier(float multiplier)
    {
        damageTakenMultiplier = multiplier;
    }

    public IEnumerator SetTakenDamageMultiplier(float multiplier, float duration)
    {
        SetTakenDamageMultiplier(multiplier);
        yield return new WaitForSeconds(duration);
        ResetTakenDamageMultiplier();
    }

    public void ResetTakenDamageMultiplier()
    {
        damageTakenMultiplier = 1f;
    }

    // Change opacity -> 1f = 100%
    public void changeOpacity(float amount)
    {
        Color color = sr.color;
        color.a = amount;
        sr.color = color;
    }
}
