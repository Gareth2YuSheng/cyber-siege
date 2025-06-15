using UnityEngine;

public class AntivirusTowerScript : BasicTowerScript
{
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform firingPoint;

    protected override void Action()
    {
        GameObject bulletObj = Instantiate(bulletPrefab, firingPoint.position, Quaternion.identity);
        BulletScript bulletScript = bulletObj.GetComponent<BulletScript>();
        bulletScript.SetBulletDamage(damage);
        bulletScript.SetTarget(enemyTarget);
    }
}
