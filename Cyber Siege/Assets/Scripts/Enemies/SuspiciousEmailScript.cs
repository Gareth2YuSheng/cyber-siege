using UnityEngine;

public class SuspiciousEmailScript : SuspiciousEnemyScript
{
    [Header("References")]
    [SerializeField] private GameObject[] enemyPrefabs;

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
        GameObject prefabToSpawn = enemyPrefabs[0];
        // int index = Random.Range(0, enemyPrefabs.Length);
        // GameObject prefabToSpawn = enemyPrefabs[index];
        EnemyManager.main.SpawnEnemies(
            onDeathSpawnCount,
            transform.position,
            GetCurrentPathIndex(),
            prefabToSpawn);
    }
}
