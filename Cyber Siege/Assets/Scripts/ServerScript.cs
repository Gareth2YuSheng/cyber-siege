using UnityEngine;

public class ServerScript : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Sprite damagedServerSprite;

    private SpriteRenderer sr;
    private Sprite baseSprite;

    private void Awake()
    {
        sr = gameObject.GetComponent<SpriteRenderer>();
        baseSprite = sr.sprite;
    }

    public virtual void UpdateHealthySprite()
    {
        if (sr.sprite != baseSprite)
        {
            sr.sprite = baseSprite;
        }
    }

    public virtual void UpdateDamagedSprite()
    {
        if (sr.sprite != damagedServerSprite)
        {
            sr.sprite = damagedServerSprite;
        }
    }
}
