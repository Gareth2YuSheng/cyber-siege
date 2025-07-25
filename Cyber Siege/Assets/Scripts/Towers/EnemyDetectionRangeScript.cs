using UnityEngine;

public class EnemyDetectionRangeScript : MonoBehaviour
{
    private CircleCollider2D slowingRangeCollider;
    private TwoFA_GateScript myGate;

    private void Awake()
    {
        slowingRangeCollider = gameObject.GetComponent<CircleCollider2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            BasicEnemyScript enemy = collision.GetComponent<BasicEnemyScript>();
            if (enemy != null)
            {
                myGate.AddToSlowedEnemies(enemy);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            BasicEnemyScript enemy = collision.GetComponent<BasicEnemyScript>();
            if (enemy != null)
            {
                myGate.RemovedFromSlowedEnemies(enemy);
            }
        }
    }

    public void SetDetectionRange(float _range)
    {
        slowingRangeCollider.radius = _range;
    }

    public void SetMyTower(TwoFA_GateScript gate)
    {
        myGate = gate;
    }
}
