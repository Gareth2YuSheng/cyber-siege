using UnityEngine;

public class DoSScript : BasicEnemyScript
{
    [Header("References")]
    [SerializeField] private GameObject DDoSBotPrefab;

    [Header("Attributes")]
    [SerializeField] private int botnetSpawnCount;
    [SerializeField] private float botnetSpawnRate;
    //Spawns {botnetSpawnCount} bots every {botnetSpawnRate} seconds

    private float timeSinceLastSpawn = 0f;

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        // DoS Specific behavior
        timeSinceLastSpawn += Time.deltaTime;

        if (timeSinceLastSpawn >= botnetSpawnRate)
        {
            EnemyManager.main.SpawnEnemies(
                botnetSpawnCount,
                transform.position,
                GetCurrentPathIndex(),
                DDoSBotPrefab);
            timeSinceLastSpawn = 0f;
        }
    }
}
