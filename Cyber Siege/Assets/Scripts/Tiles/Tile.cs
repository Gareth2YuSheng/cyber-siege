using UnityEngine;

public class Tile : MonoBehaviour
{
    [Header("Attributes")]
    [SerializeField] protected Color hoverColor;

    protected SpriteRenderer sr;
    protected Color initialColor;
    protected GameObject currentTower;

    protected bool isBuilding = false;
    protected Vector3 centerPosition;

    protected virtual void Start()
    {
        sr = gameObject.GetComponentInChildren<SpriteRenderer>();
        initialColor = sr.color;
        sr.enabled = false;
        centerPosition = sr.bounds.center;
    }

    protected void OnMouseEnter()
    {
        // If not in building mode dont do anything
        if (!isBuilding) return;
        // Set Selected Tile
        BuildManager.main.SetSelectedTilePosition(centerPosition);
        // If selected tile already contains a tower,
        // set it as restricted and do not change the hover color
        BuildManager.main.SetSelectedTileRestricted(false);
        if (currentTower != null)
        {
            BuildManager.main.SetSelectedTileRestricted(true);
        }
        // change hover colour if there is not tower
        else if (currentTower == null)
        {
            sr.color = hoverColor;
        }
    }

    protected void OnMouseExit()
    {
        // If not in building mode dont do anything
        if (!isBuilding) return;
        // Clear selected tile
        BuildManager.main.ClearSelectedTile();
        // Set selected tile restricted back to normal
        if (currentTower != null)
        {
            // BuildManager.main.SetSelectedTileRestricted(false);
        }
        // change hover colour if there is not tower
        else if (currentTower == null)
        {
            sr.color = initialColor;
        }
    }

    protected void OnMouseDown()
    {
        // If not in building mode dont do anything
        if (!isBuilding) return;
        // If tile currently has no tower
        if (currentTower == null)
        {
            Debug.Log($"Build Selected Tower on {gameObject.name}");
            //Build the Selected Tower on this tile
            Tower towerToBuild = BuildManager.main.GetSelectedTower();
            BuildManager.main.BuySelectedTower();
            currentTower = Instantiate(towerToBuild.prefab, transform.position, Quaternion.identity);
            //Disable building mode
            BuildManager.main.DisableBuilding();
            //Set tile colour back to initialColour
            sr.color = initialColor;
            // Clear selected tile
            BuildManager.main.ClearSelectedTile();
        }
        else
        {
            // Prompt error
            UIManager.main.ShowErrorPrompt("You cannot build ontop of a Tower!");

        }
        // Else if tower exists, upgrade the tower or show error
        // else if (BuildManager.main.isBuilding && currentTower != null)
    }

    protected void StartBuilding()
    {
        isBuilding = true;
        // Show tile
        sr.enabled = true;
    }

    protected void StopBuilding()
    {
        isBuilding = false;
        // Hide tile
        sr.enabled = false;
    }
}
