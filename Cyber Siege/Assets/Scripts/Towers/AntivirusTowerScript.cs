using System.Collections;
using UnityEngine;

public class AntivirusTowerScript : BasicTowerScript
{
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private GameObject upgradedBulletPrefab;
    [SerializeField] private Transform firingPoint;
    [SerializeField] private int burstCount = 3;
    [SerializeField] private float burstCoolDown = 0.1f;
    [SerializeField] private float slowingFactor = 0.4f;
    [SerializeField] private float slowingDuration = 2.0f;

    // private bool burstMode = false;

    protected override void Action()
    {
        // If burst mode enabled - burst fire
        if (upgrades[1].purchased)
        {
            StartCoroutine(BurstFire());
        }
        // Else shoot normally
        else
        {
            Shoot();
        }
    }

    // Upgrade 1 - Quarantine Protocol
    //  - Virus enemies hit are slowed by X% for Y seconds.
    // public override void Upgrade1()
    // {
    //     base.Upgrade1();
    // }

    // Upgrade 2 - Rapid Scan Protocol
    // - Burst fire mode
    // public override void Upgrade2()
    // {
    //     base.Upgrade2();
    // }

    private void Shoot()
    {
        // If quarantine protocol enabled
        if (upgrades[0].purchased)
        {
            GameObject bulletObj = Instantiate(upgradedBulletPrefab, firingPoint.position, Quaternion.identity);
            VKSlowBulletScript bulletScript = bulletObj.GetComponent<VKSlowBulletScript>();
            bulletScript.SetBulletDamage(damage);
            bulletScript.SetTarget(enemyTarget);
            bulletScript.SetSlowingDuration(slowingDuration);
            bulletScript.SetSlowingFactor(slowingFactor);
        }
        // else use the normal bullet
        else
        {
            GameObject bulletObj = Instantiate(bulletPrefab, firingPoint.position, Quaternion.identity);
            BulletScript bulletScript = bulletObj.GetComponent<BulletScript>();
            bulletScript.SetBulletDamage(damage);
            bulletScript.SetTarget(enemyTarget);
        }
    }

    private IEnumerator BurstFire()
    {
        for (int i = 0; i < burstCount; i++)
        {
            Shoot();
            yield return new WaitForSeconds(burstCoolDown);
        }
    }
}
