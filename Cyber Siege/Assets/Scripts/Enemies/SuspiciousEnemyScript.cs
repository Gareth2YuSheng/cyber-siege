using UnityEngine;

public class SuspiciousEnemyScript : MonoBehaviour
{
    private BasicEnemyScript myBEScript;

    private void Start()
    {
        // Hide phishing enemies first
        myBEScript = gameObject.GetComponent<BasicEnemyScript>();
        myBEScript.Hide();
    }

    private void Update()
    {

    }

    public void Reveal()
    {
        myBEScript.Reveal();
    }
}
