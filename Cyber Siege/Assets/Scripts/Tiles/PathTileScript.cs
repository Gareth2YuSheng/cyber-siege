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
}
