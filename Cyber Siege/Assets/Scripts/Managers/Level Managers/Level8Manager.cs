using System.Collections;
using UnityEngine;

public class Level8Manager : LevelManager
{
    [Header("References")]
    [SerializeField] private Sprite encryptionNodeImage;
    [SerializeField] private Sprite ransomwareImage;

    /*
        Map Design Idea:
        Long map to give player chance to defend against ransomware
        *Ransomware spawn rate is increased in this level

        Increase initial currency to 200 in case player cant handle ransomware properly
    */

    protected override IEnumerator StartLevel()
    {
        // Disable start button and tower menu while user reads the prompt
        DisableUIs();
        // Ransomware Prompt
        yield return ShowPrompt(
            "New Enemy Detected: Ransomware",
            "Ransomware locks down your defenses and demands a price—literally. It disrupts nearby towers as it moves, forcing tough choices: pay up for a risky fix or use the right tool to defend against it safely. \n*Click on ransomware to pay ransom",
            ransomwareImage);
        // Resource Monitor Prompt
        yield return ShowPrompt(
            "New Tower: Encryption Node",
            "This tower acts as a digital shield, silently absorbing ransomware disruptions for nearby towers—though it needs a moment to recover after each block",
            encryptionNodeImage);
        // Enable them after user has read the prompt
        EnableUIs();
    }
}

