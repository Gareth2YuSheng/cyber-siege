using UnityEngine;

public class TutorialPointScript : MonoBehaviour
{
    private bool seenEnemy = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Should only trigger 1 function call when seeing the very first enemy in the map
        if (collision.gameObject.layer == LayerMask.NameToLayer("Enemy") && !seenEnemy)
        {
            seenEnemy = true;
            TutorialManager.main.SeenEnemy();
        }
    }
}
