using UnityEngine;
using UnityEngine.Events;

public class CurrencyManager : MonoBehaviour
{
    public static CurrencyManager main;

    [Header("Events")]
    public UnityEvent onCurrencyChange = new UnityEvent();

    [Header("Attributes")]
    [SerializeField] private int currency;

    private void Awake()
    {
        main = this;
    }

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
}
