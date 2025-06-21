using UnityEngine;

public class ThreatIntelligenceTowerScript : BasicTowerScript
{
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform firingPoint;
    [SerializeField] private float rangeBuffFactor = 1.25f;
    [SerializeField] private float fireRateBuffFactor = 1.25f;

    protected override void Action()
    {
        Shoot();
    }

    protected override void FindEnemyTarget()
    {
        RaycastHit2D[] hits = Physics2D.CircleCastAll(transform.position, range, (Vector2)transform.position, 0f, enemyMask);

        //If there is a target in range
        if (hits.Length > 0)
        {
            // Allow Tower to hit all targets even if hidden but not reveal them
            // Except Virus Type enemies
            // enemyTarget = hits[0].transform;
            foreach (RaycastHit2D hit in hits)
            {
                // Check if target is hidden
                BasicEnemyScript enemyScript = hit.transform.GetComponentInParent<BasicEnemyScript>();
                if (enemyScript != null && !hit.transform.CompareTag("Virus Enemy"))
                {
                    enemyTarget = hit.transform;
                    return; // Return so only assignes the first one
                }
            }
        }
    }

    private void Shoot()
    {
        GameObject bulletObj = Instantiate(bulletPrefab, firingPoint.position, Quaternion.identity);
        BulletScript bulletScript = bulletObj.GetComponent<BulletScript>();
        bulletScript.SetBulletDamage(towerDamage);
        bulletScript.SetTarget(enemyTarget);
    }

    /* Upgrades
        Upgrade 1 - Threat Profiling
        Buffs towers fire rate within radius (deal with known threats faster)

        Upgrade 2 - Expanded Network Visibility
        Increases detection radius of nearby towers.
    */

    public override void Upgrade1()
    {
        base.Upgrade1();
    }
    public override void Upgrade2()
    {
        base.Upgrade2();
    }
}
