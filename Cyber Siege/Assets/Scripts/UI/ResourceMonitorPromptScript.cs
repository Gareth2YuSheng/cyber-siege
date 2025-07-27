using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ResourceMonitorPromptScript : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject cleanServerButton;
    [SerializeField] private TextMeshProUGUI resourceMonitorPromptText;

    private ResourceMonitorScript selectedResourceMonitor;
    private TextMeshProUGUI cleanServerButtonText;
    private Button cleanServerButtonButton;

    private string initialPromptText = "Run security cleanup on the server?";
    private string[] cleaningLabels = new string[] { "Cleaning...", "Cleaning..", "Cleaning." };

    private void Awake()
    {
        cleanServerButtonText = cleanServerButton.GetComponentInChildren<TextMeshProUGUI>();
        cleanServerButtonButton = cleanServerButton.GetComponent<Button>();
    }

    public void SetResourceMonitorScript(ResourceMonitorScript script)
    {
        // Remove Previous Listener from previous resource monitor
        if (selectedResourceMonitor != null)
        {
            selectedResourceMonitor.onCooldownSecondChanged.RemoveListener(UpdateCleanServerButtonLabel);
        }
        // Update selected script to new one
        selectedResourceMonitor = script;
        // Add Event Listener
        selectedResourceMonitor.onCooldownSecondChanged.AddListener(UpdateCleanServerButtonLabel);
        // Update Label
        UpdateCleanServerButtonLabel(selectedResourceMonitor.GetCoolDown());
    }

    public void UpdateCleanServerButtonLabel(int remainingCooldown)
    {
        // Disable button if not ready
        cleanServerButtonButton.interactable = remainingCooldown == 0;
        cleanServerButtonText.text = remainingCooldown == 0 ? "Clean" : $"{remainingCooldown}s";
        // Update Prompt Text
        resourceMonitorPromptText.text = remainingCooldown == 0 ? initialPromptText : cleaningLabels[remainingCooldown % 3];
    }

    // On Click Functions
    public void RMPromptYesButtonOnClick()
    {
        // Purge Cryptojacking
        selectedResourceMonitor.CleanupServer();
    }

    public void RMPromptCancelButtonOnClick()
    {
        // Close the Prompt
        UIManager.main.HideResourceMonitorPrompt();
    }
}
