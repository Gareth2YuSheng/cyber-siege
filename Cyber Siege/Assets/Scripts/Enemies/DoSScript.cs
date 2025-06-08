using UnityEngine;
using System.Collections;

public class DoSScript : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject DDoSBotPrefab;

    [Header("Attributes")]
    [SerializeField] private float botnetSpawnCount;
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
            StartCoroutine(SpawnBotNet());
            timeSinceLastSpawn = 0f;
        }
    }

    private IEnumerator SpawnBotNet()
    {
        for (int i = 0; i < botnetSpawnCount; i++)
        {
            GameObject bot = Instantiate(DDoSBotPrefab, myTransform.position, Quaternion.identity);
            BasicEnemyScript botScript = bot.GetComponent<BasicEnemyScript>();
            // Update bot's movement target to same as DoS
            botScript.UpdatePathIndex(myBEScript.GetCurrentPathIndex());
            // Update Enemy Manager
            EnemyManager.main.EnemySpawned();
            // Delay before spawning next bot
            yield return new WaitForSeconds(0.1f); // slight delay between each bot
        }
    }
}
