using UnityEngine;

public class BasicPathTowerScript : BasicTowerScript
{
    private Tile myTile;

    protected override void Start()
    {
        base.Start();
    }

    public void SetMyTile(Tile tile)
    {
        myTile = tile;
    }

    protected virtual void OnMouseDown()
    {
        if (myTile != null)
        {
            myTile.OnTileClickedExternally();
        }
    }
}
