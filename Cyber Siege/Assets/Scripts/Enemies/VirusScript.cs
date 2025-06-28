using UnityEngine;

public class VirusScript : BasicEnemyScript
{
    [Header("References")]
    [SerializeField] private GameObject spawnEnemyPrefab;

    [Header("Attributes")]
    [SerializeField] private int VirusSpawnCount;
    // [SerializeField] private float VirusSpawnRate;
    //Spawns {botnetSpawnCount} bots every {botnetSpawnRate} seconds

    // This function will be called by normal towers and will trigger virus spawns
    public override void TakeDamage(int damage)
    {
        Spawn();
        base.TakeDamage(damage);
    }

    // This function will be called by specialised virus killing objects
    public void TakeSpecialisedDamage(int damage)
    {
        base.TakeDamage(damage);
    }

    private void Spawn()
    {
        Vector3 currDirection = GetMovementDirection();
        Vector3 offset = new Vector3(0f, 0f, 0f);
        // check if going in y direction
        if (currDirection.x == 0f)
        {
            // Vary the y coord
            offset.y = Random.Range(-0.3f, 0.3f);
        }
        // check if going in x direction
        else if (currDirection.y == 0f)
        {
            // Vary the x coord
            offset.x = Random.Range(-0.3f, 0.3f);
        }
        Vector3 spawnPosition = transform.position + offset;
        EnemyManager.main.SpawnEnemies(
            VirusSpawnCount,
            spawnPosition,
            GetCurrentPathIndex(),
            spawnEnemyPrefab,
            0.5f
        );
    }
}


