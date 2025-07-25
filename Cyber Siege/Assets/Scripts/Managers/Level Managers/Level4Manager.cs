using System.Collections;
using UnityEngine;

public class Level4Manager : LevelManager
{
    [Header("Attributes")]
    [SerializeField] private Sprite enemyImage;
    [SerializeField] private Sprite towerImage1;

    /*
        Map Design Idea:
        Level 4, the SUS level.
        Starting with 71 currency is sufficient as it forces player to get Threat Intelligence tower.
        This also allows sell and get back to 50 to allow for another tower to be bought in case they purchased Threat Intelligence tower by accident.
        Difficulty scale x1.3.
    */
    protected override IEnumerator StartLevel()
    {
        // Disable start button and tower menu while user reads the prompt
        DisableUIs();
        // Sus Email Prompt
        yield return ShowPrompt(
            "New Enemy Detected: Suspicious Email",
            "Sus! Suspicious Emails use spoofing to look like they’re from someone you trust.\n\nOne click can let in viruses or steal information.\n\nThis hidden enemy drops malware when destroyed — don’t fall for the bait!",
            enemyImage);
        // Threat Intelligence Prompt
        yield return ShowPrompt(
            "New Tower: Threat Intelligence",
            "This tower targets hidden enemies like the Suspicious Email, it can target other enemies but not viruses.\n\nYou never know when you need em'!",
            towerImage1);
        // Enable them after user has read the prompt
        EnableUIs();
    }
}
