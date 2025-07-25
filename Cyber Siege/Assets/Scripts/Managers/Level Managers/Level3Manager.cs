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
            "A Denial-of-Service (DoS) attack floods a network with fake requests, slowing everything down.\n\nIt doesn’t steal data, but it can waste time and money.\n\nThe DoS enemy spawns DDoS bots that overwhelm your defenses — take it out fast before they swarm!",
            enemyImage1);
        // DDoS Prompt
        yield return ShowPrompt(
            "New Enemy Detected: DDoS",
            "Like DoS, but DDoS comes from many computers.\n\nDDoS attacks are harder to stop because there are more computers involved in the attack.\n\nIt a DoS! They spawn rapidly from DDoS enemies!",
            enemyImage2);
        // Firewall Prompt
        yield return ShowPrompt(
            "New Tower: Firewall",
            "The Firewall tower acts as your fiery defence and stops enemies in their tracks!\n\nThough, be warned, it can only protect you to a certain extent!",
            towerImage1);
        // Enable them after user has read the prompt
        EnableUIs();
    }
}
