using UnityEngine;

public class GroundTileScript : Tile
{
    protected override void Start()
    {
        base.Start();

        // Add Event Listeners
        BuildManager.main.onStartGroundBuilding.AddListener(StartBuilding);
        BuildManager.main.onStopGroundBuilding.AddListener(StopBuilding);
    }
}
