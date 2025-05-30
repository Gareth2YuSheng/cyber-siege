using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager main;

    [Header("References")]
    public Transform startPoint;

    [Header("Attributes")]
    public Transform[] enemyPath;
    public int currency; //Set to non-serialised after finish testing
    public int serverHealth; //Set to non-serialised after finish testing

    public bool isServerAlive = true;

    private void Awake()
    {
        main = this;
    }

    private void Start()
    {
        currency = 100;
        serverHealth = 100;
    }

    //Currency Related Functions
    public void IncreaseCurrency(int amt)
    {
        currency += amt;
    }

    public bool SpendCurrency(int amt)
    {
        if (amt <= currency)
        {
            //Buy item
            currency -= amt;
            return true;
        }
        else
        {
            //Do some error prompt
            Debug.Log("Not enuf money");
            return false;
        }
    }

    //Health Related Functions
    public void HealServer(int amt)
    {
        serverHealth += amt;
    }

    public void DamageServer(int amt)
    {
        if (isServerAlive)
        {
            serverHealth -= amt;
            if (serverHealth <= 0)
            {
                isServerAlive = false;
            }
        }
    }
}
