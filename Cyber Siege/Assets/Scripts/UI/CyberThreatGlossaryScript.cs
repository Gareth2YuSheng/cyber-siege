using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class CyberThreatGlossary : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject CyberThreatButtonPrefab;
    [SerializeField] private GameObject EnemyName;
    [SerializeField] private GameObject EnemyDescription;
    [SerializeField] private GameObject EnemyImage;


    [Header("Attributes")]
    [SerializeField] private Enemy[] enemies;

    // Referenced the LevelSelectionScreen
    private void Start()
    {
        // Fill first information
        fillInformationUp(0);

        for (int i = 0; i < enemies.Length; i++)
        {
            int currIndex = i;

            // Create the buttons
            GameObject selectionButton = Instantiate(CyberThreatButtonPrefab, gameObject.transform);

            Button button = selectionButton.GetComponentInChildren<Button>();

            // Set Cost Label
            TextMeshProUGUI label = button.GetComponentInChildren<TextMeshProUGUI>();
            label.text = enemies[i].enemyName;

            // Set Click Listener
            button.onClick.AddListener(() =>
            {
                Debug.Log("CLICK");
                // Send details to Enemy Preview
                fillInformationUp(currIndex);

            });
        }
    }

    public void fillInformationUp(int currIndex)
    {
        TextMeshProUGUI enemyNameText = EnemyName.GetComponent<TextMeshProUGUI>();
        enemyNameText.text = enemies[currIndex].enemyName;

        TextMeshProUGUI enemyDescriptionText = EnemyDescription.GetComponent<TextMeshProUGUI>();
        enemyDescriptionText.text = enemies[currIndex].enemyDescription;

        Image enemyImage = EnemyImage.GetComponent<Image>();
        enemyImage.sprite = enemies[currIndex].enemySprite;

        // Link not implemented for now
    }

    public void BackButtonOnClick()
    {
        SceneManager.LoadSceneAsync("MainMenu");
    }
}
