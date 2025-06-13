using UnityEngine;
using System.Collections;
using System;

public class VirusScript : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject VirusPrefab;

    [Header("Attributes")]
    [SerializeField] private int VirusSpawnCount;
    // [SerializeField] private float VirusSpawnRate;
    //Spawns {botnetSpawnCount} bots every {botnetSpawnRate} seconds

    private Transform myTransform;
    private BasicEnemyScript myBEScript;
    // private float timeSinceLastSpawn = 0f;
    private Vector3 lastPosition;
    private Boolean spawnCheck = false;

    private void Awake()
    {
        // initial value for lastPosition
        lastPosition = transform.position;
    }
    private void Start()
    {
        myTransform = gameObject.GetComponent<Transform>();
        myBEScript = gameObject.GetComponent<BasicEnemyScript>();
        myBEScript.onTakeDamage.AddListener(Spawn);
    }

    // Update is called once per frame
    private void Update()
    {
        Vector3 currentPosition = transform.position;
        // Current position

        // Get Direction

        if (spawnCheck) // <- Add additional check for tower.
        {
            if (currentPosition != lastPosition)
            {

                Vector3 direction = GetMovementDirection(currentPosition, lastPosition);

                if (direction != Vector3.zero)
                {
                    Vector3 spawnPosition = currentPosition - (direction * 1f);
                    EnemyManager.main.SpawnEnemies(
                        VirusSpawnCount,
                        spawnPosition,
                        myBEScript.GetCurrentPathIndex(),
                        VirusPrefab
                    );
                }
                // Modify position
                //myTransform.position
            }
            spawnCheck = false;
        }
        // Get Direction


        // set last to current
        lastPosition = transform.position;
    }

    private Vector3 GetMovementDirection(Vector3 current, Vector3 last)
    {
        Debug.Log(current);
        Debug.Log(last);
        Vector3 movement = current - last;

        // If no movement, return zero vector
        if (movement == Vector3.zero)
            return Vector3.zero;

        // Get primary movement direction (either x or y axis)
        if (Mathf.Abs(movement.x) > Mathf.Abs(movement.y))
        {
            return new Vector3(Mathf.Sign(movement.x), 0, 0);
        }
        else
        {
            return new Vector3(0, Mathf.Sign(movement.y), 0);
        }
    }

    private void Spawn()
    {
        spawnCheck = true;
    }
}


