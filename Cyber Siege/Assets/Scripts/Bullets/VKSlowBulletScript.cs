using UnityEngine;

public class VKSlowBulletScript : VKBulletScript
{
    private float slowingFactor;
    private float slowingDuration;
    protected override void SpecialEffect(VirusScript virus)
    {
        base.SpecialEffect(virus);
        // Add slowing Effect
        StartCoroutine(virus.UpdateMovementSpeed(
            virus.GetBaseSpeed() * (1f - slowingFactor),
            slowingDuration
        ));
    }

    public void SetSlowingFactor(float factor)
    {
        slowingFactor = factor;
    }

    public void SetSlowingDuration(float duration)
    {
        slowingDuration = duration;
    }
}
