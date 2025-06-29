using TMPro;
using UnityEngine;

public class TowerUpgradeSelectionButtonScript : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private TextMeshProUGUI upgradeNameLabel;
    [SerializeField] private TextMeshProUGUI upgradeDescriptionLabel;
    [SerializeField] private TextMeshProUGUI upgradeCostLabel;

    public void SetButtonLabels(string _name, string description, int cost, bool purchased)
    {
        upgradeNameLabel.text = _name;
        upgradeDescriptionLabel.text = description;
        SetButtonCostLabel(cost, purchased);
    }

    public void SetButtonCostLabel(int cost, bool purchased)
    {
        upgradeCostLabel.text = purchased ? "Purchased" : (cost <= 0 ? "" : "$" + cost.ToString());
    }
}
