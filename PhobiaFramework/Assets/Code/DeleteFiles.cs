using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using TMPro;
using System.Linq;
using System.Threading.Tasks;

// The script manages the deletion of files displayed in a grid layout.

public class DeleteFiles : MonoBehaviour
{
    DatabaseService dbService;
    public GameObject gridItemPrefab;
    public Transform gridParent;
    public Button deleteFilesButton;
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
        }
        else
        {
            Debug.LogError("GameObject with DatabaseService not found.");
        }

        files = new List<FileMetaData>();

        StartCoroutine(showFiles());

        /*deleteFilesButton.onClick.AddListener(() =>
        {

            
        });*/
    }

    public IEnumerator showFiles()
    {
        List<FileMetaData> newFilesList = new List<FileMetaData>();

        yield return dbService.getAllModelFileData((data) =>
        {
            newFilesList = data;
        });

        yield return dbService.getAll360Media((data) =>
        {
            newFilesList.AddRange(data);
        });

        yield return dbService.getAllSoundMedia((data) =>
        {
            newFilesList.AddRange(data);
        });

        yield return dbService.getAllSceneryFileData((data) =>
        {
            newFilesList.AddRange(data);
        });

        if (files.Count < newFilesList.Count)
        {
            int index = 0;
            foreach (var file in newFilesList)
            {
                yield return StartCoroutine(CreateGridItem(file, index));
                index++;
            }
            files = newFilesList;
        }
        else if (files.Count > newFilesList.Count)
        {
            reloadFiles();
        }
        else
        {
            Debug.Log("No more models found in the database.");
        }
    }

    public void reloadFiles()
    {
        foreach (Transform child in gridParent.transform)
        {
            // Destroy the child grid item
            Destroy(child.gameObject);
        }
        files = new List<FileMetaData>();
        StartCoroutine(showFiles());
    }

    public IEnumerator CreateGridItem(FileMetaData file, int index)
    {
        if (files.Any(x => x.filetype == file.filetype && x.pathToIcon == file.pathToIcon && x.path == file.path))
        {
            Debug.Log("Grid item already exists for file: " + file.filename);
            yield break; // Exit the function early if the grid item already exists
        }

        GameObject gridItem = Instantiate(gridItemPrefab, gridParent);

        GridLayoutGroup gridLayoutGroup = gridParent.GetComponent<GridLayoutGroup>();
        float cellSizeX = gridLayoutGroup.cellSize.x;
        float spacingX = gridLayoutGroup.spacing.x;
        int columnCount = Mathf.FloorToInt(gridParent.GetComponent<RectTransform>().rect.width / (cellSizeX + spacingX));

        gridItem.name = "GridItem" + index;

        Image iconImage = gridItem.transform.Find("IconImage").GetComponent<Image>();
        yield return StartCoroutine(LoadImageFromFirebase(file.pathToIcon, iconImage));

        TextMeshProUGUI nameText = gridItem.transform.Find("NameText").GetComponent<TextMeshProUGUI>();
        nameText.text = file.filename;

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
            dbService.deleteFile(file.filename, file.filetype, file);
            reloadFiles();
        });

        LayoutRebuilder.ForceRebuildLayoutImmediate(gridParent.GetComponent<RectTransform>());

        gridLayoutGroup.gameObject.SetActive(false);
        gridLayoutGroup.gameObject.SetActive(true);

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

}
