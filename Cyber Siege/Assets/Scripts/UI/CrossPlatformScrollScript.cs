using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CrossPlatformScrollScript : ScrollRect
{
    [Header("Attributes")]
    // public float macSensitivityMultiplier = 1f;
    // public float windowsSensitivityMultiplier = 5f;

    public float windowsSensMultiplier = 7f;
    public float macSensMultiplier = 1f;
    protected override void Start()
    {
        base.Start();
        horizontal = false;  // disable horizontal scrolling
    }

    public override void OnScroll(PointerEventData data)
    {
        float sensitivityModifier = windowsSensMultiplier;

#if UNITY_STANDALONE_OSX || UNITY_EDITOR_OSX
        sensitivityModifier = macSensMultiplier;
#endif

        // Modify scrollDelta in place before calling base
        data.scrollDelta *= sensitivityModifier;
        base.OnScroll(data);
    }
}
