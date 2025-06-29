using UnityEngine;

public class ThreatIntelligenceTowerScript : BasicTowerScript
{
    [Header("References")]
    // [SerializeField] private GameObject buffArea;
    [SerializeField] private LayerMask towerMask;

    [Header("Attributes")]
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform firingPoint;
    [SerializeField] private float rangeBuffFactor = 1.25f;
    [SerializeField] private float fireRateBuffFactor = 1.25f;

    // private CircleCollider2D buffAreaCollider;
    private BasicTowerScript myScript;

    public override void InitialiseTower()
    {
        base.InitialiseTower();
        myScript = gameObject.GetComponent<BasicTowerScript>();
        // Add Event Listener
        BuildManager.main.onTowerBuilt.AddListener(ScanForTowersInRange);
    }

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
        Increases detection radius of nearby towers
    */

    public override void Upgrade1()
    {
        base.Upgrade1();
        ScanForTowersInRange();
    }
    public override void Upgrade2()
    {
        base.Upgrade2();
        ScanForTowersInRange();
    }

    private void ScanForTowersInRange()
    {
        Debug.Log("ThreatIntelTower Scanning For New Towers");

        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, range, towerMask);
        foreach (Collider2D hit in hits)
        {
            // Hits will be objects with colliders, which are currently the tower Bases
            // Script is in parent object
            BasicTowerScript tower = hit.GetComponentInParent<BasicTowerScript>();
            // Dont scan for itself
            if (tower != null && tower != myScript)
            {
                Debug.Log($"Found tower: {tower.towerName}");
                // If upgrade 1 has been purchased
                if (upgrades[0].purchased)
                {
                    tower.UpdateTowerBPS(fireRateBuffFactor);
                }
                // If upgrade 2 has been purchased, Buff the tower's range if havent already
                if (upgrades[1].purchased && tower.GetTowerRange() == tower.GetTowerBaseRange())
                {
                    tower.UpdateTowerRange(rangeBuffFactor);
                }
            }
        }
    }
}
