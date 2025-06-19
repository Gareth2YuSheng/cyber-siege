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
        TowerUpgrade upgrade = upgrades[0];
        Debug.Log("Selected Upgrade: " + upgrade.upgradeName);
        // Mark Upgrade as purchased
        upgrade.purchased = true;
        // Assume we checked that we can afford the upgrade
        LevelManager.main.SpendCurrency(upgrade.cost);
    }

    public override void Upgrade2()
    {
        TowerUpgrade upgrade = upgrades[1];
        Debug.Log("Selected Upgrade: " + upgrade.upgradeName);
        // Mark Upgrade as purchased
        upgrade.purchased = true;
        // Assume we checked that we can afford the upgrade
        LevelManager.main.SpendCurrency(upgrade.cost);
    }
}
