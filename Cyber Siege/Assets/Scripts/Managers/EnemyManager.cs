using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class EnemyManager : MonoBehaviour
{
    public static EnemyManager main;

    [Header("References")]
    [SerializeField] private GameObject[] enemyPrefabs;

    [Header("Attributes")]
    [SerializeField] private int baseEnemyCount = 8;
    [SerializeField] private float enemiesPerSecond = 2f;
    [SerializeField] private float difficultyScalingFactor = 0.75f;
    [SerializeField] private float enemiesPerSecondCap = 15f;

    [Header("Events")]
    public UnityEvent onWaveStart = new UnityEvent();
    public UnityEvent onWaveEnd = new UnityEvent();
    public UnityEvent onLevelEnd = new UnityEvent();
    public UnityEvent onRansomwareClick = new UnityEvent();

    public bool waveOngoing = false;
    private int currentWave = 0;
    private float timeSinceLastSpawn = 0f;
    private int enemiesAlive;
    private int enemiesLeftToSpawn;
    private float eps; //enemies per second
    public RansomwareScript selectedRansomwareScript;
    private int maxWaveCount = 0;

    private void Awake()
    {
        main = this;
    }


    // Update is called once per frame
    private void Update()
    {
        if (!waveOngoing) return;


        timeSinceLastSpawn += Time.deltaTime;

        if (timeSinceLastSpawn >= (1f / eps) && enemiesLeftToSpawn > 0)
        {
            SpawnEnemy();
            timeSinceLastSpawn = 0f;
            enemiesLeftToSpawn--;
            enemiesAlive++;
        }

        //If there are no more enemies alive, to spawn 
        //and we are still in spawning mode, end the wave
        if (enemiesAlive == 0 && enemiesLeftToSpawn == 0 && waveOngoing)
        {
            EndWave();
        }
    }

    public void EnemySpawned(int numOfEnemies)
    {
        enemiesAlive += numOfEnemies;
    }

    public void EnemyDestroyed()
    {
        enemiesAlive--;
    }

    public void StartWave()
    {
        if (maxWaveCount > 0 && currentWave >= maxWaveCount)
        {
            Debug.Log("Max Wave Count Reached");
            Debug.Log(currentWave);
            return;
        }
        currentWave++;
        waveOngoing = true;
        enemiesLeftToSpawn = EnemiesPerWave();
        eps = EnemiesPerSecond();
        onWaveStart.Invoke();
    }

    private void EndWave()
    {
        Debug.Log("Wave Ended");
        waveOngoing = false;
        timeSinceLastSpawn = 0f;
        onWaveEnd.Invoke();
        if (maxWaveCount > 0 && currentWave == maxWaveCount)
        {
            onLevelEnd.Invoke();
            Debug.Log("Level Ended");
        }
    }

    public bool HasLevelEnded()
    {
        return currentWave == maxWaveCount;
    }

    public int GetCurrentWave()
    {
        return currentWave;
    }

    public int GetMaxWaveCount()
    {
        return maxWaveCount;
    }

    public void SetMaxWaveCount(int count)
    {
        maxWaveCount = count;
    }

    private void SpawnEnemy()
    {
        // GameObject prefabToSpawn = enemyPrefabs[0];
        // Randomise the enemies
        int index = Random.Range(0, enemyPrefabs.Length);
        GameObject prefabToSpawn = enemyPrefabs[index];

        Vector3 position = LevelManager.main.startPoint.position;
        position.z = -1; // Force the z-coord so it spawns above the path

        Instantiate(prefabToSpawn, position, Quaternion.identity);
    }

    public void SpawnEnemies(int count, Vector3 position, int pathIndex, GameObject prefab)
    {
        StartCoroutine(SpawnCoroutine(count, position, pathIndex, prefab, 0f));
    }

    public void SpawnEnemies(int count, Vector3 position, int pathIndex, GameObject prefab, float delay)
    {
        StartCoroutine(SpawnCoroutine(count, position, pathIndex, prefab, delay));
    }

    private IEnumerator SpawnCoroutine(int count, Vector3 position, int pathIndex, GameObject prefab, float initialSpawnDelay)
    {
        if (initialSpawnDelay > 0f)
        {
            yield return new WaitForSeconds(initialSpawnDelay);
        }

        for (int i = 0; i < count; i++)
        {
            GameObject enemy = Instantiate(prefab, position, Quaternion.identity);
            BasicEnemyScript script = enemy.GetComponent<BasicEnemyScript>();
            script.UpdatePathIndex(pathIndex);
            enemiesAlive++;
            yield return new WaitForSeconds(0.1f);
        }
    }

    private int EnemiesPerWave()
    {
        return Mathf.RoundToInt(baseEnemyCount *
            Mathf.Pow(currentWave, difficultyScalingFactor));
    }

    private float EnemiesPerSecond()
    {
        return Mathf.Clamp(enemiesPerSecond *
            Mathf.Pow(currentWave, difficultyScalingFactor),
            0f, enemiesPerSecondCap);
    }

    public void SetSelectedRansomware(RansomwareScript _ransomwareScript)
    {
        selectedRansomwareScript = _ransomwareScript;
        // Open tower upgrade menu
        onRansomwareClick.Invoke();
    }


    public RansomwareScript GetRansomwareScript()
    {
        return selectedRansomwareScript;
    }

}
