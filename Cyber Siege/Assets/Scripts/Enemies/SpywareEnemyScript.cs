using UnityEngine;

public class SpywareEnemyScript : BasicEnemyScript
{
    protected override void Start()
    {
        base.Start();
        Hide();
        Vanish();
    }
}
