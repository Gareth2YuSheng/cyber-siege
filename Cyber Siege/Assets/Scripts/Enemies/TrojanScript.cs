using UnityEngine;

public class TrojanScript : BasicEnemyScript
{
    [Header("References")]
    [SerializeField] private Sprite revealedSprite;
    [SerializeField] private Sprite stunnedRevealedSprite;
    [SerializeField] private AudioClip audioClipRevealed;

    public override void InitialiseEnemy()
    {
        base.InitialiseEnemy();
        Hide();
    }

    protected override void Start()
    {
        base.Start();
        // Hide();
        onEnemyReveal.AddListener(RevealSelf);
    }

    // Update is called once per frame
    private void RevealSelf()
    {
        // On first hit, change sprite.
        sr.sprite = revealedSprite;
        if (audioClipRevealed != null && SoundManager.main != null)
        {
            SoundManager.main.PlaySoundFXClip(audioClipRevealed, 1f);
        }
    }

    protected override void ToggleStunnedSprite(bool stun)
    {
        if (stunnedSprite != null)
        {
            if (isSlowed && slowEffect != null) slowEffect.SetActive(!stun);
            if (isHidden)
            {
                sr.sprite = stun ? stunnedSprite : baseSprite;
                stunEffect.SetActive(stun);
            }
            else
            {
                sr.sprite = stun ? stunnedRevealedSprite : revealedSprite;
                stunEffect.SetActive(stun);
            }
        }
    }

    public override void DestroySelf()
    {
        // Cleanup Event Listener
        onEnemyReveal.RemoveListener(RevealSelf);
        base.DestroySelf();
    }
}
