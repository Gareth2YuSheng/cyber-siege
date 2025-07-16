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
        UIManager.main.DisableTowerMenu();
        UIManager.main.DisableStartWaveButton();
        // Virus Prompt
        UIManager.main.SetLevelPromptContent(
            "New Enemy Detected: Suspicious Call",
            "Another sus enemy! Suspicious Calls happen when people send fake calls to trick you into giving away personal information like passwords, usernames, or credit card numbers. They might pretend to be your bank or a shipping company to make you believe the call is real!",
            enemyImage1);
        yield return WaitForPrompt();
        UIManager.main.SetLevelPromptContent(
            "New Enemy Detected: Suspicious Text Message",
            "Sussy baka. Suspicious Text Messages come in the form of Smishing, where bad people send fake text messages to trick you into giving away personal information like passwords, usernames, or credit card numbers. They might pretend to be your bank or a shipping company to make you believe the message is real.",
            enemyImage2);
        yield return WaitForPrompt();
        // Virus Prompt
        UIManager.main.SetLevelPromptContent(
            "New Tower: IDS",
            "Intruder Detection System! This tower helps uncover suspicious enemies and allows them to be targetted!",
            towerImage1);
        yield return WaitForPrompt();
        // Enable them after user has read the prompt
        UIManager.main.EnableTowerMenu();
        UIManager.main.EnableStartWaveButton();
    }
}
