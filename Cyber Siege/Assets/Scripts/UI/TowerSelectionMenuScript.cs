using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TowerSelectionMenuScript : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject towerSelectionButtonPrefab;
    [SerializeField] private SpriteRenderer towerPreviewSR;

    private void Start()
    {
        for (int i = 0; i < BuildManager.main.towers.Length; i++)
        {
            int currIndex = i;

            // Create the buttons
            GameObject selectionButton = Instantiate(towerSelectionButtonPrefab, gameObject.transform);

            Button button = selectionButton.GetComponentInChildren<Button>();

            // Set the button image
            Image buttonImage = button.GetComponent<Image>();
            buttonImage.sprite = BuildManager.main.towers[i].sprite;

            // Set Cost Label
            TextMeshProUGUI label = button.GetComponentInChildren<TextMeshProUGUI>();
            label.text = $"${BuildManager.main.towers[i].towerSObj.cost}";

            // Set Click Listener
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(() => { TowerSelectButtonOnClick(currIndex); });
        }
    }

    private void Update()
    {

    }

    public void TowerSelectButtonOnClick(int towerIndex)
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

            // Fire Prompt
            UIManager.main.ShowErrorPrompt("Cannot Afford This Tower!");
        }
    }
}
