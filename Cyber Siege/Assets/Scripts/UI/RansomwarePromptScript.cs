using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class RansomwarePromptScript : MonoBehaviour
{
    [Header("References")]

    // Comment these references to go back to dynamic population of buttons
    [SerializeField] private Button purchaseButton;
    [SerializeField] private Button exitButton;

    private void Start()
    {
        // Add Event Listeners

        // LevelManager.main.onCurrencyChange.AddListener(ReturnSuccess);
        purchaseButton.onClick.AddListener(Purchase);
        exitButton.onClick.AddListener(CloseTowerUpgradeMenuButtonOnClick);

    }

    public void Purchase()
    {
        // Check whether enough money
        // Purchase on a 50%
        int choice = Random.Range(0, 2);  // Random.Range(min, max) where max is exclusive

        LevelManager.main.SpendCurrency(100);
        RansomwareScript ransomware = EnemyManager.main.GetRansomwareScript();

        // Disable prompting
        ransomware.onPurchase();

        if (choice == 1) // true
        {
            UIManager.main.ShowErrorPrompt("Successful!");
            UIManager.main.CloseRansomwarePrompt();
            // Kill the Ransomware
            ransomware.DestroySelf();
        }
        else
        {
            UIManager.main.ShowErrorPrompt("Not successful!");
            UIManager.main.CloseRansomwarePrompt();
        }

    }

    // On Click Functions
    public void CloseTowerUpgradeMenuButtonOnClick()
    {
        UIManager.main.CloseRansomwarePrompt();
    }
}
