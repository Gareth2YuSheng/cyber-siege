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
            "Neigh! A Trojan hides in plain sight — it looks like something safe, but once inside, it causes trouble.\n\nOften spread through phishing or fake sites, Trojans must be revealed before they can be targeted.\n\nHmm... which tower sees through the disguise",
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
