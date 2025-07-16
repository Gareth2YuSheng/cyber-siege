using DG.Tweening;
using UnityEngine;

public class WarningEffectScript : MonoBehaviour
{
    private SpriteRenderer sr;
    // For fade
    // private float minAlpha = 0.2f;
    // private float maxAlpha = 1f;
    // private bool toggle = false;

    private void Awake()
    {
        sr = gameObject.GetComponent<SpriteRenderer>();
    }

    // private void Start()
    // {
    //     Fade();
    // }

    private void Update()
    {
        // Color color = sr.color;

        // if (toggle)
        // {
        //     // Decrease opacity
        // }

        // sr.color = color;
    }

    // private void Fade()
    // {
    // sr.DOFade(maxAlpha, 0.5)
    // .SetLoops(-1, LoopType.Yoyo)
    // .SetEase(Ease.InOutSine);
    // }
}
