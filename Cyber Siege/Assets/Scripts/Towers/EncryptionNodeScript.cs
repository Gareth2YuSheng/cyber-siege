using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class EncryptionNodeScript : BasicTowerScript
{
    [Header("References")]
    [SerializeField] private LayerMask towerMask;
    [SerializeField] private GameObject cooldownEffect;

    [Header("Attributes")]
    [SerializeField] private float cooldownInterval = 6f;
    [SerializeField] private float stunDuration = 3f;

    private HashSet<BasicTowerScript> towersInRange = new HashSet<BasicTowerScript>();
    private BasicTowerScript protectedTower = null;

    public override void InitialiseTower()
    {
        base.InitialiseTower();
        cooldownEffect.SetActive(false);

        // Do an Initial Scan
        ScanForTowersInRange();
        // Add Event Listener
        BuildManager.main.onTowerBuilt.AddListener(ScanForTowersInRange);
    }

    private void OnDestroy()
    {
        // Remove protection from towersInRange
        foreach (BasicTowerScript tower in towersInRange)
        {
            if (tower != null)
            {
                tower.UnProtectTower();
                tower.ResetEncryptionNode();
                tower.onTowerDestroyed.RemoveListener(HandleTowerDestroyed);
            }
        }
    }

    protected override void Action()
    {
        // Override to prevent unexpeted behavior
    }

    private void UpdateProtectionForTowersInRange()
    {
        foreach (BasicTowerScript tower in towersInRange)
        {
            if (!disabled || tower == protectedTower)
            {
                tower.ProtectTower();
            }
            else
            {
                tower.UnProtectTower();
            }
        }
    }

    private void ScanForTowersInRange()
    {
        Debug.Log("Encryption Node Scanning For New Towers");

        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, range, towerMask);
        foreach (Collider2D hit in hits)
        {
            // Hits will be objects with colliders, which are currently the tower Bases
            // Script is in parent object
            BasicTowerScript tower = hit.GetComponentInParent<BasicTowerScript>();
            // Dont scan for itself
            if (tower != null && tower != this && !towersInRange.Contains(tower))
            {
                Debug.Log($"Found new tower: {tower.towerName}");
                tower.SetEncryptionNode(this);
                tower.onTowerDestroyed.AddListener(HandleTowerDestroyed);
                towersInRange.Add(tower);
            }
        }

        UpdateProtectionForTowersInRange();
    }

    // This is added in the case that towers are sold while being protected
    // to clean up the references in towersInRange
    private void HandleTowerDestroyed(BasicTowerScript tower)
    {
        towersInRange.Remove(tower);
        tower.onTowerDestroyed.RemoveListener(HandleTowerDestroyed);
    }

    public void DisableEncryptionNode(BasicTowerScript tower, BasicEnemyScript enemy)
    {
        if (disabled) return;

        protectedTower = tower;

        // If upgrade 1 was purchased
        if (upgrades[0].purchased)
        {
            // Stun enemy
            if (enemy != null) // In case the enemy was destroyed
            {
                // May need to handle null exception for when it tries to switch sprite back.
                StartCoroutine(enemy.Stun(stunDuration));
            }
        }

        Debug.Log($"Encryption node disabled. Tower {tower.name} remains protected. {cooldownInterval} second start");
        StartCoroutine(Disable());
    }

    protected override IEnumerator Disable()
    {
        disabled = true;
        UpdateProtectionForTowersInRange();
        cooldownEffect.SetActive(true);

        yield return new WaitForSeconds(cooldownInterval);

        Debug.Log("Encryption Node Back Online");
        protectedTower = null;
        disabled = false;
        UpdateProtectionForTowersInRange();
        cooldownEffect.SetActive(false);
    }

    /* Upgrades
        Upgrade 1 - Secondary Verification
        Stun the Ransomware that caused cooldown via tower for 3 seconds.

        Upgrade 2 - Encryption Fortification
        Decreases the cooldown timing from 6 seconds to 3 seconds.
    */

    public override void Upgrade2()
    {
        base.Upgrade2();
        cooldownInterval = 3f;
    }
}
