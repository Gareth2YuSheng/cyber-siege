using UnityEngine;

public class NetworkScrubberScript : BasicTowerScript
{
    [Header("References")]
    [SerializeField] private Transform[] firingPoints;
    [SerializeField] private Transform[] upgradedFiringPoints;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private GameObject normalBarrels;
    [SerializeField] private GameObject upgradedBarrels;

    protected override void Action()
    {
        Transform[] points;

        if (upgrades[1].purchased)
        {
            points = upgradedFiringPoints;
        }
        else
        {
            points = firingPoints;
        }

        foreach (Transform point in points)
        {
            // Direction calculated based on firing point transform relative to tower transform
            Vector2 direction = (point.position - transform.position).normalized;

            GameObject bulletObj = Instantiate(bulletPrefab, point.position, Quaternion.identity);
            BulletScript bulletScript = bulletObj.GetComponent<BulletScript>();
            bulletScript.SetBulletDamage(towerDamage);
            // Take into account the distance of the firing point from the center
            // of the tower when setting the bullet range
            float bulletRange = range - Vector3.Distance(transform.position, point.position);
            bulletScript.SetDirection(direction, bulletRange);
            // Increase bullet pierce if upgrade 1 was purchased
            if (upgrades[0].purchased)
            {
                bulletScript.SetMaxPierce(2);
            }
        }
    }

    /*Upgrades
        Upgrade 1 - Deep Packet Inspection
        Increased fire rate and bullets pierce more enemies

        Upgrade 2 - Parallel Scrubbing
        Increases the amount bullets being fired at a time
        (Doubles the number of firing points)
    */

    public override void Upgrade1()
    {
        base.Upgrade1();
        // Increase Fire Rate
        UpdateTowerBPS(1.5f);
    }
    public override void Upgrade2()
    {
        base.Upgrade2();
        // Swap to upgraded barrels
        normalBarrels.SetActive(false);
        upgradedBarrels.SetActive(true);
    }
}
