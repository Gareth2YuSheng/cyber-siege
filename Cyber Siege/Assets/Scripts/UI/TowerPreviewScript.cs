using UnityEngine;

public class TowerPreviewScript : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform previewRangeTransform;
    [SerializeField] private SpriteRenderer previewRangeSR;

    private SpriteRenderer mySR;
    private Color initialSpriteColor;
    private bool isBuilding = false;

    [Header("Attributes")]
    [SerializeField] private Vector3 idlePosition = new Vector3(500, 500, 0);

    // private SpriteRenderer previewRangeSR;
    // private Transform previewRangeTransform;

    private void Start()
    {
        // previewRangeSR = GetComponentInChildren<SpriteRenderer>(); 
        // previewRangeTransform = GetComponentInChildren<Transform>();
        mySR = gameObject.GetComponent<SpriteRenderer>();

        // Add Event Listeners
        BuildManager.main.onStartBuilding.AddListener(StartBuilding);
        BuildManager.main.onStopBuilding.AddListener(StopBuilding);
    }

    private void Update()
    {
        if (!isBuilding) return;

        // If building mode activated, follow the player's mouse
        // Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        // transform.position = mousePosition;

        if (BuildManager.main.isTileSelected)
        {
            transform.position = BuildManager.main.selectedTilePosition;
        }
        else
        {
            transform.position = idlePosition;
        }
        // If selected tile is restricted, turn sprite red
        if (BuildManager.main.isSelectedTileRestricted)
        {
            mySR.color = Color.red;
        }
        else
        {
            mySR.color = initialSpriteColor;
        }
    }

    private void StartBuilding()
    {
        isBuilding = true;
        //Enable the preview range sprite renderer
        previewRangeSR.enabled = true;
        //Set the preview range transform size <---- Fix this
        float rangeSize = BuildManager.main.GetSelectedTowerRange() * 5f;
        previewRangeTransform.localScale = new Vector3(rangeSize, rangeSize, rangeSize);
        //Change the Tower Preview Sprite
        mySR.sprite = BuildManager.main.GetSelectedTower().sprite;
        initialSpriteColor = mySR.color;

    }

    private void StopBuilding()
    {
        isBuilding = false;
        //Move the preview offscreen
        transform.position = idlePosition;
        //Disable the range sprite renderer
        previewRangeSR.enabled = false;
        // Clear the sprite
        mySR.sprite = null;
    }
}
