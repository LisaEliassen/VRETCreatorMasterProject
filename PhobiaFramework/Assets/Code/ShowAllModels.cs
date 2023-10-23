using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShowAllModels : MonoBehaviour
{
    DatabaseService dbService;
    public GameObject gridItemPrefab;
    public Transform gridParent;

    // Start is called before the first frame update
    void Start()
    {
        // Find the GameObject with the DatabaseService script
        GameObject databaseServiceObject = GameObject.Find("DatabaseService");
        //dbService = new DatabaseService("Firebase");

        // Check if the GameObject was found
        if (databaseServiceObject != null)
        {
            // Get the DatabaseService component from the found GameObject
            dbService = databaseServiceObject.GetComponent<DatabaseService>();
        }
        else
        {
            Debug.LogError("GameObject with DatabaseService not found.");
        }
    }


    private void CreateGridItem(string modelName, string modelIconPath, string modelStoragePath, int index)
    {
        /*GameObject gridItem = Instantiate(gridItemPrefab, gridParent);
        gridItem.name = "GridItem" + index;

        Image iconImage = gridItem.transform.Find("IconImage").GetComponent<Image>();
        StartCoroutine(LoadImageFromFirebase(modelIconPath, iconImage));

        TextMeshProUGUI nameText = gridItem.transform.Find("NameText").GetComponent<TextMeshProUGUI>();
        nameText.text = modelName;

        // Add an onclick listener for the grid item to load the model from Firebase Storage
        Button button = gridItem.GetComponent<Button>();
        button.onClick.AddListener(() =>
        {
            LoadModelFromFirebase(modelStoragePath);
        });*/
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
