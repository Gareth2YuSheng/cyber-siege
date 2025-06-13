using UnityEngine;
using System.Collections;

public class TrojanScript : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Sprite revealedSprite;

    private SpriteRenderer spriteRenderer;
    private BasicEnemyScript myBEScript;

    private void Start()
    {
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        myBEScript = gameObject.GetComponent<BasicEnemyScript>();
        myBEScript.onEnemyReveal.AddListener(RevealSelf);
    }

    // Update is called once per frame
    private void RevealSelf()
    {
        // On first hit, change sprite.
        spriteRenderer.sprite = revealedSprite;
    }
}
