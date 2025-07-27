using System.Collections;
using UnityEngine;

public class Level5Manager : LevelManager
{
    [Header("Attributes")]
    [SerializeField] private Sprite enemyImage1;
    [SerializeField] private Sprite enemyImage2;

    [SerializeField] private Sprite towerImage1;

    /*
        Map Design Idea:
        Level 5. Shaped like the Sus Call (or a banana).
        Starting with 100 currency is sufficient as it forces player to get either get Threat Intelligence, or go the IDS + Antivirus route.
        Difficulty scale x1.3.
    */

    protected override IEnumerator StartLevel()
    {
        // Disable start button and tower menu while user reads the prompt
        DisableUIs();
        // Suspicious Call Prompt
        yield return ShowPrompt(
            "New Enemy Detected: Suspicious Call",
            "Another sus enemy! Suspicious Calls pretend to be someone you trust to steal personal info.\n\nThis hidden threat boosts nearby phishing enemies — spot it fast before it strengthens the scam!",
            enemyImage1);
        // Suspicious Text Prompt
        yield return ShowPrompt(
            "New Enemy Detected: Suspicious Text Message",
            "Sneaky enemy detected! Suspicious Text Messages are hidden and fast, tricking you with fake texts pretending to be banks or delivery services.\n\nWatch out — they clutter your screen with fake SMS distractions!",
            enemyImage2);
        // IDS Prompt
        yield return ShowPrompt(
            "New Tower: IDS",
            "Intruder Detection System! This tower helps uncover suspicious and other hidden enemies, allowing them to be targetted!",
            towerImage1);
        // Enable them after user has read the prompt
        EnableUIs();
    }
}
