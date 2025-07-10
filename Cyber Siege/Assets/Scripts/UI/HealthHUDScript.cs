using UnityEngine;
using TMPro;

public class HealthHUDScript : MonoBehaviour
{
    private TextMeshProUGUI healthLabel;

    private void Start()
    {
        healthLabel = gameObject.GetComponentInChildren<TextMeshProUGUI>();
        //Add Event Listener
        HealthManager.main.onHealthChange.AddListener(UpdateHealthLabel);
    }

    public void UpdateHealthLabel()
    {
        healthLabel.text = $"{HealthManager.main.GetServerHealth()}";
    }
}
