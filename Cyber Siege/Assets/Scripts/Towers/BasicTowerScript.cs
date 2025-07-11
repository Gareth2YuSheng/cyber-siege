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
    [SerializeField] public TowerUpgrade[] upgrades;
    [SerializeField] protected AudioClip effectAudio;
    [SerializeField] protected GameObject protectedEffect;

    //Attributes
    [NonSerialized] public string towerName;
    protected int cost;
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
    protected int fireRateDebuffStacks;

    //For Shooting
    protected Transform enemyTarget;
    protected float timeUntilFire;

    // For Ransomware
    protected bool disabled = false;
    protected bool safeFromRansomware = false; // To prevent disabling when using Encryption Node
    protected bool onFirstRansomwareHit = true; // Sanity check to prevent too fast a frame update, this will allow an X amt of seconds leeway
    protected EncryptionNodeScript encryptionNodeProtecting;

    // For Upgrade Menu
    protected Tile myTile;

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
        cost = tower.cost;

        baseBPS = bps;
        baseRange = range;

        UpdateTowerRangeTransform();
        // Hide Tower Range
        HideTowerRange();

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
        // If tower is not disabled and wave is ongoing
        if (!disabled && EnemyManager.main.waveOngoing) //not fully sanity tested
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
                    // Sound Effect
                    SoundManager.main.PlaySoundFXClip(effectAudio, 1f);

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

    public void SetMyTile(Tile tile)
    {
        myTile = tile;
    }

    protected virtual void OnMouseDown()
    {
        if (myTile != null)
        {
            myTile.OnTileClickedExternally();
        }
    }

    protected virtual void OnMouseEnter()
    {
        if (myTile != null)
        {
            myTile.OnTileEnteredExternally();
        }
    }

    protected virtual void OnMouseExit()
    {
        if (myTile != null)
        {
            myTile.OnTileExitedExternally();
        }
    }

    // For Upgrades
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
        CurrencyManager.main.SpendCurrency(_upgrade.cost);
    }

    // public bool HasPurchasedUpgrade(int index)
    // {
    //     if (index < 0 || index > 1) return false;
    //     return upgrades[index].purchased;
    // }

    // public int GetTowerCost()
    // {
    //     return cost;
    // }

    // public int GetTowerUpgrade1Cost()
    // {
    //     return upgrades[0].cost;
    // }

    // public int GetTowerUpgrade2Cost()
    // {
    //     return upgrades[1].cost;
    // }

    public int GetTotalMoneySpentOnTower()
    {
        int amt = cost;
        if (upgrades[0].purchased) amt += upgrades[0].cost;
        if (upgrades[1].purchased) amt += upgrades[1].cost;
        return amt;
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

    private void UpdateTowerRangeTransform()
    {
        // Range (Radius) is to be multiplied by 2 as X, Y and Z are length variables.
        towerRangeTransform.localScale = new Vector3(range * 2f, range * 2f, range * 2f);
    }

    public void ShowTowerRange()
    {
        towerRangeTransform.gameObject.SetActive(true);
    }

    public void HideTowerRange()
    {
        towerRangeTransform.gameObject.SetActive(false);
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

    public void AddTowerFireRateDebuffStack(int amt)
    {
        fireRateDebuffStacks += amt;
    }

    public void RemoveTowerFireRateDebuffStack(int amt)
    {
        fireRateDebuffStacks -= amt;
        if (fireRateDebuffStacks < 0)
        {
            fireRateDebuffStacks = 0;
        }
    }

    // For Ransomware
    public void DisableTower(BasicEnemyScript enemy)
    {
        // If safe from ransomware, do not disable, but stun 
        if (safeFromRansomware && onFirstRansomwareHit)
        {
            Debug.Log("KEEPING TOWER SAFE");
            onFirstRansomwareHit = false;
            // Disable the Encryption Node protecting it.
            // If upgrade is purchased stun enemy


            StartCoroutine(KeepTowerSafe(enemy));

        }
        else if (!disabled && !safeFromRansomware)
        {
            Debug.Log("DISABLE TOWER!");
            StartCoroutine(DisableTowerForSeconds());

        }
    }

    protected IEnumerator KeepTowerSafe(BasicEnemyScript enemy)
    {
        // Disable the Encryption Node that protects it for 6 seconds
        encryptionNodeProtecting.DisableEncryptionNode(this, enemy);
        // Wait for 6 seconds
        yield return new WaitForSeconds(6f);

        // Code to execute after 6 seconds
        Debug.Log("6 seconds have passed! Removing Ransomware Protection");
        safeFromRansomware = false;
        onFirstRansomwareHit = true;
    }

    // EncryptionNode - Protect the tower
    public void ProtectTower(EncryptionNodeScript theScript)
    {
        Debug.Log("Protect Tower");
        protectedEffect.SetActive(true);
        encryptionNodeProtecting = theScript;
        safeFromRansomware = true;
    }

    // EncryptionNode - Unprotect the tower
    public void UnProtectTower()
    {
        Debug.Log("Unprotect Tower");
        safeFromRansomware = false;
        protectedEffect.SetActive(false);
    }

    public void SetEncryptionNode(EncryptionNodeScript script)
    {
        encryptionNodeProtecting = script;
    }

    public void ResetEncryptionNode()
    {
        encryptionNodeProtecting = null;
    }

    protected IEnumerator DisableTowerForSeconds()
    {
        disabled = true;
        yield return new WaitForSeconds(6f);
        Debug.Log("No more disabled");
        disabled = false;
    }

    public bool isTowerDisabled()
    {
        return disabled;
    }

    public TowerPlacementType GetTowerType()
    {
        return tower.placementType;
    }
}
