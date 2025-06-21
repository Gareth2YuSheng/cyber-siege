using UnityEngine;
using UnityEngine.Events;

public class LevelManager : MonoBehaviour
{
    public static LevelManager main;

    [Header("References")]
    public Transform startPoint;

    [Header("Attributes")]
    public Transform[] enemyPath;
    public int currency;
    public int serverHealth;

    private bool isServerAlive = true;

    [Header("Events")]
    public UnityEvent onCurrencyChange = new UnityEvent();
    public UnityEvent onHealthChange = new UnityEvent();
    public UnityEvent onServerDeath = new UnityEvent();

    private void Awake()
    {
        main = this;
    }

    private void Start()
    {
        // currency = 100;
        IncreaseCurrency(100);
        // serverHealth = 100;
        HealServer(100);
    }

    //Currency Related Functions
    public void IncreaseCurrency(int amt)
    {
        currency += amt;
        onCurrencyChange.Invoke();
    }

    public bool SpendCurrency(int amt)
    {
        if (amt <= currency)
        {
            //Buy item
            currency -= amt;
            onCurrencyChange.Invoke();
            return true;
        }
        else
        {
            //Do some error prompt
            UIManager.main.ShowErrorPrompt("Not enuf money");
            Debug.Log("Not enuf money");
            return false;
        }
    }

    //Health Related Functions
    public void HealServer(int amt)
    {
        serverHealth += amt;
        onHealthChange.Invoke();
    }

    public void DamageServer(int amt)
    {
        if (isServerAlive)
        {
            serverHealth -= amt;
            onHealthChange.Invoke();
            if (serverHealth <= 0)
            {
                isServerAlive = false;
                onServerDeath.Invoke();
            }
        }
    }
}
