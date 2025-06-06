using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TowerMenuScript : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Button cancelButton;
    [SerializeField] private SpriteRenderer towerPreviewSR;
    [SerializeField] private TextMeshProUGUI moneyLabel;

    private void Start()
    {
        //Hide tower preview first
        towerPreviewSR.enabled = false;
        //No need to edit the position as it should already be placed 
        //outside of the scene
    }

    private void Update()
    {
        //Show/Hide Cancel Button
        //If build mode is activated and button is currently hidden
        if (BuildManager.main.isBuilding && !cancelButton.gameObject.activeSelf)
        {
            cancelButton.gameObject.SetActive(true);
        }
        //Else if the build mode has been deactivated and the button is still active
        else if (!BuildManager.main.isBuilding && cancelButton.gameObject.activeSelf)
        {
            cancelButton.gameObject.SetActive(false);
        }

        //Money Label
        moneyLabel.text = $"${LevelManager.main.currency}";
    }

    private void ActivateTowerPreview()
    {
        towerPreviewSR.enabled = true;
        //Change the Tower Preview Sprite
        towerPreviewSR.sprite = BuildManager.main.GetSelectedTower().sprite;
        //Make the Tower Preview follow the mouse - Done in TowerPreviewScript
    }

    private void ResetTowerPreview()
    {
        towerPreviewSR.enabled = false;
        towerPreviewSR.sprite = null;
        //Move it out of the scene - Done in TowerPreviewScript
    }



    public void TowerMenuTowerSelectButtonOnClick(int towerIndex)
    {
        Debug.Log($"Selected Tower {towerIndex}");
        BuildManager.main.SetSelectedTower(towerIndex);
        //Check if player can afford the tower
        if (BuildManager.main.CanAffordSelectedTower())
        {
            BuildManager.main.EnableBuilding();
            ActivateTowerPreview();
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
        ResetTowerPreview();
    }

}
