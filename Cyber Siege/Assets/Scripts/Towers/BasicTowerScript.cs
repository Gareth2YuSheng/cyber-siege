using UnityEngine;
using UnityEditor;

public class BasicTowerScript : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private ScriptableTower tower;
    [SerializeField] private Transform towerRangeTransform;
    [SerializeField] private LayerMask enemyMask;
    [SerializeField] private Transform turretRotationPart;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform firingPoint;

    // [SerializeField] private GameObject upgradeUI;
    // [SerializeField] private Button upgradeButton;

    //Attributes
    private string towerName;
    private int cost;
    private float range; // Radius
    private float rotationSpeed;
    private float bps;
    private int level = 1;
    private bool isRotatable;

    //For Modification (Upgrades)
    private int baseUpgradeCost;
    private float baseBPS;
    private float baseRange;

    //For Shooting
    private Transform enemyTarget;
    private float timeUntilFire;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        towerName = tower.towerName;
        cost = tower.cost;
        range = tower.range;
        rotationSpeed = tower.rotationSpeed;
        bps = tower.bps;
        baseUpgradeCost = tower.baseUpgradeCost;
        isRotatable = tower.isRotatable;

        baseBPS = bps;
        baseRange = range;

        UpdateTowerRangeTransform();
    }

    // Update is called once per frame
    private void Update()
    {
        //If no target, look for one
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
                Shoot();
                timeUntilFire = 0f;
            }
        }
    }

    // Change for each Tower
    private void Shoot()
    {
        GameObject bulletObj = Instantiate(bulletPrefab, firingPoint.position, Quaternion.identity);
        BulletScript bulletScript = bulletObj.GetComponent<BulletScript>();
        bulletScript.SetTarget(enemyTarget);
    }

    // Change for each Tower
    private void FindEnemyTarget()
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

    private bool CheckTargetIsInRange()
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

    private int CalculateUpgradeCost()
    {
        return Mathf.RoundToInt(baseUpgradeCost * Mathf.Pow(level, 0.8f));
    }

    private float CalculateBPS()
    {
        return baseBPS * Mathf.Pow(level, 0.6f);
    }

    private float CalculateTargetingRange()
    {
        return range * Mathf.Pow(level, 0.4f);
    }

    private void Upgrade()
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
        towerRangeTransform.localScale = new Vector3(range * 2, range * 2, range * 2); 
    }

    // private void OnDrawGizmosSelected()
    // {
    //     // To show range only to developer
    //     Handles.color = Color.red;
    //     Handles.DrawWireDisc(transform.position, transform.forward, range);
    // }
}
