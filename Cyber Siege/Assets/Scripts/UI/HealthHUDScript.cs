using UnityEngine;
using TMPro;

public class HealthHUDScript : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI healthLabel;

    private void Start()
    {
        //Add Event Listener
        LevelManager.main.onHealthChange.AddListener(UpdateHealthLabel);
    }

    private void Update()
    {

    }

    public void UpdateHealthLabel()
    {
        healthLabel.text = $"{LevelManager.main.serverHealth}";
    }
}
