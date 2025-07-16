using System.Collections;
using UnityEngine;

public class Level2Manager : LevelManager
{
    [Header("Attributes")]
    [SerializeField] private Sprite enemyImage;

    protected override IEnumerator StartLevel()
    {
        // Disable start button and tower menu while user reads the prompt
        UIManager.main.DisableTowerMenu();
        UIManager.main.DisableStartWaveButton();
        // Virus Prompt
        UIManager.main.SetLevelPromptContent(
            "New Enemy Detected: Virus",
            "A virus is a bad computer program that attaches itself to other programs or files. It spreads when you open or share those files. A virus can mess up your computer, make it run slow, or even delete important stuff. It can spread to other computers when you share infected files or programs with others. DO NOT shoot with Network Scrubber!",
            enemyImage);
        yield return WaitForPrompt();
        // Virus Prompt
        yield return WaitForPrompt();
        // Enable them after user has read the prompt
        UIManager.main.EnableTowerMenu();
        UIManager.main.EnableStartWaveButton();
    }
}
