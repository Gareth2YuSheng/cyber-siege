using UnityEngine;
using UnityEngine.Events;

public class BasicEnemyScript : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private ScriptableEnemy enemy;

    [Header("Events")]
    public UnityEvent onEnemyDeath = new UnityEvent();

    //Attributes
    private int health;
    private float moveSpeed;
    private int currencyValue;
    private int damageDealtToServer;
    private bool isDestroyed = false;

    //For Pathing
    private int pathIndex = 0;
    private Transform movementTarget;
    private Rigidbody2D rb;

    //For Modifiers
    private float baseMoveSpeed;
    private float baseHealth;

    private void Start()
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

    private void Update()
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
        //For Movement
        Vector2 direction = (movementTarget.position - transform.position).normalized;
        rb.linearVelocity = direction * moveSpeed;
    }

    //Movement Related Functions
    private void UpdateMovementTarget()
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

    public void UpdateMovementSpeed(float newMoveSpeed)
    {
        moveSpeed = newMoveSpeed;
    }

    public void ResetMovementSpeed()
    {
        moveSpeed = baseMoveSpeed;
    }

    // Health Related Functions
    public void TakeDamage(int dmg)
    {
        health -= dmg;

        if (health <= 0 && !isDestroyed)
        {
            // Invoke the Event
            onEnemyDeath.Invoke();
            // Increase player money
            LevelManager.main.IncreaseCurrency(currencyValue);
            // Destroy Game Object
            DestroySelf();
        }
    }

    private void DestroySelf()
    {
        isDestroyed = true;
        Destroy(gameObject);
        EnemyManager.main.EnemyDestroyed();
    }
}
