using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class TowerUpgradeMenuScript : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private TextMeshProUGUI towerNameLabel;
    [SerializeField] private GameObject towerUpgradeSelectionButtonPrefab;
    [SerializeField] private GameObject towerUpgradeButtonSection;
    [SerializeField] private Color upgradePurchasedColor;

    // Comment these references to go back to dynamic population of buttons
    [SerializeField] private Button upgradeButton1;
    [SerializeField] private Button upgradeButton2;
    [SerializeField] private TowerUpgradeSelectionButtonScript upgradeButton1Script;
    [SerializeField] private TowerUpgradeSelectionButtonScript upgradeButton2Script;

    private Color initialDisabledColor;

    private void Start()
    {
        // Add Event Listeners
        BuildManager.main.onTowerSelectedForUpgrading.AddListener(UpdateTowerDetails);
        LevelManager.main.onCurrencyChange.AddListener(CheckUpgradesAffordable);

        initialDisabledColor = upgradeButton1.colors.disabledColor;
    }

    public void UpdateTowerDetails()
    {
        BasicTowerScript tower = BuildManager.main.GetSelectedTowerToUpgrade();
        // Set tower name label
        towerNameLabel.text = tower.towerName;

        // Populate menu buttons dynamically - should not have more than 2 currently
        // for (int i = 0; i < tower.upgrades.Length; i++)
        // {
        //     int currIndex = i;

        //     // Create the buttons
        //     GameObject selectionButton = Instantiate(towerUpgradeSelectionButtonPrefab, towerUpgradeButtonSection.transform);

        //     Button button = selectionButton.GetComponentInChildren<Button>();
        //     // Set Button Labels
        //     button.GetComponent<TowerUpgradeSelectionButtonScript>().SetButtonLabels(
        //         tower.upgrades[i].upgradeName,
        //         tower.upgrades[i].description,
        //         tower.upgrades[i].cost.ToString()
        //     );

        //     // // Set Click Listener
        //     button.onClick.RemoveAllListeners();
        //     button.onClick.AddListener(() => { Debug.Log(tower.upgrades[currIndex].upgradeName); });
        // }

        // Assuming we are never going to have more than 2 upgrades on a tower at a time
        // Set Upgrade button labels
        UpdateUpgradeButtonLabels(tower);

        // Set OnClick Listeners
        upgradeButton1.onClick.RemoveAllListeners();
        upgradeButton1.onClick.AddListener(() =>
        {
            // only upgrade if enough currency
            if (tower.upgrades[0].cost <= LevelManager.main.currency)
            {
                tower.Upgrade1();
            }
        });
        upgradeButton2.onClick.RemoveAllListeners();
        upgradeButton2.onClick.AddListener(() =>
        {
            if (tower.upgrades[1].cost <= LevelManager.main.currency)
            {
                tower.Upgrade2();
            }
        });

        // If not enough currency for upgrade, make button red
        CheckUpgradesAffordable();
    }

    private void UpdateUpgradeButtonLabels(BasicTowerScript tower)
    {
        upgradeButton1Script.SetButtonLabels(
            tower.upgrades[0].upgradeName,
            tower.upgrades[0].description,
            tower.upgrades[0].cost,
            tower.upgrades[0].purchased
        );
        upgradeButton2Script.SetButtonLabels(
            tower.upgrades[1].upgradeName,
            tower.upgrades[1].description,
            tower.upgrades[1].cost,
            tower.upgrades[1].purchased
        );
    }

    private void UpdateUpgradeButtonPurchasedLabel(BasicTowerScript tower)
    {
        upgradeButton1Script.SetButtonCostLabel(
            tower.upgrades[0].cost,
            tower.upgrades[0].purchased);
        upgradeButton2Script.SetButtonCostLabel(
            tower.upgrades[1].cost,
            tower.upgrades[1].purchased);
    }

    private void CheckUpgradesAffordable()
    {
        BasicTowerScript tower = BuildManager.main.GetSelectedTowerToUpgrade();
        // If no tower has been selected, return
        if (tower == null) return;

        UpdateUpgradeButtonPurchasedLabel(tower);

        Debug.Log($"Checking if Upgrades are Affordable for {tower.towerName}");
        // // Check first upgrade
        UpdateUpgradeButton(
            upgradeButton1,
            tower.upgrades[0].purchased,
            tower.upgrades[0].cost,
            LevelManager.main.currency
        );

        // Check second upgrade 
        UpdateUpgradeButton(
            upgradeButton2,
            tower.upgrades[1].purchased,
            tower.upgrades[1].cost,
            LevelManager.main.currency
        );

        // Force Rebuild Upgrade Button Section
        LayoutRebuilder.ForceRebuildLayoutImmediate(towerUpgradeButtonSection.GetComponent<RectTransform>());
    }

    private void UpdateUpgradeButton(Button button, bool purchased, int cost, int currency)
    {
        // If upgrade has not been purchased and we CAN afford it
        if (!purchased && cost <= currency)
        {
            // Enable button
            button.interactable = true;
        }
        else
        {
            // Disable button with green
            ColorBlock colors = button.colors;
            // Disable the button based on if purchased - green, else if not purchased and cannot afford - red
            colors.disabledColor = purchased ? upgradePurchasedColor : initialDisabledColor;
            button.colors = colors;
            button.interactable = false;
        }
    }

    // On Click Functions
    public void CloseTowerUpgradeMenuButtonOnClick()
    {
        BuildManager.main.CloseTowerUpgradeMenu();
    }
}
