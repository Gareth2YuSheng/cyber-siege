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

    private void Start()
    {
        speed = bullet.bulletSpeed;
        rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        if (!target) return;

        // Allows bullets to home on target
        Vector2 direction = (target.position - transform.position).normalized;
        rb.linearVelocity = direction * speed;

        // Rotate bullet to face target
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;
        transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, angle));
    }

    public void SetTarget(Transform _target)
    {
        target = _target;
    }

    public void SetBulletDamage(int _damage)
    {
        damage = _damage;
    }

    protected virtual void OnCollisionEnter2D(Collision2D collision)
    {
        // Debug.Log($"Collided with {collision.gameObject.name}");
        // If collision object is an tagged as an enemy or is in the enemy layer (enemy layer is 8)
        // and bullet has not collided with any other enemy (only 1 enemy per bullet)
        if (collision.gameObject.layer == LayerMask.NameToLayer("Enemy") && !hasCollided)
        {
            hasCollided = true;
            BasicEnemyScript enemy = collision.gameObject.GetComponent<BasicEnemyScript>();
            enemy.TakeDamage(damage);
            Destroy(gameObject);
        }
    }
}
