using UnityEngine;

public class MockBasicEnemyScript : BasicEnemyScript
{
    public bool isDestroyedFlag = false;

    // Override this because Destroy cannot be called in EditorMode Tests
    public override void DestroySelf()
    {
        isDestroyedFlag = true;
    }
}
