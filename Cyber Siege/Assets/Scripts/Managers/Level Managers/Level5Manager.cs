using System.Collections;
using UnityEngine;

public class Level5Manager : LevelManager
{
    [Header("Attributes")]
    [SerializeField] private Sprite enemyImage1;
    [SerializeField] private Sprite enemyImage2;

    [SerializeField] private Sprite towerImage1;
    protected override IEnumerator StartLevel()
    {
        // Disable start button and tower menu while user reads the prompt
        DisableUIs();
        // Suspicious Call Prompt
        yield return ShowPrompt(
            "New Enemy Detected: Suspicious Call",
            "Another sus enemy! Suspicious Calls happen when people send fake calls to trick you into giving away personal information like passwords, usernames, or credit card numbers. They might pretend to be your bank or a shipping company to make you believe the call is real!",
            enemyImage1);
        // Suspicious Text Prompt
        yield return ShowPrompt(
            "New Enemy Detected: Suspicious Text Message",
            "Suspicious Text Messages come in the form of Smishing, where bad people send fake text messages to trick you into giving away personal information like passwords, usernames, or credit card numbers. They might pretend to be your bank or a shipping company to make you believe the message is real.",
            enemyImage2);
        // IDS Prompt
        yield return ShowPrompt(
            "New Tower: IDS",
            "Intruder Detection System! This tower helps uncover suspicious enemies and allows them to be targetted!",
            towerImage1);
        // Enable them after user has read the prompt
        EnableUIs();
    }
}
