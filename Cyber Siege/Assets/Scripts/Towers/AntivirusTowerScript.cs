using UnityEngine;

public class AntivirusTowerScript : BasicTowerScript
{
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform firingPoint;

    // public override string[] upgradeNames { get; } = new string[]
    // { "Quarantine Protocol", "Rapid Scan Protocol" };

    protected override void Action()
    {
        GameObject bulletObj = Instantiate(bulletPrefab, firingPoint.position, Quaternion.identity);
        BulletScript bulletScript = bulletObj.GetComponent<BulletScript>();
        bulletScript.SetBulletDamage(damage);
        bulletScript.SetTarget(enemyTarget);
    }

    public override void Upgrade1()
    {
        Debug.Log("Antivirus Tower Upgrade 1");
        Debug.Log(upgrades[0].upgradeName);
    }

    public override void Upgrade2()
    {
        Debug.Log("Antivirus Tower Upgrade 2");
        Debug.Log(upgrades[1].upgradeName);
    }
}
