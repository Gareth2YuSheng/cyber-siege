using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Level1Manager : IndividualLevelManager
{
    [Header("Attributes")]
    [SerializeField] private Sprite virusImage;
    [SerializeField] private Sprite networkScrubberImage;

    protected override IEnumerator StartLevel()
    {
        // Disable start button and tower menu while user reads the prompt
        UIManager.main.DisableTowerMenu();
        UIManager.main.DisableStartWaveButton();
        // Virus Prompt
        UIManager.main.SetLevelPromptContent(
            "New Enemy Detected: Virus",
            "An unstable and aggressive threat that behaves unpredictably when eliminated. <u>Some defenses may only make things worse</u> — choose your countermeasures carefully.\n\n"
            + "Like real-world computer viruses, it's designed to spread and disrupt systems when not properly contained.",
            virusImage);
        yield return WaitForPrompt();
        // Virus Prompt
        UIManager.main.SetLevelPromptContent(
            "New Tower: Network Scrubber",
            "Shoots at enemies in all directions.",
            networkScrubberImage);
        yield return WaitForPrompt();
        // Enable them after user has read the prompt
        UIManager.main.EnableTowerMenu();
        UIManager.main.EnableStartWaveButton();
    }
}
