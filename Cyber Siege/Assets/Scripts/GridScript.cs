using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class GridScript : MonoBehaviour
{
    public GameObject boxPrefab; // Assign your UI box prefab
    public Transform gridParent; // Assign a GameObject with a GridLayoutGroup
    public int rows = 5;
    public int cols = 5;
    public int numberOfBoxes = 15;

    private List<GameObject> spawnedBoxes = new List<GameObject>();

    void Start()
    {
        SpawnGrid();
    }

    void SpawnGrid()
    {
        int totalCells = rows * cols;
        numberOfBoxes = Mathf.Clamp(numberOfBoxes, 2, totalCells);

        HashSet<int> usedIndices = new HashSet<int>();
        while (usedIndices.Count < numberOfBoxes)
        {
            int randIndex = Random.Range(0, totalCells);
            usedIndices.Add(randIndex);
        }

        for (int i = 0; i < totalCells; i++)
        {
            GameObject newBox = Instantiate(boxPrefab, gridParent);
            newBox.SetActive(usedIndices.Contains(i));
            spawnedBoxes.Add(newBox);
        }

        // Assign entry and exit
        List<GameObject> activeBoxes = spawnedBoxes.FindAll(b => b.activeSelf);
        if (activeBoxes.Count >= 2)
        {
            GameObject entry = activeBoxes[Random.Range(0, activeBoxes.Count)];
            GameObject exit;
            do
            {
                exit = activeBoxes[Random.Range(0, activeBoxes.Count)];
            } while (exit == entry);

            entry.GetComponent<Image>().color = Color.green; // Entry box
            exit.GetComponent<Image>().color = Color.red;   // Exit box
        }
    }
}