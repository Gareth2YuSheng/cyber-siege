using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CrossPlatformScrollScript : ScrollRect
{
    [Header("Attributes")]
    [SerializeField] private float macSensitivityMultiplier = 1f;
    [SerializeField] private float windowsSensitivityMultiplier = 5f;

    public override void OnScroll(PointerEventData data)
    {
        float sensitivityModifier = windowsSensitivityMultiplier;

#if UNITY_STANDALONE_OSX || UNITY_EDITOR_OSX
        sensitivityModifier = macSensitivityMultiplier;
#endif

        // Modify scrollDelta in place before calling base
        data.scrollDelta *= sensitivityModifier;

        base.OnScroll(data);
    }
}
