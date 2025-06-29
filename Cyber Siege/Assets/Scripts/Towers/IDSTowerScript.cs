using UnityEngine;

public class IDSTowerScript : BasicTowerScript
{
    [Header("Attributes")]
    // [SerializeField] private float slowingFactor = 0.4f;
    [SerializeField] private float bonusDamageMultiplier = 1.5f;

    private float upgrade2Duration = 2f;

    protected override void Update()
    {
        // Ransomware handling
        FindRansomwareScript();
        if (!disabled && EnemyManager.main.waveOngoing)
        {
            if (upgrades[0].purchased)
            {
                Action();
            }
            else
            {
                timeUntilFire += Time.deltaTime;
                if (timeUntilFire >= (1f / bps))
                {
                    Action();
                    timeUntilFire = 0f;
                }
            }
        }
    }

    // Reveal All hidden enemies in range
    protected override void Action()
    {
        Debug.Log("Scanning");
        RaycastHit2D[] hits = Physics2D.CircleCastAll(transform.position, range, (Vector2)transform.position, 0f, enemyMask);

        //If there is a target in range
        if (hits.Length > 0)
        {
            foreach (RaycastHit2D hit in hits)
            {
                // Check if target is hidden
                BasicEnemyScript enemy = hit.transform.GetComponentInParent<BasicEnemyScript>();
                if (enemy != null && enemy.isHidden)
                {
                    // Reveal hidden enemy
                    enemy.Reveal();
                    if (upgrades[1].purchased)
                    {
                        // Slow the enemy for 2s
                        // StartCoroutine(enemy.UpdateMovementSpeed(1f - slowingFactor, upgrade2Duration));
                        // Stun the enemy for duration
                        StartCoroutine(enemy.Stun(upgrade2Duration));
                        // Apply Taken Damage Increase for duration
                        StartCoroutine(enemy.SetTakenDamageMultiplier(bonusDamageMultiplier, upgrade2Duration));
                    }
                }
            }
        }
    }

    /*Upgrades
        Upgrade 1 - Real Time Protection
        Removes Cooldown from scans

        Upgrade 2 - Targeted Containment
        Stuns and increase damage taken by 50% upon revealing an enemy for 2s
    */
}
