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
            "A virus is a harmful program that attaches itself to other files or software.\n\nUnstable and aggressive, it can behave unpredictably when attacked — <b>some defenses might even make things worse</b>.\n\nChoose your countermeasures carefully.",
            enemyImage);
        // Enable them after user has read the prompt
        EnableUIs();
    }
}
