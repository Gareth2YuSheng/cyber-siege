using System;
using System.Collections;
using UnityEngine;

public class BasicTowerScript : MonoBehaviour
{
    [Header("Base References")]
    [SerializeField] protected ScriptableTower tower;
    [SerializeField] protected Transform towerRangeTransform;
    [SerializeField] protected LayerMask enemyMask;
    [SerializeField] protected Transform turretRotationPart;
    // [SerializeField] private GameObject bulletPrefab;
    // [SerializeField] private Transform firingPoint;

    [SerializeField] public TowerUpgrade[] upgrades;
    // [SerializeField] protected virtual TowerUpgrade[] upgrades { get; } = new TowerUpgrade[] { };
    // public virtual string[] upgradeNames { get; } = new string[] { "Upgrade 1", "Upgrade 2" };
    // public virtual string[] upgradeDescriptions { get; } = new string[] { "Desc 1", "Desc 2" };

    //Attributes
    [NonSerialized] public string towerName;
    // private int cost;
    protected float range; // Radius
    protected int towerDamage;
    protected float rotationSpeed;
    protected float bps;
    // protected int level = 1;
    protected bool isRotatable;

    //For Modification (Upgrades)
    protected int baseUpgradeCost;
    protected float baseBPS;
    protected float baseRange;

    //For Shooting
    protected Transform enemyTarget;
    protected float timeUntilFire;

    // For Ransomware
    public RansomwareScript ransomwareScript; // Reference to RansomwareScript
    protected RansomwareScript ransomware; // For caching
    public bool disabled = false;

    public virtual void InitialiseTower()
    {
        towerName = tower.towerName;
        // cost = tower.cost;
        range = tower.range;
        rotationSpeed = tower.rotationSpeed;
        bps = tower.bps;
        baseUpgradeCost = tower.baseUpgradeCost;
        isRotatable = tower.isRotatable;
        towerDamage = tower.damage;

        baseBPS = bps;
        baseRange = range;

        UpdateTowerRangeTransform();

        // Populate tower upgrades if empty
        if (upgrades.Length == 0)
        {
            upgrades = new TowerUpgrade[2];
            for (int i = 0; i < upgrades.Length; i++)
            {
                upgrades[i] = new TowerUpgrade
                {
                    upgradeName = "N/A",
                    description = "",
                    cost = 0,
                    purchased = false
                };
            }
        }
    }

    protected virtual void Update()
    {
        // Ransomware handling
        FindRansomwareScript();
        if (!disabled)
        {
            // If no target, look for one
            if (enemyTarget == null)
            {
                FindEnemyTarget();
                return;
            }
            //If target goes out of range, reset target
            if (!CheckTargetIsInRange())
            {
                enemyTarget = null;
            }
            //Else Shoot at it
            else
            {
                //If tower is rotatable, rotate towards target
                if (isRotatable) RotateTowardsTarget();
                //Shoot
                timeUntilFire += Time.deltaTime;
                if (timeUntilFire >= (1f / bps))
                {
                    Action();
                    timeUntilFire = 0f;
                }
            }
        }
    }

    // Change for each Tower
    protected virtual void Action() { }

    // Change for each Tower
    protected virtual void FindEnemyTarget()
    {
        RaycastHit2D[] hits = Physics2D.CircleCastAll(transform.position, range, (Vector2)transform.position, 0f, enemyMask);

        //If there is a target in range
        if (hits.Length > 0)
        {
            // enemyTarget = hits[0].transform;
            // Only target non-hidden enemies
            foreach (RaycastHit2D hit in hits)
            {
                // Check if target is hidden
                BasicEnemyScript enemyScript = hit.transform.GetComponentInParent<BasicEnemyScript>();
                if (enemyScript != null && !enemyScript.isHidden)
                {
                    // Invoke to un-disguise Trojans. (For select towers)
                    // Move logic to specific towers
                    // Debug.Log("HIDDEN FOUND!");
                    // enemyScript.Reveal();

                    enemyTarget = hit.transform;
                    return; // Return so only assignes the first one
                }
            }
        }
    }

    protected bool CheckTargetIsInRange()
    {
        return Vector2.Distance(enemyTarget.position, transform.position) <= range;
    }

