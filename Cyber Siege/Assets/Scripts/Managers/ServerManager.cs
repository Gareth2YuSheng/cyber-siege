using UnityEngine;

public class ServerManager : MonoBehaviour
{
    public static ServerManager main;

    // [Header("Attributes")]
    // [SerializeField] private float cryptojackingInterval = 3f;

    private int cryptojackingCounter = 0;
    private float timeUntilCryptoJacked;

    private void Awake()
    {
        main = this;
    }

    /* When affected by cryptojacking
        - Slowly chip away at the player's money
        - Slow down the firerate of all towers
    */
    // private void Update()
    // {

    // }

    public int GetCryptojackingCount()
    {
        return cryptojackingCounter;
    }

    public void AddCryptojacking(int amt)
    {
        cryptojackingCounter += amt;
    }

    public void RemoveCryptojacking(int amt)
    {
        cryptojackingCounter -= amt;
    }


}
