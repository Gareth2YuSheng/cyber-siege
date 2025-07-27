using System.Collections;
using UnityEngine;

public class Level1Manager : LevelManager
{
    [Header("Attributes")]
    [SerializeField] private Sprite enemyImage;
    [SerializeField] private Sprite towerImage1;
    [SerializeField] private Sprite towerImage2;

    /*
        Map Design Idea:
        Start level, make it easy such that anyone can pass it.
    */

    protected override IEnumerator StartLevel()
    {
        // Disable start button and tower menu while user reads the prompt
        DisableUIs();
        // Worm Prompt
        yield return ShowPrompt(
            "New Enemy Detected: Worm",
            "A worm is a bad program that spreads by copying itself to other computers.\n\nIt sneaks in through weak spots or fake links and can mess things up or slow your computer down.\n\nStop it fast before it spreads!",
            enemyImage);
        // Antivirus Prompt
        yield return ShowPrompt(
            "New Tower: Antivirus",
            "Shoots at all enemies! Use them to your advantage!",
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
