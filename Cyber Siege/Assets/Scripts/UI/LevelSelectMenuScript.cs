using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class LevelSelectMenuScript : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject levelSelectButtonPrefab;
    [SerializeField] private GameObject nextButtonObject;
    [SerializeField] private GameObject previousButtonObject;


    [Header("Attributes")]
    [SerializeField] private Level[] levels;

    private int pageIndex = 0;
    private void Start()
    {
        Button nextButton = nextButtonObject.GetComponent<Button>();
        Button previousButton = previousButtonObject.GetComponent<Button>();
        nextButtonObject.SetActive(true);

        // Populate the level select section

        populateList();

        nextButton.onClick.RemoveAllListeners();
        nextButton.onClick.AddListener(() => { OnNextPage(); });
        previousButton.onClick.RemoveAllListeners();
        previousButton.onClick.AddListener(() => { OnPreviousPage(); });
    }

    public void populateList()
    {
        DestroyPage();
        // Take note, pageIndex starts from 0.
        // Deny any page below 6 * pageIndex
        for (int i = pageIndex * 6; i < levels.Length; i++)
        {
            int currIndex = i;
            if (i < (pageIndex + 1) * 6)
            {
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
    }

    public void DestroyPage()
    {
        GameObject[] pageItems = GameObject.FindGameObjectsWithTag("Pagination");
        foreach (GameObject item in pageItems)
        {
            Destroy(item);
        }

    }

    public void OnNextPage()
    {
        nextButtonObject.SetActive(false);
        previousButtonObject.SetActive(true);
        pageIndex += 1;
        populateList();
    }

    public void OnPreviousPage()
    {
        nextButtonObject.SetActive(true);
        previousButtonObject.SetActive(false);
        pageIndex -= 1;
        populateList();
    }

    public void BackButtonOnClick()
    {
        SceneManager.LoadSceneAsync("MainMenu");
    }
}
