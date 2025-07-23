using System.Collections;
using UnityEngine;

public class Level3Manager : LevelManager
{
    [Header("Attributes")]
    [SerializeField] private Sprite enemyImage1;
    [SerializeField] private Sprite enemyImage2;

    [SerializeField] private Sprite towerImage1;

    /*
        Map Design Idea:
        Level 3, Starting with 60 currency is sufficient as it forces players to upgrade and avoid using Network Scrubber.
        Difficulty scale x1.3.
    */
    
    protected override IEnumerator StartLevel()
    {
        // Disable start button and tower menu while user reads the prompt
        DisableUIs();
        // Virus Prompt
        yield return ShowPrompt(
            "New Enemy Detected: DoS",
            "A Denial-of-Service (DoS) attack is when a bad person sends too many fake requests to a network, making it hard for normal users to do things like check email, visit websites, or use online accounts. It doesn’t usually cause data loss, but it can waste a lot of time and money trying to fix the problem. The DoS enemy produces DDoS bots! Nuke it quick before it spawns too many!",
            enemyImage1);
        // DDoS Prompt
        yield return ShowPrompt(
            "New Enemy Detected: DDoS",
            "Like DoS, but DDoS comes from many computers. DDoS attacks are harder to stop because there are more computers involved in the attack. It a DoS! They spawn rapidly from DDoS enemies!",
            enemyImage2);
        // Firewall Prompt
        yield return ShowPrompt(
            "New Tower: Firewall",
            "The Firewall tower acts as your fiery defence towards Enemy types! Though, be warned, it can only protect you to a certain extent!",
            towerImage1);
        // Enable them after user has read the prompt
        EnableUIs();
    }
}
