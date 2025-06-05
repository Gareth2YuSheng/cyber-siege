using UnityEngine;

public class TowerPreviewScript : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform previewRangeTransform;
    [SerializeField] private SpriteRenderer previewRangeSR;

    private bool isBuilding = false;

    [Header("Attributes")]
    [SerializeField] private Vector3 idlePosition = new Vector3(500, 500, 0);

    // private SpriteRenderer previewRangeSR;
    // private Transform previewRangeTransform;

    private void Start()
    {
        // previewRangeSR = GetComponentInChildren<SpriteRenderer>(); 
        // previewRangeTransform = GetComponentInChildren<Transform>();

        // Add Event Listeners
        BuildManager.main.onStartBuilding.AddListener(StartBuilding);
        BuildManager.main.onStopBuilding.AddListener(StopBuilding);
    }

    private void Update()
    {
        if (!isBuilding) return;

        //If building mode activated, follow the player's mouse
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        transform.position = mousePosition;
    }

    private void StartBuilding()
    {
        isBuilding = true;
        //Enable the preview range sprite renderer
        previewRangeSR.enabled = true;
        //Set the preview range transform size <---- Fix this
        float rangeSize = BuildManager.main.GetSelectedTowerRange() * 5f;
        previewRangeTransform.localScale = new Vector3(rangeSize, rangeSize, rangeSize);
    }

    private void StopBuilding()
    {
        isBuilding = false;
        //Move the preview offscreen
        transform.position = idlePosition;
        //Disable the range sprite renderer
        previewRangeSR.enabled = false;
    }
}
