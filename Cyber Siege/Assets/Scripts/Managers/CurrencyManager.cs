using System;
using UnityEngine;
using UnityEngine.Events;

public class CurrencyManager : MonoBehaviour
{
    public static CurrencyManager main;

    [Header("Events")]
    public UnityEvent<int> onCurrencyChange = new UnityEvent<int>();

    [Header("Attributes")]
    [SerializeField] private int currency;
    [SerializeField] private float resourceMonitorMultiplier = 1.1f;

    private int resouceMonitorMultiplierStacks = 0;

    private void Awake()
    {
        if (main != null && main != this)
        {
            Destroy(this);
        }
        else
        {
            main = this;
        }
    }

    public int GetCurrency()
    {
        return currency;
    }

    public void IncreaseCurrency(int amt)
    {
        if (amt < 0) throw new ArgumentOutOfRangeException("Cannot Add Negative Currency");
        currency += amt;
        onCurrencyChange.Invoke(amt);
    }

    public void GainCurrencyFromKillingEnemy(int amt)
    {
        if (amt < 0) throw new ArgumentOutOfRangeException("Cannot Add Negative Currency");
        float multiplier = Mathf.Pow(resourceMonitorMultiplier, resouceMonitorMultiplierStacks);
        IncreaseCurrency(Mathf.RoundToInt(amt * multiplier));
    }

    public bool SpendCurrency(int amt)
    {
        if (amt < 0) throw new ArgumentOutOfRangeException("Cannot Spend Negative Currency");
        if (amt <= currency)
        {
            //Buy item
            DecreaseCurrency(amt);
            return true;
        }
        else
        {
            //Do some error prompt
            if (UIManager.main)
            {
                UIManager.main.ShowErrorPrompt("Not enuf money");
            }
            Debug.Log("Not enuf money");
            return false;
        }
    }

    public void DecreaseCurrency(int amt)
    {
        if (amt < 0) throw new ArgumentOutOfRangeException("Cannot Subtract Negative Currency");
        if (currency > 0)
        {
            currency -= amt;
            // Prevent negative currency
            if (currency < 0)
            {
                currency = 0;
            }
            onCurrencyChange.Invoke(-amt);
        }
    }

    public void IncreaseRMMultiplierStacks(int amt)
    {
        if (amt < 0) throw new ArgumentOutOfRangeException("Cannot Add Negative Stacks");
        resouceMonitorMultiplierStacks += amt;
    }

    public void DecreaseRMMultiplierStacks(int amt)
    {
        if (amt < 0) throw new ArgumentOutOfRangeException("Cannot Decrease Negative Stacks");
        if (resouceMonitorMultiplierStacks > 0)
        {
            resouceMonitorMultiplierStacks -= amt;
            if (resouceMonitorMultiplierStacks < 0)
            {
                resouceMonitorMultiplierStacks = 0;
            }
        }
    }

    public int GetRMMultiplierStacks()
    {
        return resouceMonitorMultiplierStacks;
    }

    // For Testing
    public void Reset()
    {
        currency = 0;
        resouceMonitorMultiplierStacks = 0;
    }
}
