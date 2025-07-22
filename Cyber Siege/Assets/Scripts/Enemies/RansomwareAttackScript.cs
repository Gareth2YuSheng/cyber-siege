using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RansomwareAttackScript : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private SpriteRenderer sr;

    private RansomwareScript myRansomwareScript;

    // If the shockwave lingers for multiple frames, OnTriggerEnter2D could trigger the same tower multiple times, thus use a hashmap to prevent it
    // private HashSet<BasicTowerScript> affectedTowers = new HashSet<BasicTowerScript>();

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Hits will be objects with colliders, which are currently the tower Bases
        // Script is in parent object
        BasicTowerScript tower = collision.GetComponentInParent<BasicTowerScript>();
        // if (tower != null && tower.towerName != "Encryption Node" && !affectedTowers.Contains(tower))
        if (tower != null && tower.towerName != "Encryption Node")
        {
            // affectedTowers.Add(tower);
            Debug.Log($"Shockwave hit tower: {tower.towerName}");
            tower.DisableTower(myRansomwareScript);
        }
    }

    public IEnumerator ExpandAndFade(float range, float duration)
    {
        Debug.Log("VVWOOM");
        // Set initial state to zero to make it small
        Vector3 initialScale = Vector3.zero;
        // This was trial and error calculations
        Vector3 targetScale = new Vector3(range * 9, range * 9, 1f);

        float elapsed = 0f;
        while (elapsed < duration)
        {
            transform.localScale = Vector3.Lerp(initialScale, targetScale, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        Destroy(gameObject);
    }

    public void SetRansomwareScript(RansomwareScript script)
    {
        myRansomwareScript = script;
    }
}
