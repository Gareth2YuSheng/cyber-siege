using System.Collections;
using UnityEngine;

public class Level7Manager : LevelManager
{
    [Header("References")]
    [SerializeField] private Sprite resourceMonitorImage;
    [SerializeField] private Sprite cryptojackingImage;

    /*
        Map Design Idea:
        Shorter map to give player chance to make use of path towers more
        Also to give less distractions so that they can look out for cryptojacking
        *Cryptojacking spawn rate is increased in this level but interval increased to 7 seconds for balancing.
    */

    protected override IEnumerator StartLevel()
    {
        // Disable start button and tower menu while user reads the prompt
        DisableUIs();
        // Cryptojacking Prompt
        yield return ShowPrompt(
            "New Enemy Detected: Cryptojacking",
            "Cryptojacking is when hackers secretly use your system to mine cryptocurrency, quietly draining resources and slowing performance. These stealthy enemies latch onto your server, slowing your defenses and clogging up performance. If something feels off, it might be time for a system check.",
            cryptojackingImage);
        // Resource Monitor Prompt
        yield return ShowPrompt(
            "New Tower: Resource Monitor",
            "This tower can scan for hidden threats and cleanse spyware, keeping your server secure and your resources flowing.",
            resourceMonitorImage);
        // Enable them after user has read the prompt
        EnableUIs();
    }
}
