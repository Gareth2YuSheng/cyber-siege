using UnityEngine;

public class TrojanScript : BasicEnemyScript
{
    [Header("References")]
    [SerializeField] private Sprite revealedSprite;
    [SerializeField] private AudioClip audioClipRevealed;

    private SpriteRenderer spriteRenderer;

    protected override void Start()
    {
        base.Start();
        Hide();
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        onEnemyReveal.AddListener(RevealSelf);
    }

    // Update is called once per frame
    private void RevealSelf()
    {
        // On first hit, change sprite.
        spriteRenderer.sprite = revealedSprite;
        SoundManager.main.PlaySoundFXClip(audioClipRevealed, 1f);
    }
}
