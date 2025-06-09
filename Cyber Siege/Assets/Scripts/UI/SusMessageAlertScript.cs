using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class SusMessageAlertScript : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private TextMeshProUGUI susMessageTitle;
    [SerializeField] private TextMeshProUGUI susMessageBody;
    [SerializeField] private TextMeshProUGUI susMessageActionButtonText;
    [SerializeField] private Button susMessageCloseButton;

    private string[] scamMessagesTitles = new string[] {
        "[ALERT] URGENT: Your account has been compromised!",
        "Congratulations! You’ve won a $1,000 gift card!",
        "Unusual login detected from Kuala Lumpur, Malaysia.",
        "Your tax refund is ready!",
        "Warning: Suspicious activity detected on your network.",
        "Your package is on hold due to unpaid customs fees.",
        "System Alert: Your device is infected with 3 viruses!"
    };

    private string[] scamMessageBodies = new string[] {
        "Tap here to claim your refund before your account is locked permanently.",
        "Confirm your prize before it expires in 10 minutes.",
        "Was this you?",
        "You may be eligible for up to $2,500.",
        "Click to activate firewall protection.",
        "Settle now to avoid return.",
        "Immediate action is required."
    };

    private string[] scamMessageActions = new string[] {
        "[Click to Verify] Failure to act may result in data loss.",
        "[Claim Now]",
        "[Yes, it was me] [No, secure account]",
        "Tap to complete verification.",
        "[Enable Protection]",
        "[Pay Fees]",
        "[Clean Now]"
    };

    private void Start()
    {
        // Add close button functionality
        susMessageCloseButton.onClick.AddListener(() => Destroy(gameObject));
    }

    private void Update()
    {

    }

    public void PopulateMessage()
    {
        int index = Random.Range(0, scamMessagesTitles.Length);
        susMessageTitle.text = scamMessagesTitles[index];
        susMessageBody.text = scamMessageBodies[index];
        susMessageActionButtonText.text = scamMessageActions[index];
    }
}
