using UnityEngine;

public class MockServerScript : ServerScript
{
    public bool healthySpriteCalled = false;
    public bool damagedSpriteCalled = false;

    public override void UpdateHealthySprite()
    {
        healthySpriteCalled = true;
    }

    public override void UpdateDamagedSprite()
    {
        damagedSpriteCalled = true;
    }

    public void ResetFlags()
    {
        healthySpriteCalled = false;
        damagedSpriteCalled = false;
    }
}