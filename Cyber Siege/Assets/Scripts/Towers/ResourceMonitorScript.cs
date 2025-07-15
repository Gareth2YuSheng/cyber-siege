using UnityEngine;
using UnityEngine.Events;

public class ResourceMonitorScript : BasicTowerScript
{
    [Header("Attributes")]
    [SerializeField] private float cleanupCooldownDuration = 15f;

    [Header("Events")]
    public UnityEvent<int> onCooldownSecondChagned = new UnityEvent<int>();

    private float timeUntilCleanupReady = 15f; //start with ability ready
    private int lastSecondRecorded = -1;

    protected override void Update()
    {
        base.Update();
        // Manage cleanup cooldown
        if (timeUntilCleanupReady < cleanupCooldownDuration)
        {
            timeUntilCleanupReady += Time.deltaTime;
            if (timeUntilCleanupReady > cleanupCooldownDuration)
            {
                timeUntilCleanupReady = cleanupCooldownDuration;
            }
            // Update Cooldown Timer
            int secondsLeft = Mathf.CeilToInt(cleanupCooldownDuration - timeUntilCleanupReady);
            if (secondsLeft != lastSecondRecorded)
            {
                lastSecondRecorded = secondsLeft;
                onCooldownSecondChagned.Invoke(secondsLeft);
            }
        }
    }

    protected override void OnMouseDown()
    {
        base.OnMouseDown();
        // Make sure not in building mode
        if (!BuildManager.main.isBuilding())
        {
            // Open Prompt
            UIManager.main.ShowResourceMonitorPrompt(this);
        }
    }

    public void CleanupServer()
    {
        // Check whether cleanup ability is on cooldown
        if (timeUntilCleanupReady < cleanupCooldownDuration)
        {
            Debug.Log("Resouce Monitor Cleanup is on Cooldown!");
            return;
        }
        Debug.Log("Cleaning Server");

        // Find and Destroy a spyware enemy (e.g. Cryptojacking)
        ServerManager.main.PurgeFirstSpyware();
        timeUntilCleanupReady = 0f;
        lastSecondRecorded = -1;
    }

    public int GetCoolDown()
    {
        return Mathf.CeilToInt(cleanupCooldownDuration - timeUntilCleanupReady);
    }

    /* Upgrades:
        Upgrade 1 - Background Checker
        Detects when Spyware is active and warns player

        Upgrade 2 - Cost Auditor
        Increases resource gain per kill if no spyware is present.
    */
}
