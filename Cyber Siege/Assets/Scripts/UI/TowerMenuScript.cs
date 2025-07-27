using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TowerMenuScript : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Button cancelButton;
    [SerializeField] private SpriteRenderer towerPreviewSR;
    [SerializeField] private TextMeshProUGUI moneyLabel;
    [SerializeField] private GameObject currencyPopup;
    [SerializeField] private TextMeshProUGUI waveLabel;

    private bool initCurrency = false;

    private void Start()
    {
        //Hide tower preview first
        towerPreviewSR.enabled = false;
        //No need to edit the position as it should already be placed 
        //outside of the scene

        // Add Event Listeners
        CurrencyManager.main.onCurrencyChange.AddListener(UpdateCurrency);
        EnemyManager.main.onWaveStart.AddListener(UpdateWaveLabel);

        BuildManager.main.onStartGroundBuilding.AddListener(StartBuilding);
        BuildManager.main.onStartPathBuilding.AddListener(StartBuilding);
        BuildManager.main.onStopGroundBuilding.AddListener(StopBuilding);
        BuildManager.main.onStopPathBuilding.AddListener(StopBuilding);
    }

    public void UpdateWaveLabel()
    {
        string labelText = $"Wave: {EnemyManager.main.GetCurrentWave()}";
        if (EnemyManager.main.GetMaxWaveCount() > 0)
        {
            labelText += $"/{EnemyManager.main.GetMaxWaveCount()}";
        }
        waveLabel.text = labelText;
    }

    private void UpdateCurrency(int amt)
    {
        if (initCurrency) SpawnCurrencyPopup(amt);
        UpdateCurrencyLabel();

        if (!initCurrency) initCurrency = true;
    }

    public void UpdateCurrencyLabel()
    {
        moneyLabel.text = $"${CurrencyManager.main.GetCurrency()}";
    }

    private void SpawnCurrencyPopup(int amt)
    {
        GameObject popup = Instantiate(currencyPopup,
            new Vector3(moneyLabel.transform.position.x - 18f, moneyLabel.transform.position.y, moneyLabel.transform.position.z - 10f),
            Quaternion.identity,
            gameObject.transform);
        popup.GetComponent<CurrencyPopupScirpt>().SetLabel(amt);
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

    public void TowerMenuBuildCancelButtonOnClick()
    {
        BuildManager.main.DisableBuilding();
        // Hide Tower Preview
        towerPreviewSR.enabled = false;
    }

}
