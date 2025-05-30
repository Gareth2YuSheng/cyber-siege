using UnityEngine;
using TMPro;

public class HealthHUDScript : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI healthLabel;

    private void Update()
    {
        //Health Label
        healthLabel.text = $"{LevelManager.main.serverHealth}";
    }
}
