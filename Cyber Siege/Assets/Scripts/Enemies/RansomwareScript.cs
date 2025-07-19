using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class RansomwareScript : BasicEnemyScript
{
    [Header("References")]
    [SerializeField] protected Transform enemyRangeTransform;
    [SerializeField] protected LayerMask towerMask;

    [Header("Attributes")]
    [SerializeField] private float range;
    [SerializeField] private float cooldown = 5f;

    protected Transform towerTarget;
    public bool hasAttemptedRemoval = false;
    private float timeUntilDisableTowers;

    protected override void Start()
    {
        base.Start();
        UpdateEnemyRangeTransform();
    }

    protected override void Update()
    {
        base.Update();

        timeUntilDisableTowers += Time.deltaTime;
        if (timeUntilDisableTowers >= cooldown)
        {
            ScanAndDisableTowersInRange();
            timeUntilDisableTowers = 0f;
        }
    }

    protected virtual void OnMouseDown()
    {
        if (!hasAttemptedRemoval)
        {
            EnemyManager.main.SetSelectedRansomware(this);
        }
    }

    private void UpdateEnemyRangeTransform()
    {
        // Range (Radius) is to be multiplied by 2 as X, Y and Z are length variables.
        enemyRangeTransform.localScale = new Vector3(range / transform.localScale.x * 2, range / transform.localScale.y * 2, range / transform.localScale.z * 2);
    }

    public void PromptPurchased()
    {
        hasAttemptedRemoval = true;
    }

    private void ScanAndDisableTowersInRange()
    {
        Debug.Log("Ransomware Scanning For Towers");

        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, range, towerMask);
        foreach (Collider2D hit in hits)
        {
            // Hits will be objects with colliders, which are currently the tower Bases
            // Script is in parent object
            BasicTowerScript tower = hit.GetComponentInParent<BasicTowerScript>();
            if (tower != null && tower.towerName != "Encryption Node")
            {
                Debug.Log($"Found tower: {tower.towerName}");
                tower.DisableTower(this);
            }
        }
    }

}
