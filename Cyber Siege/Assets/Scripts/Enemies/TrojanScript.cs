using UnityEngine;
using System.Collections;

public class TrojanScript : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Sprite DisguiseRevealedSprite;

    [Header("Attributes")]
    // [SerializeField] private int WormSpawnCount;
    // [SerializeField] private float WormSpawnRate;
    //Spawns {botnetSpawnCount} bots every {botnetSpawnRate} seconds
    private SpriteRenderer spriteRenderer;

    private Transform myTransform;
    private BasicEnemyScript myBEScript;
    private float timeSinceLastSpawn = 0f;

    private void Awake()
    {
        // Get reference of SpriteRenderer
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    private void Start()
    {
        myBEScript = gameObject.GetComponent<BasicEnemyScript>();
        myBEScript.onEnemyReveal.AddListener(RevealSelf);
    }

    // Update is called once per frame
    private void RevealSelf()
    {
        // On first hit, change sprite.
        spriteRenderer.sprite = DisguiseRevealedSprite;
    }
}
