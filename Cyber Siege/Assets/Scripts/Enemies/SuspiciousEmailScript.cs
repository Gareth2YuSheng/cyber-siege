using UnityEngine;

public class SuspiciousEmailScript : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject[] enemyPrefabs;

    [Header("Attributes")]
    [SerializeField] private int onDeathSpawnCount;

    private Transform myTransform;
    private BasicEnemyScript myBEScript;

    private void Start()
    {
        myTransform = gameObject.GetComponent<Transform>();
        myBEScript = gameObject.GetComponent<BasicEnemyScript>();
        // Add Event Listenter
        gameObject.GetComponent<BasicEnemyScript>().onEnemyDeath.AddListener(SpawnEnemiesOnDeath);
    }

    private void Update()
    {

    }

    // Cannot directly spawn the enemies in here as the gameObject 
    // is being destroyed before spawnning the rest of the enemies 
    // Logic moved to EnemyManager
    private void SpawnEnemiesOnDeath()
    {
        GameObject prefabToSpawn = enemyPrefabs[0];
        // int index = Random.Range(0, enemyPrefabs.Length);
        // GameObject prefabToSpawn = enemyPrefabs[index];
        EnemyManager.main.SpawnEnemies(onDeathSpawnCount, myTransform.position,
            myBEScript.GetCurrentPathIndex(), prefabToSpawn);
    }
}
