using UnityEngine;
using TMPro;

public class HealthHUDScript : MonoBehaviour
{
    private TextMeshProUGUI healthLabel;

    private void Awake()
    {
        healthLabel = gameObject.GetComponentInChildren<TextMeshProUGUI>();
    }

    private void Start()
    {
        //Add Event Listener
        HealthManager.main.onHealthChange.AddListener(UpdateHealthLabel);
    }

    public void UpdateHealthLabel()
    {
        healthLabel.text = $"{HealthManager.main.GetServerHealth()}";
    }
}
