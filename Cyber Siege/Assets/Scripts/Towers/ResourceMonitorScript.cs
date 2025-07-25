using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class ResourceMonitorScript : BasicTowerScript
{
    [Header("References")]
    [SerializeField] private GameObject warningEffect;

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
        // Upgrade 1
        if (upgrades[0].purchased)
        {
            // Show warning alert if spyware is active
            if (ServerManager.main.HasSpywareAttached() && !warningEffect.activeSelf)
            {
                warningEffect.SetActive(true);
            }
            // Hide warning alert if no spyware is active
            else if (!ServerManager.main.HasSpywareAttached() && warningEffect.activeSelf)
            {
                warningEffect.SetActive(false);
            }
        }
    }

    private void OnDestroy()
    {
        // If tower is sold, remove the multipler stack
        if (upgrades[1].purchased)
        {
            CurrencyManager.main.DecreaseRMMultiplierStacks(1);
        }
    }

    protected override void OnMouseDown()
    {
        base.OnMouseDown();
        Debug.Log("Resource Monitor OnMouseDown");

        // Make sure not in building mode
        if (!BuildManager.main.isBuilding())
        {
            Debug.Log("Opening RM Prompt");
            // Open Prompt
            // UIManager.main.ShowResourceMonitorPrompt(this);
            // Delay 1 frame to make sure upgrade menu opens first and no UI overlap
            StartCoroutine(ShowRMPromptWithDelay());
        }
    }

    private IEnumerator ShowRMPromptWithDelay()
    {
        yield return null; // Waits 1 frame before opening RMPrompt
        if (!BuildManager.main.isBuilding())
        {
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
    public override void Upgrade2()
    {
        base.Upgrade2();
        CurrencyManager.main.IncreaseRMMultiplierStacks(1);
    }
}
