using UnityEngine;

public class ServerScript : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Sprite damagedServerSprite;

    private SpriteRenderer sr;
    private Sprite baseSprite;

    private void Start()
    {
        sr = gameObject.GetComponent<SpriteRenderer>();
        baseSprite = sr.sprite;
    }

    public void UpdateHealthySprite()
    {
        if (sr.sprite != baseSprite)
        {
            sr.sprite = baseSprite;
        }
    }

    public void UpdateDamagedSprite()
    {
        if (sr.sprite != damagedServerSprite)
        {
            sr.sprite = damagedServerSprite;
        }
    }
}
