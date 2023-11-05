using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Collections;
using TMPro;
using System.Threading.Tasks;

public class ShowAllModels : MonoBehaviour
{
    DatabaseService dbService;
    LoadGlb loadGlbScript;
    public GameObject gridItemPrefab;
    public Transform gridParent;
    public Button showModelsButton;
    public Button backButton;
    public GameObject EditSceneUI;
    public GameObject ModelUI;
    List<FileMetaData> files;

    // Start is called before the first frame update
    void Start()
    {
        // Find the GameObject with the DatabaseService script
        GameObject databaseServiceObject = GameObject.Find("DatabaseService");

        // Check if the GameObject was found
        if (databaseServiceObject != null)
        {
            // Get the DatabaseService component from the found GameObject
            dbService = databaseServiceObject.GetComponent<DatabaseService>();


            loadGlbScript = databaseServiceObject.GetComponent<LoadGlb>();
        }
        else
        {
            Debug.LogError("GameObject with DatabaseService not found.");
        }

        files = new List<FileMetaData>();

        showModelsButton.onClick.AddListener(() =>
        {
            StartCoroutine(FetchModels());
        });

        backButton.onClick.AddListener(() =>
        {
            EditSceneUI.SetActive(true);
            ModelUI.SetActive(false);
        });
    }

    public IEnumerator FetchModels()
    {
        EditSceneUI.SetActive(false);
        ModelUI.SetActive(true);

        if (files.Count == 0) 
        { 
            yield return dbService.getAllModelFileData((data) =>
            {
                files = data;
            });

            if (files.Count > 0)
            {
                int index = 0;
                foreach (var file in files)
                {
                    yield return StartCoroutine(CreateGridItem(file.filename, file.pathToIcon, file.path, index));
                    index++;
                }
            }
            else
            {
                Debug.LogError("No models found in the database.");
            }
        }        
    }

    public IEnumerator CreateGridItem(string modelName, string modelIconPath, string modelStoragePath, int index)
    {
        GameObject gridItem = Instantiate(gridItemPrefab, gridParent);

        GridLayoutGroup gridLayoutGroup = gridParent.GetComponent<GridLayoutGroup>();
        float cellSizeX = gridLayoutGroup.cellSize.x;
        float spacingX = gridLayoutGroup.spacing.x;
        int columnCount = Mathf.FloorToInt(gridParent.GetComponent<RectTransform>().rect.width / (cellSizeX + spacingX));

        gridItem.name = "GridItem" + index;

        Image iconImage = gridItem.transform.Find("IconImage").GetComponent<Image>();
        yield return StartCoroutine(LoadImageFromFirebase(modelIconPath, iconImage));

        TextMeshProUGUI nameText = gridItem.transform.Find("NameText").GetComponent<TextMeshProUGUI>();
        nameText.text = modelName;

        int row = index / columnCount;
        int column = index % columnCount;

        // Adjust the anchored position to include the top padding
        float paddingTop = 80; // Adjust this value as needed
        float adjustedPosY = -(cellSizeX + spacingX) * row - paddingTop;

        RectTransform rectTransform = gridItem.GetComponent<RectTransform>();
        rectTransform.anchorMin = new Vector2(0, 1);
        rectTransform.anchorMax = new Vector2(0, 1);
        rectTransform.pivot = new Vector2(0, 1);
        rectTransform.anchoredPosition = new Vector2((cellSizeX + spacingX) * column, adjustedPosY);
        //rectTransform.anchoredPosition = new Vector2((cellSizeX + spacingX) * column, -(cellSizeX + spacingX) * row);

        Button button = gridItem.AddComponent<Button>();

        // Add an onclick listener for the grid item to load the model from Firebase Storage
        button.onClick.AddListener(() =>
        {
            loadGlbScript.SpawnObject(modelName, modelStoragePath);
            EditSceneUI.SetActive(true);
            ModelUI.SetActive(false);
            Debug.Log("Button for model " + modelName + " was clicked!");
        });
    }

    public IEnumerator LoadImageFromFirebase(string modelIconPath, Image iconImage)
    {
        Task<Texture2D> loadTextureTask = LoadTextureAsync(modelIconPath);
        yield return new WaitUntil(() => loadTextureTask.IsCompleted);

        if (loadTextureTask.IsFaulted || loadTextureTask.IsCanceled)
        {
            Debug.Log("Error loading texture from Firebase.");
        }
        else
        {
            Texture2D texture = loadTextureTask.Result;
            if (texture != null)
            {
                iconImage.sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
            }
        }
    }

    private async Task<Texture2D> LoadTextureAsync(string modelIconPath)
    {
        string downloadUrl = await dbService.GetDownloadURL(modelIconPath);
        if (!string.IsNullOrEmpty(downloadUrl))
        {
            byte[] imageData = await dbService.getFile(downloadUrl);
            if (imageData != null && imageData.Length > 0)
            {
                Texture2D texture = new Texture2D(2, 2);
                texture.LoadImage(imageData);
                return texture;
            }
        }

        return null;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
