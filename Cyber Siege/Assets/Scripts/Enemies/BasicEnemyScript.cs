using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class BasicEnemyScript : MonoBehaviour, IPooledObject
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
    public bool hasReachedServer = false;

    // For Modifiers
    protected float baseMoveSpeed;
    protected float baseHealth;

    // For Debuffs
    protected float damageTakenMultiplier = 1f;
    protected bool isSlowed = false;
    protected int slowStackCounter = 0;

    // For Object Pooling
    protected string objectPoolTag;

    public virtual void InitialiseEnemy()
    {
        if (enemy == null) return;

        health = enemy.health;
        moveSpeed = enemy.moveSpeed;
        currencyValue = enemy.currencyValue;
        damageDealtToServer = enemy.damageDealtToServer;

        baseMoveSpeed = moveSpeed;
        baseHealth = health;

        objectPoolTag = enemy.objectPoolTag;

        sr = gameObject.GetComponent<SpriteRenderer>();
        rb = gameObject.GetComponent<Rigidbody2D>();

        baseSprite = sr.sprite;

        // Hide Effects
        slowEffect.SetActive(false);
        stunEffect.SetActive(false);
    }

    protected virtual void ResetEnemy()
    {
        // Reset Attributes
        health = enemy.health;
        moveSpeed = enemy.moveSpeed;
        currencyValue = enemy.currencyValue;
        damageDealtToServer = enemy.damageDealtToServer;
        isDestroyed = false;
        isHidden = false;
        // Reset Sprite
        sr.sprite = baseSprite;
        // Hide Effects
        slowEffect.SetActive(false);
        stunEffect.SetActive(false);
        // Reset Debuffs
        damageTakenMultiplier = 1f;
        isSlowed = false;
        slowStackCounter = 0;
        // Reset Enemy Movement
        isBlocked = false;
        hasReachedServer = false;
        pathIndex = 0;
        // pathIndex = 0;
        movementTarget = null;
        // Start Moving
        UpdateMovementTarget();
    }

    protected virtual void Awake()
    {
        // Placed here instead of start as with Object Pooling now, 
        // Start only runs after enemy spawns
        InitialiseEnemy();
    }

    protected virtual void Start()
    {
        // Debug.Log("---------Running Enemy Start");
    }

    // Allow Update to be overridable by children to add logic and behavior
    protected virtual void Update()
    {

    }

    private void FixedUpdate()
    {
        // If no movement target, dont move
        if (movementTarget == null) return;

        // If Enemy is currently blocked OR has alr reached the server, stop moving
        if (isBlocked || hasReachedServer)
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
            if (pathIndex >= EnemyManager.main.enemyPath.Length)
            {
                ReachedServer();
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
            if (!towerScript.IsTowerDisabled())
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

    // Object Pooling Functions
    public virtual void OnObjectSpawn()
    {
        ResetEnemy();
    }

    public virtual void ReturnPooledObject()
    {
        if (ObjectManager.main == null)
        {
            Debug.LogWarning("Object Pool is not in the scene");
            return;
        }
        // Debug.Log("Returning Enemy To <" + objectPoolTag + "> Pool");

        // Reset Enemy's position to start point
        // Or else towers will detect enemies where they last died
        gameObject.transform.position = EnemyManager.main.startPoint.position;

        ObjectManager.main.ReturnToPool(objectPoolTag, gameObject);
    }

    public virtual string GetEnemyName()
    {
        return enemy.name;
    }

    // For Unit Testing
    public int GetHealth()
    {
        return health;
    }

    protected virtual void ReachedServer()
    {
        hasReachedServer = true;
        HealthManager.main.DamageServer(damageDealtToServer);
        DestroySelf();
    }

    //Movement Related Functions
    protected virtual void UpdateMovementTarget()
    {
        if (EnemyManager.main == null)
        {
            Debug.LogWarning("Missing Enemy Manager");
            return;
        }
        movementTarget = EnemyManager.main.enemyPath[pathIndex];
    }

    public void UpdatePathIndexForSpawn(int _pathIndex)
    {
        pathIndex = _pathIndex;
        UpdateMovementTarget();
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
        if (amt < 0) throw new ArgumentOutOfRangeException("Cannot Update to Negative Speed");
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
        if (slowStackCounter > 0) slowStackCounter--;
        // if the number of slow stacks is still > 0, 
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
            Vector3 firstTarget = EnemyManager.main.enemyPath[0].position;
            Vector3 startPoint = EnemyManager.main.startPoint.position;
            return (firstTarget - startPoint).normalized;
        }

        // Else, Calculate direction moving based on current and previous pathIndex points
        Vector3 currentMovementTarget = EnemyManager.main.enemyPath[pathIndex].position;
        Vector3 prevMovementTarget = EnemyManager.main.enemyPath[pathIndex - 1].position;

        return (currentMovementTarget - prevMovementTarget).normalized;
    }

    public void Block()
    {
        isBlocked = true;
    }

    public void Unblock()
    {
        isBlocked = false;
    }

    // Sprite Related Functions

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
        // Debug.Log($"Damage taken {dmg * damageTakenMultiplier}");
        onTakeDamage.Invoke();

        if (health <= 0 && !isDestroyed)
        {
            // Invoke the Event
            onEnemyDeath.Invoke(this);
            // Increase player money
            if (CurrencyManager.main != null)
            {
                CurrencyManager.main.GainCurrencyFromKillingEnemy(currencyValue);
            }
            else
            {
                Debug.Log("CurrencyManager cannot be found");
            }
            // Destroy Game Object
            DestroySelf();
        }
    }

    public virtual void DestroySelf()
    {
        isDestroyed = true;
        // Destroy(gameObject);
        ReturnPooledObject();
        EnemyManager.main?.EnemyDestroyed();
        // Play Enemy Death Sound Effect
        if (audioClipDestroy != null)
        {
            SoundManager.main?.PlaySoundFXClip(audioClipDestroy, 1f);
        }
    }

    public bool IsDestroyed()
    {
        return isDestroyed;
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

    protected void Vanish()
    {
        sr.enabled = false;
    }

    protected void UnVanish()
    {
        sr.enabled = true;
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
