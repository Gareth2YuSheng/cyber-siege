using System.Collections;
using UnityEngine;

public class Level4Manager : LevelManager
{
    [Header("Attributes")]
    [SerializeField] private Sprite enemyImage;
    [SerializeField] private Sprite towerImage1;
    protected override IEnumerator StartLevel()
    {
        // Disable start button and tower menu while user reads the prompt
        DisableUIs();
        // Sus Email Prompt
        yield return ShowPrompt(
            "New Enemy Detected: Suspicious Email",
            "Sus! Email spoofing is a result of Suspicious Emails, and happens when a bad person sends an email that looks like it's from someone you trust, like a business or friend. Because the email looks real, you might open it and click on a dangerous link or download a harmful attachment. This can trick people into giving away sensitive information or letting in a virus.",
            enemyImage);
        // Threat Intelligence Prompt
        yield return ShowPrompt(
            "New Tower: Threat Intelligence",
            "This tower targets hidden enemies like the Suspicious Email! You never know when you need em'!",
            towerImage1);
        // Enable them after user has read the prompt
        EnableUIs();
    }
}
