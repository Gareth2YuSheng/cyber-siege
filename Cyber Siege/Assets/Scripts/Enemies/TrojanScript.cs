using UnityEngine;

public class TrojanScript : BasicEnemyScript
{
    [Header("References")]
    [SerializeField] private Sprite revealedSprite;
    [SerializeField] private Sprite stunnedRevealedSprite;
    [SerializeField] private AudioClip audioClipRevealed;

    protected override void Start()
    {
        base.Start();
        Hide();
        onEnemyReveal.AddListener(RevealSelf);
    }

    // Update is called once per frame
    private void RevealSelf()
    {
        // On first hit, change sprite.
        sr.sprite = revealedSprite;
        SoundManager.main.PlaySoundFXClip(audioClipRevealed, 1f);
    }

    protected override void ToggleStunnedSprite(bool stun)
    {
        if (stunnedSprite != null)
        {
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
}
