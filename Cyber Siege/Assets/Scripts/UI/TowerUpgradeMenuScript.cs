using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class TowerUpgradeMenuScript : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private TextMeshProUGUI towerNameLabel;
    [SerializeField] private GameObject towerUpgradeSelectionButtonPrefab;
    [SerializeField] private GameObject towerUpgradeButtonSection;

    // Comment these references to go back to dynamic population of buttons
    [SerializeField] private Button upgradeButton1;
    [SerializeField] private Button upgradeButton2;
    [SerializeField] private TowerUpgradeSelectionButtonScript upgradeButton1Script;
    [SerializeField] private TowerUpgradeSelectionButtonScript upgradeButton2Script;

    private void Start()
    {
        // Add Event Listeners
        BuildManager.main.onTowerSelectedForUpgrading.AddListener(UpdateTowerDetails);
        LevelManager.main.onCurrencyChange.AddListener(CheckUpgradesAffordable);
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
        upgradeButton1Script.SetButtonLabels(
            tower.upgrades[0].upgradeName,
            tower.upgrades[0].description,
            tower.upgrades[0].cost.ToString()
        );
        upgradeButton2Script.SetButtonLabels(
            tower.upgrades[1].upgradeName,
            tower.upgrades[1].description,
            tower.upgrades[1].cost.ToString()
        );

        // If not enough currency for upgrade, make button red
        CheckUpgradesAffordable();

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

        // Force Rebuild Upgrade Button Section
        LayoutRebuilder.ForceRebuildLayoutImmediate(towerUpgradeButtonSection.GetComponent<RectTransform>());
    }

    private void CheckUpgradesAffordable()
    {
        BasicTowerScript tower = BuildManager.main.GetSelectedTowerToUpgrade();
        // If no tower has been selected, return
        if (tower == null) return;

        Debug.Log($"Checking if Upgrades are Affordable for {tower.towerName}");
        int currency = LevelManager.main.currency;
        // Check first upgrade
        if (tower.upgrades[0].cost > currency)
        {
            // Disable button
            upgradeButton1.interactable = false;
            // Make button red?
        }
        else
        {
            // Enable button
            upgradeButton1.interactable = true;
        }
        // Check second upgrade 
        if (tower.upgrades[1].cost > currency)
        {
            // Disable button
            upgradeButton2.interactable = false;
        }
        else
        {
            // Enable button
            upgradeButton2.interactable = true;
        }
    }

    // On Click Functions
    public void CloseTowerUpgradeMenuButtonOnClick()
    {
        BuildManager.main.CloseTowerUpgradeMenu();
    }
}
