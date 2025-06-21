using UnityEngine;

public class PathTileScript : Tile
{
    protected override void Start()
    {
        base.Start();

        // Add Event Listeners
        BuildManager.main.onStartPathBuilding.AddListener(StartBuilding);
        BuildManager.main.onStopPathBuilding.AddListener(StopBuilding);
    }

    protected override void BuildTower()
    {
        base.BuildTower();
        BasicPathTowerScript script = currentTower.GetComponent<BasicPathTowerScript>();
        if (script != null) script.SetMyTile(this);
    }
}
