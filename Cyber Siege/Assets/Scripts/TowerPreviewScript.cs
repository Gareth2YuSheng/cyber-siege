using UnityEngine;

public class TowerPreviewScript : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform previewRangeTransform;
    [SerializeField] private SpriteRenderer previewRangeSR;

    [Header("Attributes")]
    [SerializeField] private Vector3 idlePosition = new Vector3(500, 500, 0);

    // private SpriteRenderer previewRangeSR;
    // private Transform previewRangeTransform;
    private bool wasBuilding = false; // Caching prev bool value for effciency

    private void Start()
    {
        // previewRangeSR = GetComponentInChildren<SpriteRenderer>(); 
        // previewRangeTransform = GetComponentInChildren<Transform>();
    }

    private void Update()
    {
        bool isBuilding = BuildManager.main.isBuilding;
        // if state just became true
        if (isBuilding && !wasBuilding)
        {
            //Enable the preview range sprite renderer
            previewRangeSR.enabled = true;
            //Set the preview range transform size
            float rangeSize = BuildManager.main.GetSelectedTowerRange() * 5f;
            previewRangeTransform.localScale = new Vector3(rangeSize, rangeSize, rangeSize);
        }

        // if state just became false
        if (!isBuilding && wasBuilding)
        {
            //Move the preview offscreen
            transform.position = idlePosition;
            //Disable the range sprite renderer
            previewRangeSR.enabled = false;
        }

        // if building mode is activated, follow the player's mouse
        if (isBuilding)
        {
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            transform.position = mousePosition;
        }

        wasBuilding = isBuilding;
    }
}