    private void RotateTowardsTarget()
    {
        float angle = Mathf.Atan2(enemyTarget.position.y - transform.position.y, enemyTarget.position.x - transform.position.x) * Mathf.Rad2Deg - 90f;

        Quaternion targetRotation = Quaternion.Euler(new Vector3(0f, 0f, angle));
        // Has some snapping motion
        // turretRotationPoint.rotation = targetRotation;
        // No snapping motion
        turretRotationPart.rotation = Quaternion.RotateTowards(turretRotationPart.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }

    private void UpdateTowerRangeTransform()
    {
        // Range (Radius) is to be multiplied by 2 as X, Y and Z are length variables.
        towerRangeTransform.localScale = new Vector3(range * 2f, range * 2f, range * 2f);
    }

    // For stat upgrades

    // public int CalculateUpgradeCost()
    // {
    //     return Mathf.RoundToInt(baseUpgradeCost * Mathf.Pow(level, 0.8f));
    // }

    // public float CalculateBPS()
    // {
    //     return baseBPS * Mathf.Pow(level, 0.6f);
    // }

    // public float CalculateTargetingRange()
    // {
    //     return range * Mathf.Pow(level, 0.4f);
    // }

    // public void UpgradeStats()
    // {
    //     if (CalculateUpgradeCost() > LevelManager.main.currency) return;

    //     LevelManager.main.SpendCurrency(CalculateUpgradeCost());

    //     level++;
    //     bps = CalculateBPS();
    //     range = CalculateTargetingRange();

    //     // CloseUpgradeUI();
    //     Debug.Log("New BPS: " + bps);
    //     Debug.Log("New Range: " + range);
    //     Debug.Log("New Cost: " + CalculateUpgradeCost());
    // }

    // For Behavioral Upgrades
    public virtual void Upgrade1()
    {
        PurchaseUpgrade(upgrades[0]);
    }

    public virtual void Upgrade2()
    {
        PurchaseUpgrade(upgrades[1]);
    }

    protected void PurchaseUpgrade(TowerUpgrade _upgrade)
    {
        Debug.Log("Selected Upgrade: " + _upgrade.upgradeName);
        // Mark Upgrade as purchased
        _upgrade.purchased = true;
        // Assume we checked that we can afford the upgrade
        LevelManager.main.SpendCurrency(_upgrade.cost);
    }

    // For Tower Range
    public float GetTowerRange()
    {
        return range;
    }

    public float GetTowerBaseRange()
    {
        return baseRange;
    }

    public void UpdateTowerRange(float amt)
    {
        range = baseRange * amt;
        UpdateTowerRangeTransform();
    }

    public void ResetTowerRange()
    {
        range = baseRange;
        UpdateTowerRangeTransform();
    }

    // For Tower Fire Rate
    public float GetTowerBPS()
    {
        return bps;
    }

    public float GetTowerBaseBPS()
    {
        return baseBPS;
    }

    public void UpdateTowerBPS(float amt)
    {
        bps = baseBPS * amt;
    }

    public void ResetTowerBPS()
    {
        bps = baseBPS;
    }

    // Ransomware related functions
    public virtual void FindRansomwareScript()
    {
        // Debug.Log("Running");
        // Dynamically find and update the reference to the RansomwareScript each frame
        ransomwareScript = FindFirstObjectByType<RansomwareScript>();

        if (ransomwareScript != null && ransomware == null)
        {
            ransomware = ransomwareScript;
            // Subscribe to an event or start logic based on ransomwareScript
            ransomwareScript.onDisable.AddListener(DisableTower); // Example of adding a listener
        }
    }

    protected void DisableTower()
    {
        if (!disabled)
        {
            Debug.Log("DISABLE TOWER!");
            disabled = true;
            // Start couroutine to enable after fixed amount of time
            StartCoroutine(EnableTower());
        }
    }

    protected IEnumerator DisableTower(float duration)
    {
        // If currently already disabled, dont do anything
        if (!disabled)
        {
            disabled = true;
            // Wait for duration
            yield return new WaitForSeconds(duration);
            // You can place any logic you want to perform after the X seconds here
            // Example:
            // PerformAction();
            disabled = false;
        }
    }

    protected IEnumerator EnableTower()
    {
        // Wait for 5 seconds
        yield return new WaitForSeconds(5f);

        // Code to execute after 5 seconds
        Debug.Log("5 seconds have passed! Enabling tower...");

        // You can place any logic you want to perform after the 5 seconds here
        // Example:
        // PerformAction();
        disabled = false;
    }

}
