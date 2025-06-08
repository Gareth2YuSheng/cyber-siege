using UnityEngine;
using System.Collections;

public class DoSScript : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject DDoSBotPrefab;

    [Header("Attributes")]
    [SerializeField] private int botnetSpawnCount;
    [SerializeField] private float botnetSpawnRate;
    //Spawns {botnetSpawnCount} bots every {botnetSpawnRate} seconds

    private Transform myTransform;
    private BasicEnemyScript myBEScript;
    private float timeSinceLastSpawn = 0f;

    private void Start()
    {
        myTransform = gameObject.GetComponent<Transform>();
        myBEScript = gameObject.GetComponent<BasicEnemyScript>();
    }

    // Update is called once per frame
    private void Update()
    {
        timeSinceLastSpawn += Time.deltaTime;

        if (timeSinceLastSpawn >= botnetSpawnRate)
        {
            // StartCoroutine(SpawnBotNet());
            EnemyManager.main.SpawnEnemies(botnetSpawnCount, myTransform.position,
                myBEScript.GetCurrentPathIndex(), DDoSBotPrefab);
            timeSinceLastSpawn = 0f;
        }
    }
}
