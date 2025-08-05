using UnityEngine;

public class SuspiciousEmailScript : SuspiciousEnemyScript
{
    [Header("References")]
    [SerializeField] private ScriptableEnemy enemyToSpawnSO;

    [Header("Attributes")]
    [SerializeField] private int onDeathSpawnCount;

    protected override void Start()
    {
        base.Start();
        // Add Event Listenter
        onEnemyDeath.AddListener(SpawnEnemiesOnDeath);
    }

    // Cannot directly spawn the enemies in here as the gameObject 
    // is being destroyed before spawnning the rest of the enemies 
    // Logic moved to EnemyManager
    private void SpawnEnemiesOnDeath(BasicEnemyScript enemy)
    {
        EnemyManager.main.SpawnEnemies(
            onDeathSpawnCount,
            transform.position,
            GetCurrentPathIndex(),
            enemyToSpawnSO.objectPoolTag);
    }

    public override void DestroySelf()
    {
        // Cleanup Event Listener
        onEnemyDeath.RemoveListener(SpawnEnemiesOnDeath);
        base.DestroySelf();
    }
}
