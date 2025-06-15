using UnityEngine;

public class BulletScript : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private ScriptableBullet bullet;

    private Rigidbody2D rb;
    private Transform target;
    private float speed;
    private int damage = 0;
    private bool hasCollided = false;

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
        rb.linearVelocity = direction * speed; //Alternatively use Transform translate method?
    }

    public void SetTarget(Transform _target)
    {
        target = _target;
    }

    public void SetBulletDamage(int _damage)
    {
        damage = _damage;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Debug.Log($"Collided with {collision.gameObject.name}");
        // If collision object is an tagged as an enemy or is in the enemy layer (enemy layer is 8)
        // and bullet has not collided with any other enemy (only 1 enemy per bullet)
        if ((collision.gameObject.tag == "Enemy" || collision.gameObject.layer == 8) && !hasCollided)
        {
            hasCollided = true;
            BasicEnemyScript enemy = collision.gameObject.GetComponent<BasicEnemyScript>();
            enemy.TakeDamage(damage);
            Destroy(gameObject);
        }
    }
}
