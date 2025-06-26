using System.Collections;
using UnityEngine;

public class AntivirusTowerScript : BasicTowerScript
{
    [Header("References")]
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private GameObject upgradedBulletPrefab;
    [SerializeField] private Transform firingPoint;

    [Header("Attributes")]
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

    // Upgrade 2 - Rapid Scan Protocol
    // - Burst fire mode

    private void Shoot()
    {
        Vector2 firingDirection = (firingPoint.position - transform.position).normalized;
        // If quarantine protocol enabled
        if (upgrades[0].purchased)
        {
            GameObject bulletObj = Instantiate(upgradedBulletPrefab, firingPoint.position, Quaternion.identity);
            VKSlowBulletScript bulletScript = bulletObj.GetComponent<VKSlowBulletScript>();
            bulletScript.SetBulletDamage(towerDamage);
            bulletScript.SetTarget(enemyTarget);
            bulletScript.SetSlowingDuration(slowingDuration);
            bulletScript.SetSlowingFactor(slowingFactor);
            // Rotate Bullet
            bulletScript.RotateBullet(firingDirection);
        }
        // else use the normal bullet
        else
        {
            GameObject bulletObj = Instantiate(bulletPrefab, firingPoint.position, Quaternion.identity);
            BulletScript bulletScript = bulletObj.GetComponent<BulletScript>();
            bulletScript.SetBulletDamage(towerDamage);
            bulletScript.SetTarget(enemyTarget);
            // Rotate Bullet
            bulletScript.RotateBullet(firingDirection);
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
