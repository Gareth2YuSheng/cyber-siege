using UnityEngine;

public class BulletScript : MonoBehaviour
{
    [Header("References")]
    [SerializeField] protected ScriptableBullet bullet;

    protected Rigidbody2D rb;
    protected Transform target;
    protected float speed;
    protected int damage = 0;
    protected bool hasCollided = false;
    protected Vector2 direction;
    protected Vector3 spawnPosition;
    protected float maxRange;
    protected int pierceCount;
    protected int maxPierce = 1;
    protected string mode = "";

    private void Start()
    {
        speed = bullet.bulletSpeed;
        rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        // Do nothing if mode has not been set
        if (mode == "") return;

        if (mode == "TARGET")
        {
            // If target dies, destroy self as cleanup
            if (target == null)
            {
                Destroy(gameObject);
                return;
            }

            // Allows bullets to home on target
            direction = (target.position - transform.position).normalized;

            // Rotate bullet to face target
            RotateBullet(direction);
        }
        else if (mode == "DIRECTIONAL")
        {
            // Make bullet destroy itself if out of range
            float distanceTravelled = Vector3.Distance(transform.position, spawnPosition);
            if (distanceTravelled > maxRange)
            {
                Destroy(gameObject);
                return;
            }
        }

        rb.linearVelocity = direction * speed;
    }

    public void RotateBullet(Vector2 _direction)
    {
        float angle = Mathf.Atan2(_direction.y, _direction.x) * Mathf.Rad2Deg - 90f;
        transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, angle));
    }

    public void SetTarget(Transform _target)
    {
        target = _target;
        mode = "TARGET";
    }

    public void SetDirection(Vector2 _direction, float range)
    {
        direction = _direction.normalized;
        // Set the spawn point of the bullet
        spawnPosition = transform.position;
        // Pre rotate the bullet
        RotateBullet(direction);
        // Set the max range of the bullet
        maxRange = range;
        mode = "DIRECTIONAL";
    }

    public void SetBulletDamage(int _damage)
    {
        damage = _damage;
    }

    public void SetMaxPierce(int amount)
    {
        maxPierce = amount;
    }

    protected virtual void OnCollisionEnter2D(Collision2D collision)
    {
        // Debug.Log($"Collided with {collision.gameObject.name}");
        // If collision object is an tagged as an enemy or is in the enemy layer (enemy layer is 8)
        // and bullet has not collided with more than maxPierce enemy count
        if (collision.gameObject.layer == LayerMask.NameToLayer("Enemy") && pierceCount < maxPierce)
        {
            hasCollided = true;
            pierceCount++;
            BasicEnemyScript enemy = collision.gameObject.GetComponent<BasicEnemyScript>();
            enemy.TakeDamage(damage);
            Destroy(gameObject);
        }
    }
}
