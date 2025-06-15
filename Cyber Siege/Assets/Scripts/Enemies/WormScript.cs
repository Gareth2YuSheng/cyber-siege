using UnityEngine;

public class WormScript : BasicEnemyScript
{
    [Header("References")]
    [SerializeField] private GameObject WormPrefab;

    [Header("Attributes")]
    [SerializeField] private int WormSpawnCount;
    [SerializeField] private float WormSpawnRate;
    //Spawns {botnetSpawnCount} bots every {botnetSpawnRate} seconds

    private float timeSinceLastSpawn = 0f;

    protected override void Update()
    {
        // Call the parent Update method to prevent loss of movement behavior
        base.Update();
        // Worm Specific behavior
        timeSinceLastSpawn += Time.deltaTime;

        if (timeSinceLastSpawn >= WormSpawnRate)
        {
            Duplicate();
            timeSinceLastSpawn = 0f;
        }
    }

    private void Duplicate()
    {
        // Check Current Direction
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
            WormSpawnCount,
            spawnPosition,
            GetCurrentPathIndex(),
            WormPrefab,
            0.5f
        );
    }
}
