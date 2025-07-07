using UnityEngine;
using UnityEngine.Events;

public class LevelManager : MonoBehaviour
{
    public static LevelManager main;

    [Header("References")]
    public Transform startPoint;
    [SerializeField] private ServerScript myServer;

    [Header("Attributes")]
    public Transform[] enemyPath;
    [SerializeField] private int currency;
    [SerializeField] private int serverHealth;
    private int baseHealth = 0;
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
        // To be called by the individual level Managers
        // IncreaseCurrency(200);
        // HealServer(100);

        // Ensure game is unpaused
        Time.timeScale = 1;
    }

    public void InitLevel(int _currency, int _health)
    {
        IncreaseCurrency(_currency);
        HealServer(_health);
        baseHealth = _health;
    }

    //Currency Related Functions
    public int GetCurrency()
    {
        return currency;
    }

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
    public int GetServerHealth()
    {
        return serverHealth;
    }

    public void HealServer(int amt)
    {
        // If baseHealth has not been set, means we are initialising the health
        if (baseHealth == 0)
        {
            serverHealth += amt;
            onHealthChange.Invoke();
        }
        // else if health is not full, heal
        else if (serverHealth < baseHealth)
        {
            serverHealth += amt;
            onHealthChange.Invoke();
            // Dont let server HP go above max
            if (serverHealth > baseHealth) serverHealth = baseHealth;
            // If server hp goes above 50%, switch to the health sprite
            if (serverHealth > baseHealth / 2)
            {
                myServer.UpdateHealthySprite();
            }
        }
    }

    public void DamageServer(int amt)
    {
        if (isServerAlive)
        {
            serverHealth -= amt;
            onHealthChange.Invoke();
            // If server hp drops below 50%, switch to damaged sprite
            if (serverHealth <= baseHealth / 2)
            {
                myServer.UpdateDamagedSprite();
            }
            if (serverHealth <= 0)
            {
                isServerAlive = false;
                onServerDeath.Invoke();
            }
        }
    }
}
