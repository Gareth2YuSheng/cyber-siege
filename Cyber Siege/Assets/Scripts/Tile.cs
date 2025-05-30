using System.Runtime.CompilerServices;
using NUnit.Framework;
using UnityEngine;

public class Tile : MonoBehaviour
{
    [Header("Attributes")]
    [SerializeField] private Color hoverColor;

    private SpriteRenderer sr;
    private Color initialColor;
    private GameObject currentTower;

    private void Start()
    {
        sr = gameObject.GetComponentInChildren<SpriteRenderer>();
        initialColor = sr.color;
        sr.enabled = false;
    }

    private void Update()
    {
        //If build mode is enabled, and tiles are hidden, show tiles
        if (BuildManager.main.isBuilding && !sr.enabled)
        {
            sr.enabled = true;
        }
        //Else if build more is disabled, and tiles are shown, hide tiles
        else if (!BuildManager.main.isBuilding && sr.enabled)
        {
            sr.enabled = false;
        }
    }

    private void OnMouseEnter()
    {
        if (BuildManager.main.isBuilding && currentTower == null)
        {
            sr.color = hoverColor;
        }
    }

    private void OnMouseExit()
    {
        if (BuildManager.main.isBuilding && currentTower == null)
        {
            sr.color = initialColor;
        }
    }

    private void OnMouseDown()
    {
        // If in building mode and tile currently has no tower
        if (BuildManager.main.isBuilding && currentTower == null)
        {
            // Debug.Log($"Build Selected Tower on {gameObject.name}");
            //Build the Selected Tower on this tile
            Tower towerToBuild = BuildManager.main.GetSelectedTower();
            BuildManager.main.BuySelectedTower();
            currentTower = Instantiate(towerToBuild.prefab, transform.position, Quaternion.identity);
            //Disable building mode
            BuildManager.main.DisableBuilding();
            //Set tile colour back to initialColour
            sr.color = initialColor;
        }
        // else if (BuildManager.main.isBuilding && currentTower != null)
    }
}
