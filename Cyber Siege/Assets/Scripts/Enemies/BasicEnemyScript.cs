using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class BasicEnemyScript : MonoBehaviour
{
    [Header("Base References")]
    [SerializeField] protected ScriptableEnemy enemy;

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

    protected virtual void Start()
    {
        health = enemy.health;
        moveSpeed = enemy.moveSpeed;
        currencyValue = enemy.currencyValue;
        damageDealtToServer = enemy.damageDealtToServer;

        baseMoveSpeed = moveSpeed;
        baseHealth = health;

        rb = gameObject.GetComponent<Rigidbody2D>();

        //Start moving
        UpdateMovementTarget();
    }

    // Allow Update to be overridable by children to add logic and behavior
    protected virtual void Update()
    {
        //For Basic Movement
        //Check if enemy is close to target
        if (Vector2.Distance(movementTarget.position, transform.position) <= 0.1f)
        {
            //Incement pathIndex
            pathIndex++;
            //If no more points / reached the end of the path
            if (pathIndex >= LevelManager.main.enemyPath.Length)
            {
                //Damange the server
                LevelManager.main.DamageServer(damageDealtToServer);
                DestroySelf();
                return;
            }
            //Else if the path as not ended, update the target to the next point
            else
            {
                UpdateMovementTarget();
            }
        }
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
        Vector2 direction = (movementTarget.position - transform.position).normalized;
        rb.linearVelocity = direction * moveSpeed;
    }

    // Stopping when collide with obstacle behavior
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Path Obstacle"))
        {
            BasicTowerScript towerScript = collision.gameObject.GetComponent<BasicTowerScript>();
            if (!towerScript.disabled)
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
        moveSpeed = baseMoveSpeed;
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

    public IEnumerator Stun(float duration)
    {
        isBlocked = true;
        yield return new WaitForSeconds(duration);
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
    }

    // Hidden Enemy Related Functions
    public void Reveal()
    {
        isHidden = false;
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
}
