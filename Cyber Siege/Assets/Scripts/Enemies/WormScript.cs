using UnityEngine;

public class WormScript : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject WormPrefab;

    [Header("Attributes")]
    [SerializeField] private int WormSpawnCount;
    [SerializeField] private float WormSpawnRate;
    //Spawns {botnetSpawnCount} bots every {botnetSpawnRate} seconds

    private Transform myTransform;
    private BasicEnemyScript myBEScript;
    private float timeSinceLastSpawn = 0f;
    // private Vector3 lastPosition;


    private void Awake()
    {
        // initial value for lastPosition
        // lastPosition = transform.position;
    }
    private void Start()
    {
        myTransform = gameObject.GetComponent<Transform>();
        myBEScript = gameObject.GetComponent<BasicEnemyScript>();
    }

    // Update is called once per frame
    private void Update()
    {

        timeSinceLastSpawn += Time.deltaTime;

        if (timeSinceLastSpawn >= WormSpawnRate)
        {
            Duplicate();
            timeSinceLastSpawn = 0f;
        }

        // Vector3 currentPosition = transform.position;
        // // Current position
        // timeSinceLastSpawn += Time.deltaTime;
        // if (timeSinceLastSpawn >= WormSpawnRate)
        // {
        //     // Get Direction
        //     if (currentPosition != lastPosition)
        //     {

        //         Vector3 direction = GetMovementDirection(currentPosition, lastPosition);

        //         if (direction != Vector3.zero)
        //         {
        //             Vector3 spawnPosition = currentPosition - (direction * 1f);
        //             EnemyManager.main.SpawnEnemies(
        //                 WormSpawnCount,
        //                 spawnPosition,
        //                 myBEScript.GetCurrentPathIndex(),
        //                 WormPrefab
        //             );
        //         }
        //         // Modify position
        //         //myTransform.position
        //         timeSinceLastSpawn = 0f;
        //     }
        // }
        // // set last to current
        // lastPosition = transform.position;
    }

    private void Duplicate()
    {
        // Check Current Direction
        Vector3 currDirection = myBEScript.GetMovementDirection();
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
            myBEScript.GetCurrentPathIndex(),
            WormPrefab,
            0.5f
        );
    }

    // private Vector3 GetMovementDirection(Vector3 current, Vector3 last)
    // {
    //     Debug.Log(current);
    //     Debug.Log(last);
    //     Vector3 movement = current - last;

    //     // If no movement, return zero vector
    //     if (movement == Vector3.zero)
    //         return Vector3.zero;

    //     // Get primary movement direction (either x or y axis)
    //     if (Mathf.Abs(movement.x) > Mathf.Abs(movement.y))
    //     {
    //         return new Vector3(Mathf.Sign(movement.x), 0, 0);
    //     }
    //     else
    //     {
    //         return new Vector3(0, Mathf.Sign(movement.y), 0);
    //     }
    // }


}
