using UnityEngine;

public class MockTileScript : Tile
{
    private int moneySpent = 0;
    public bool destroyTowerCalled = false;
    public bool hideTowerRangeCalled = false;

    public void SetMoneySpent(int amount)
    {
        moneySpent = amount;
    }

    public override int CalculateMoneySpentOnTile()
    {
        return moneySpent;
    }

    public override void DestroyTower()
    {
        destroyTowerCalled = true;
    }
}
