using System;
using UnityEngine;
using UnityEngine.Events;

public class BuildManager : MonoBehaviour
{
    public static BuildManager main;

    [SerializeField] public Tower[] towers;

    // [NonSerialized] public bool isBuilding = false;

    [Header("Events")]
    public UnityEvent onStartGroundBuilding = new UnityEvent();
    public UnityEvent onStopGroundBuilding = new UnityEvent();
    public UnityEvent onStartPathBuilding = new UnityEvent();
    public UnityEvent onStopPathBuilding = new UnityEvent();

    private int selectedTower = 0;
    private bool isGroundBuilding = false;
    private bool isPathBuilding = false;

    // For Tower Preview
    [NonSerialized] public bool isTileSelected = false;
    [NonSerialized] public Vector3 selectedTilePosition;
    // Defined here because currently cannot think of any better way to do this
    // And don't want to spam events as this is a small thing
    [NonSerialized] public bool isSelectedTileRestricted;

    private void Awake()
    {
        main = this;
    }

    public Tower GetSelectedTower()
    {
        // return towerPrefabs[selectedTower];
        return towers[selectedTower];
    }

    public float GetSelectedTowerRange()
    {
        return towers[selectedTower].towerSObj.range;
    }

    // public int GetSelectedTowerCost()
    // {
    //     return towers[selectedTower].towerSObj.cost;
    // }

    public bool CanAffordSelectedTower()
    {
        return towers[selectedTower].towerSObj.cost <= LevelManager.main.currency;
    }

    public void BuySelectedTower()
    {
        LevelManager.main.SpendCurrency(towers[selectedTower].towerSObj.cost);
    }

    public void SetSelectedTower(int _selectedTower)
    {
        selectedTower = _selectedTower;
    }

    // public void EnableBuilding()
    // {
    //     // isBuilding = true;
    //     // onStartGroundBuilding.Invoke();
    // }

    public void EnableGroundBuilding()
    {
        onStartGroundBuilding.Invoke();
        isGroundBuilding = true;
    }

    public void EnablePathBuilding()
    {
        onStartPathBuilding.Invoke();
        isPathBuilding = true;
    }

    public void DisableBuilding()
    {
        // isBuilding = false;
        // onStopGroundBuilding.Invoke();
        if (isGroundBuilding)
        {
            DisableGroundBuilding();
        }
        else if (isPathBuilding)
        {
            DisablePathBuilding();
        }
    }

    public void DisableGroundBuilding()
    {
        onStopGroundBuilding.Invoke();
        isGroundBuilding = false;
    }

    public void DisablePathBuilding()
    {
        onStopPathBuilding.Invoke();
        isPathBuilding = false;
    }

    // For Tower Preview
    public void SetSelectedTilePosition(Vector3 pos)
    {
        isTileSelected = true;
        selectedTilePosition = pos;
    }

    public void ClearSelectedTile()
    {
        isTileSelected = false;
    }

    public void SetSelectedTileRestricted(bool _isRestricted)
    {
        isSelectedTileRestricted = _isRestricted;
    }
}
