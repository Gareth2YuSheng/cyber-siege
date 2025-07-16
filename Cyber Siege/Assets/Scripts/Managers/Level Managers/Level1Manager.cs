using System.Collections;
using UnityEngine;

public class Level1Manager : LevelManager
{
    [Header("Attributes")]
    [SerializeField] private Sprite enemyImage;
    [SerializeField] private Sprite towerImage1;
    [SerializeField] private Sprite towerImage2;


    protected override IEnumerator StartLevel()
    {
        // Disable start button and tower menu while user reads the prompt
        UIManager.main.DisableTowerMenu();
        UIManager.main.DisableStartWaveButton();
        // Virus Prompt
        UIManager.main.SetLevelPromptContent(
            "New Enemy Detected: Worm",
            "A worm is a bad computer program that makes copies of itself and spreads to other computers. It can get into your computer by finding a weakness or by tricking people into clicking on links in emails or text messages. Once it's in, it can mess up files, add more bad programs, or keep spreading until the computer is too full to work. Hit it fast to prevent it from REPLICATING!",
            enemyImage);
        yield return WaitForPrompt();
        // Virus Prompt
        UIManager.main.SetLevelPromptContent(
            "New Tower: Antivirus",
            "Shoots at a certain enemy type! Use them to your advantage!",
            towerImage1);
        yield return WaitForPrompt();
        UIManager.main.SetLevelPromptContent(
            "New Tower: Network Scrubber",
            "Shoots at enemies in all directions. Dynamic!",
            towerImage2);
        yield return WaitForPrompt();
        // Enable them after user has read the prompt
        UIManager.main.EnableTowerMenu();
        UIManager.main.EnableStartWaveButton();
    }
}
