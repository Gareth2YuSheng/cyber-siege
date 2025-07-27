using UnityEngine;

public class MockSpywareEnemyScript : SpywareEnemyScript
{
    public bool destroyCalled = false;
    public string testEnemyName;

    public override string GetEnemyName()
    {
        return testEnemyName;
    }

    public void SetEnemyName(string _name)
    {
        testEnemyName = _name;
    }

    public override void DestroySelf()
    {
        destroyCalled = true;  // Flag to check if purge works
    }
}
