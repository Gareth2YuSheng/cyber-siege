using UnityEngine;

public class BasicTowerScript : MonoBehaviour
{
    [Header("References")]
    [SerializeField] protected ScriptableTower tower;
    [SerializeField] protected Transform towerRangeTransform;
    [SerializeField] protected LayerMask enemyMask;
    [SerializeField] protected Transform turretRotationPart;
    // [SerializeField] private GameObject bulletPrefab;
    // [SerializeField] private Transform firingPoint;

    // [SerializeField] private GameObject upgradeUI;
    // [SerializeField] private Button upgradeButton;

    //Attributes
    // private string towerName;
    // private int cost;
    protected float range; // Radius
    protected float rotationSpeed;
    protected float bps;
    protected int level = 1;
    protected bool isRotatable;

    //For Modification (Upgrades)
    protected int baseUpgradeCost;
    protected float baseBPS;
    protected float baseRange;

    //For Shooting
    protected Transform enemyTarget;
    protected float timeUntilFire;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        // towerName = tower.towerName;
        // cost = tower.cost;
        range = tower.range;
        rotationSpeed = tower.rotationSpeed;
        bps = tower.bps;
        baseUpgradeCost = tower.baseUpgradeCost;
        isRotatable = tower.isRotatable;

        baseBPS = bps;
        baseRange = range;

        UpdateTowerRangeTransform();
    }

    private void Update()
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

    // Change for each Tower
    protected virtual void Action() { }

    // Change for each Tower
    protected void FindEnemyTarget()
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

    public int CalculateUpgradeCost()
    {
        return Mathf.RoundToInt(baseUpgradeCost * Mathf.Pow(level, 0.8f));
    }

    public float CalculateBPS()
    {
        return baseBPS * Mathf.Pow(level, 0.6f);
    }

    public float CalculateTargetingRange()
    {
        return range * Mathf.Pow(level, 0.4f);
    }

    public void Upgrade()
    {
        if (CalculateUpgradeCost() > LevelManager.main.currency) return;

        LevelManager.main.SpendCurrency(CalculateUpgradeCost());

        level++;
        bps = CalculateBPS();
        range = CalculateTargetingRange();

        // CloseUpgradeUI();
        Debug.Log("New BPS: " + bps);
        Debug.Log("New Range: " + range);
        Debug.Log("New Cost: " + CalculateUpgradeCost());
    }

    private void UpdateTowerRangeTransform()
    {
        // Range (Radius) is to be multiplied by 2 as X, Y and Z are length variables.
        towerRangeTransform.localScale = new Vector3(range * 2f, range * 2f, range * 2f);
    }
}
