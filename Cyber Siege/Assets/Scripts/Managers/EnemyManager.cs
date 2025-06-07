using System;
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
    // [SerializeField] private float timeBetweenWaves = 5f;
    [SerializeField] private float difficultyScalingFactor = 0.75f;
    [SerializeField] private float enemiesPerSecondCap = 15f;

    [Header("Events")]
    // public UnityEvent onWaveStart = new UnityEvent();
    public UnityEvent onWaveEnd = new UnityEvent();

    private bool waveOngoing = false;
    public int currentWave = 1; // Public to allow access
    private float timeSinceLastSpawn;
    private int enemiesAlive;
    private int enemiesLeftToSpawn;
    private float eps; //enemies per second

    private void Awake()
    {
        main = this;
    }

    private void Start()
    {
        // StartCoroutine(StartWave());
        //Start wave from start wave button instead
        onWaveEnd.Invoke();
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

    public void EnemyDestroyed()
    {
        enemiesAlive--;
    }

    public void StartWave()
    {
        waveOngoing = true;
        enemiesLeftToSpawn = EnemiesPerWave();
        eps = EnemiesPerSecond();
    }

    private void EndWave()
    {
        waveOngoing = false;
        timeSinceLastSpawn = 0f;
        currentWave++;
        onWaveEnd.Invoke();
    }

    private void SpawnEnemy()
    {
        GameObject prefabToSpawn = enemyPrefabs[0];
        // int index = Random.Range(0, enemyPrefabs.Length);
        // GameObject prefabToSpawn = enemyPrefabs[index];
        Instantiate(prefabToSpawn, LevelManager.main.startPoint.position, Quaternion.identity);
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
}
