using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class LevelSelectMenuScript : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject levelSelectButtonPrefab;

    [Header("Attributes")]
    [SerializeField] private Level[] levels;


    private void Start()
    {
        // Populate the level select section
        for (int i = 0; i < levels.Length; i++)
        {
            int currIndex = i;

            // Create the buttons
            GameObject selectionButton = Instantiate(levelSelectButtonPrefab, gameObject.transform);

            Button button = selectionButton.GetComponentInChildren<Button>();

            // Set the button image
            Image buttonImage = button.GetComponent<Image>();
            buttonImage.sprite = levels[i].levelPreview;

            // Set Cost Label
            TextMeshProUGUI label = button.GetComponentInChildren<TextMeshProUGUI>();
            label.text = levels[i].levelName;

            // Set Click Listener
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(() => { SceneManager.LoadSceneAsync(levels[currIndex].sceneName); });
        }
    }

    public void BackButtonOnClick()
    {
        SceneManager.LoadSceneAsync("MainMenu");
    }
}
