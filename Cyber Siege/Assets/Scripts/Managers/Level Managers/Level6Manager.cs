using System.Collections;
using UnityEngine;

public class Level6Manager : LevelManager
{
    [Header("Attributes")]
    [SerializeField] private Sprite enemyImage1;
    [SerializeField] private Sprite towerImage1;

    /*
        Map Design Idea:
        Level 6. Shaped like a race derby track, cuz we have a trojan that resembles a horse when revealed.
        Starting with 100 currency is sufficient as it allows players to anticipate the enemy and use whatever is available.
        Level also forces use of IDS.
        Difficulty scale x1.3.
    */

    protected override IEnumerator StartLevel()
    {
        // Disable start button and tower menu while user reads the prompt
        DisableUIs();
        // Trojan Prompt
        yield return ShowPrompt(
            "New Enemy Detected: Trojan",
            "Neigh! A Trojan is a type of bad software that looks like a normal program or file, such as something you might download for free. It tricks you into thinking it's safe, but once installed, it can cause harm. Trojans are often spread through tricks like phishing or fake websites. Trojans will need to be revealed in order to be targetted! I wonder what tower we can use...",
            enemyImage1);
        // 2FA Gate Prompt
        yield return ShowPrompt(
            "New Tower: 2FA Gate",
            "Two Factor Authentication Gate! This tower slows down enemies passing through a small area as it requests for Authentication!",
            towerImage1);
        // Enable them after user has read the prompt
        EnableUIs();
    }
}
