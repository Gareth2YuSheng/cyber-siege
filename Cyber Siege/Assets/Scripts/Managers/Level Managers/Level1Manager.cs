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
        DisableUIs();
        // Worm Prompt
        yield return ShowPrompt(
            "New Enemy Detected: Worm",
            "A worm is a bad computer program that makes copies of itself and spreads to other computers. It can get into your computer by finding a weakness or by tricking people into clicking on links in emails or text messages. Once it's in, it can mess up files, add more bad programs, or keep spreading until the computer is too full to work. Hit it fast to prevent it from REPLICATING!",
            enemyImage);
        // Antivirus Prompt
        yield return ShowPrompt(
            "New Tower: Antivirus",
            "Shoots at a certain enemy type! Use them to your advantage!",
            towerImage1);
        // Network Scrubber Prompt
        yield return ShowPrompt(
            "New Tower: Network Scrubber",
            "Shoots at enemies in all directions. Dynamic!",
            towerImage2);
        // Enable them after user has read the prompt
        EnableUIs();
    }
}
