using System.Collections;
using UnityEngine;

public class Level2Manager : LevelManager
{
    [Header("Attributes")]
    [SerializeField] private Sprite enemyImage;

    /*
        Map Design Idea:
        Level 2, Starting with 100 currency is a must as players will need at least two towers to gain more currency.
        Difficulty scale x1.3. 2 is too much.
    */
    
    protected override IEnumerator StartLevel()
    {
        // Disable start button and tower menu while user reads the prompt
        DisableUIs();
        // Virus Prompt
        yield return ShowPrompt(
            "New Enemy Detected: Virus",
            "A virus is a bad computer program that attaches itself to other programs or files. It spreads when you open or share those files. A virus can mess up your computer, make it run slow, or even delete important stuff. It can spread to other computers when you share infected files or programs with others. DO NOT shoot with Network Scrubber!",
            enemyImage);
        // Enable them after user has read the prompt
        EnableUIs();
    }
}
