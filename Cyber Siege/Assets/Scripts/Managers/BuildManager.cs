using System;
using UnityEngine;
using UnityEngine.Events;

public class BuildManager : MonoBehaviour
{
    public static BuildManager main;

    [SerializeField] private Tower[] towers;

    // [NonSerialized] public bool isBuilding = false;

    [Header("Events")]
    public UnityEvent onStartBuilding = new UnityEvent();
    public UnityEvent onStopBuilding = new UnityEvent();


    private int selectedTower = 0;

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

    public void EnableBuilding()
    {
        // isBuilding = true;
        onStartBuilding.Invoke();
    }

    public void DisableBuilding()
    {
        // isBuilding = false;
        onStopBuilding.Invoke();
    }
}
