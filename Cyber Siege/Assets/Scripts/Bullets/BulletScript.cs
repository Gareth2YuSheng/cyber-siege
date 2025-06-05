using UnityEngine;

public class BulletScript : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private ScriptableBullet bullet;

    private Rigidbody2D rb;
    private Transform target;
    private float speed;
    private int damage;

    private void Start()
    {
        speed = bullet.bulletSpeed;
        damage = bullet.bulletDamage;
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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // If collision object is an tagged as an enemy
        if (collision.gameObject.tag == "Enemy")
        {
            BasicEnemyScript enemy = collision.gameObject.GetComponent<BasicEnemyScript>();
            enemy.TakeDamage(damage);
            Destroy(gameObject);
        }
    }
}
