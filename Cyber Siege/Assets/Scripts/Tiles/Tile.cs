using UnityEngine;

public class Tile : MonoBehaviour
{
    [Header("Attributes")]
    [SerializeField] protected Color hoverColor;

    protected SpriteRenderer sr;
    protected Color initialColor;
    protected GameObject currentTower;
    protected BasicTowerScript currentTowerScript;

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

    protected virtual void OnMouseDown()
    {
        // If not in building mode dont do anything
        // if (!isBuilding) return;

        // If we are not in building mode AND tile currently has no tower
        if (!isBuilding && currentTower == null) return;
        // Else if we are in building mode AND tile already has a tower
        if (isBuilding && currentTower != null)
        {
            // Prompt error
            UIManager.main.ShowErrorPrompt("You cannot build on top of a Tower!");
            return;
        }
        // Else If we are in building mode AND tile currently has no tower 
        if (isBuilding && currentTower == null)
        {
            BuildTower();
        }
        // Else if we are not in building mode AND tile already has a tower
        else
        {
            // Set selected Tower to upgrade and Open the Upgrade menu
            BuildManager.main.SetSelectedTowerToUpgrade(currentTowerScript);
        }

    }

    protected virtual void BuildTower()
    {
        Debug.Log($"Build Selected Tower on {gameObject.name}");
        //Build the Selected Tower on this tile
        Tower towerToBuild = BuildManager.main.GetSelectedTower();
        BuildManager.main.BuySelectedTower();
        currentTower = Instantiate(towerToBuild.prefab, transform.position, Quaternion.identity);
        currentTowerScript = currentTower.GetComponent<BasicTowerScript>();
        //Disable building mode
        BuildManager.main.DisableBuilding();
        //Set tile colour back to initialColour
        sr.color = initialColor;
        // Clear selected tile
        BuildManager.main.ClearSelectedTile();
    }

    // For BuildManager to call tile clicking logic
    public void OnTileClickedExternally()
    {
        OnMouseDown();
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
