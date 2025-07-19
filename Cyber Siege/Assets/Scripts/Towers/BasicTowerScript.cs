using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

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
    [SerializeField] protected GameObject disabledEffect;
    [SerializeField] protected GameObject upgradeLayer1;
    [SerializeField] protected GameObject upgradeLayer2;

    [Header("Base Events")]
    public UnityEvent<BasicTowerScript> onTowerDestroyed = new UnityEvent<BasicTowerScript>();

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

    //For Shooting
    protected Transform enemyTarget;
    protected float timeUntilFire;

    // For Ransomware
    protected bool disabled = false;
    protected bool safeFromRansomware = false; // To prevent disabling when using Encryption Node
    protected EncryptionNodeScript encryptionNodeProtecting;
    protected float towerDisabledDuration = 5f;

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

        // Hide Upgrade Layers if provided
        if (upgradeLayer1 != null)
        {
            upgradeLayer1.SetActive(false);
        }
        if (upgradeLayer2 != null)
        {
            upgradeLayer2.SetActive(false);
        }
        // Hide Effects
        protectedEffect.SetActive(false);
        disabledEffect.SetActive(false);
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
                float adjustedBps = bps * Mathf.Pow(0.7f, ServerManager.main.GetCryptojackingCount()); // 30% reduction per stack
                timeUntilFire += Time.deltaTime;
                if (timeUntilFire >= (1f / adjustedBps))
                {
                    if (effectAudio != null && SoundManager.main != null)
                    {
                        // Sound Effect
                        SoundManager.main.PlaySoundFXClip(effectAudio, 1f);
                    }
                    Action();
                    timeUntilFire = 0f;
                }
            }
        }
    }

    // Change for each Tower - Not decalred abstract as I dont want the class to be abstract
    protected virtual void Action() { }

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

    public virtual void DestroySelf()
    {
        onTowerDestroyed.Invoke(this);
        Destroy(gameObject);
    }

    // For Tower's Tile
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
        if (upgradeLayer1 != null)
        {
            upgradeLayer1.SetActive(true);
        }
    }

    public virtual void Upgrade2()
    {
        PurchaseUpgrade(upgrades[1]);
        if (upgradeLayer2 != null)
        {
            upgradeLayer2.SetActive(true);
        }
    }

    protected void PurchaseUpgrade(TowerUpgrade _upgrade)
    {
        Debug.Log("Selected Upgrade: " + _upgrade.upgradeName);
        // Mark Upgrade as purchased
        _upgrade.purchased = true;
        // Assume we checked that we can afford the upgrade
        CurrencyManager.main.SpendCurrency(_upgrade.cost);
    }

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

    // For Ransomware
    public void DisableTower(BasicEnemyScript enemy)
    {
        // If tower is being protected by an Encryption Node, and it is enabled
        if (encryptionNodeProtecting != null && !encryptionNodeProtecting.IsTowerDisabled())
        {
            // Disable the encryption Node Instead
            encryptionNodeProtecting.DisableEncryptionNode(this, enemy);
            // Let the Encryption Node handle the rest of the logic required
        }

        // Else if encryption node is already disabled, and we are not being protected 
        // OR we have no encryption node protecting us, AND tower is not already disabled
        else if ((encryptionNodeProtecting == null ||
        (encryptionNodeProtecting.IsTowerDisabled() && !safeFromRansomware))
        && !disabled)
        {
            // Disable the Tower
            StartCoroutine(Disable());
        }

        // Else, If the encryption Node is already disabled, but we are being protected,
        // Dont do anything
    }

    protected virtual IEnumerator Disable()
    {
        disabled = true;
        disabledEffect.SetActive(true);
        yield return new WaitForSeconds(towerDisabledDuration);
        disabled = false;
        disabledEffect.SetActive(false);
    }

    // EncryptionNode - Protect the tower
    public void ProtectTower()
    {
        protectedEffect.SetActive(true);
        safeFromRansomware = true;
    }

    // EncryptionNode - Unprotect the tower
    public void UnProtectTower()
    {
        protectedEffect.SetActive(false);
        safeFromRansomware = false;
    }

    public void SetEncryptionNode(EncryptionNodeScript script)
    {
        encryptionNodeProtecting = script;
    }

    public void ResetEncryptionNode()
    {
        encryptionNodeProtecting = null;
    }

    public bool IsTowerDisabled()
    {
        return disabled;
    }
}
