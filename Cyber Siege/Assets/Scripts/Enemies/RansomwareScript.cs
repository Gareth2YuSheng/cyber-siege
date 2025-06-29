using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class RansomwareScript : BasicEnemyScript
{
    public static RansomwareScript main;

    [Header("References")]
    // For testing
    [SerializeField] protected Transform enemyRangeTransform;

    [SerializeField] protected LayerMask towerMask;
    [SerializeField] protected String HittableTag;


    [Header("Attributes")]
    [SerializeField] private float range;
    // [SerializeField] private float WormSpawnRate;
    //Spawns {botnetSpawnCount} bots every {botnetSpawnRate} seconds

    protected Transform towerTarget;
    public UnityEvent onDisable = new UnityEvent();
    public bool prompted = false;

    protected override void Start()
    {
        base.Start();
        UpdateEnemyRangeTransform();
        // ransomwareAsButton.onClick.AddListener(onClick);
    }

    protected override void Update()
    {
        // Call the parent Update method to prevent loss of movement behavior
        base.Update();

        // If no target, look for one
        if (towerTarget == null)
        {
            FindTowerTarget();
            return;
        }
        //If target goes out of range, reset target
        if (!CheckTargetIsInRange())
        {
            towerTarget = null;
        }
        //Else Disable it.
        else
        {
            // Debug.Log("DISABLE!!!!");
            onDisable.Invoke();
            // Invoke event to disable specific tower.


            // //If tower is rotatable, rotate towards target
            // if (isRotatable) RotateTowardsTarget();
            // //Shoot
            // timeUntilFire += Time.deltaTime;
            // if (timeUntilFire >= (1f / bps))
            // {
            //     Action();
            //     timeUntilFire = 0f;
            // }
        }

    }

    protected bool CheckTargetIsInRange()
    {
        return Vector2.Distance(towerTarget.position, transform.position) <= range;
    }

    private void UpdateEnemyRangeTransform()
    {
        // Range (Radius) is to be multiplied by 2 as X, Y and Z are length variables.
        enemyRangeTransform.localScale = new Vector3(range / transform.localScale.x * 2, range / transform.localScale.y * 2, range / transform.localScale.z * 2);
    }

    protected void FindTowerTarget()
    {
        RaycastHit2D[] hits = Physics2D.CircleCastAll(transform.position, range, (Vector2)transform.position, 0f, towerMask);


        //If there is a target in range
        if (hits.Length > 0)
        {
            // towerTarget = hits[0].transform;
            // Only target non-hidden enemies
            foreach (RaycastHit2D hit in hits)
            {
                if (hit.collider.CompareTag(HittableTag))
                {
                    // Do something with the hit object that has the "Tower" tag
                    towerTarget = hit.transform;
                    isBlocked = false;
                }
                // Check if target is hidden
                // BasicEnemyScript enemyScript = hit.transform.GetComponentInParent<BasicEnemyScript>();
                // if (enemyScript != null && !enemyScript.isHidden)
                // {
                //     // Invoke to un-disguise Trojans. (For select towers)
                //     // Move logic to specific towers
                //     // Debug.Log("HIDDEN FOUND!");
                //     // enemyScript.Reveal();

                //     towerTarget = hit.transform;
                //     return; // Return so only assignes the first one
                // }

            }
        }


    }
    public void onPurchase()
    {
        prompted = true;
    }

    protected virtual void OnMouseDown()
    {
        if (!prompted)
        {
            EnemyManager.main.SetSelectedRansomware(this);
        }
    }

}
