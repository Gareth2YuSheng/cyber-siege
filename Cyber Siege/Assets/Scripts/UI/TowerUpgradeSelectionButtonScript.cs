using TMPro;
using UnityEngine;

public class TowerUpgradeSelectionButtonScript : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private TextMeshProUGUI upgradeNameLabel;
    [SerializeField] private TextMeshProUGUI upgradeDescriptionLabel;
    [SerializeField] private TextMeshProUGUI upgradeCostLabel;

    public void SetButtonLabels(string _name, string description, string cost)
    {
        upgradeNameLabel.text = _name;
        upgradeDescriptionLabel.text = description;
        upgradeCostLabel.text = cost;
    }
}
