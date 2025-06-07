using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TowerMenuScript : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Button cancelButton;
    [SerializeField] private SpriteRenderer towerPreviewSR;
    [SerializeField] private TextMeshProUGUI moneyLabel;
    [SerializeField] private TextMeshProUGUI waveLabel;


    private void Start()
    {
        //Hide tower preview first
        towerPreviewSR.enabled = false;
        //No need to edit the position as it should already be placed 
        //outside of the scene

        // Add Event Listeners

        LevelManager.main.onCurrencyChange.AddListener(UpdateCurrencyLabel);
        EnemyManager.main.onWaveEnd.AddListener(UpdateWaveLabel);

        BuildManager.main.onStartBuilding.AddListener(StartBuilding);
        BuildManager.main.onStopBuilding.AddListener(StopBuilding);
    }

    private void Update()
    {

    }

    private void UpdateWaveLabel()
    {
        waveLabel.text = $"Wave: {EnemyManager.main.currentWave}";
    }


    private void UpdateCurrencyLabel()
    {
        moneyLabel.text = $"{LevelManager.main.currency}";
    }

    private void StartBuilding()
    {
        // Show Cancel Building Button
        cancelButton.gameObject.SetActive(true);
    }

    private void StopBuilding()
    {
        // Hide Cancel Building Button
        cancelButton.gameObject.SetActive(false);
    }

    // ON CLICK FUNCTIONS

    public void TowerMenuTowerSelectButtonOnClick(int towerIndex)
    {
        Debug.Log($"Selected Tower {towerIndex}");
        BuildManager.main.SetSelectedTower(towerIndex);
        //Check if player can afford the tower
        if (BuildManager.main.CanAffordSelectedTower())
        {
            BuildManager.main.EnableBuilding();
            // Show Tower Preview
            towerPreviewSR.enabled = true;
        }
        else
        {
            //Error Message Here
            Debug.Log("Cannot Afford This Tower!");
        }
    }

    public void TowerMenuBuildCancelButtonOnClick()
    {
        Debug.Log("Cancel Build Mode");
        BuildManager.main.DisableBuilding();
        // Hide Tower Preview
        towerPreviewSR.enabled = false;
    }

}
