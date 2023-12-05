using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Collections;
using TMPro;
using System.Linq;
using System.Threading.Tasks;

public class ShowAll360Media : MonoBehaviour
{
    DatabaseService dbService;
    MediaManager mediaManager;
    public GameObject databaseServiceObject;
    public GameObject gridItemPrefab360;
    public Transform gridParent;
    public Button addMediaButton;
    public GameObject EditSceneUI;
    public GameObject MediaUI;
    List<FileMetaData> files;

    // Start is called before the first frame update
    void Start()
    {
        // Check if the GameObject was found
        if (databaseServiceObject != null)
        {
            // Get the DatabaseService component from the found GameObject
            dbService = databaseServiceObject.GetComponent<DatabaseService>();


            mediaManager = databaseServiceObject.GetComponent<MediaManager>();
        }
        else
        {
            Debug.LogError("GameObject with DatabaseService not found.");
        }

        files = new List<FileMetaData>();

        addMediaButton.onClick.AddListener(() =>
        {
            StartCoroutine(FetchMedia());
        });
    }

    public IEnumerator FetchMedia()
    {
        List<FileMetaData> newFilesList = new List<FileMetaData>();

        yield return dbService.getAll360Media((data) =>
        {
            newFilesList = data;
        });

        if (files.Count < newFilesList.Count)
        {
            int index = 0;
            foreach (var file in newFilesList)
            {
                yield return StartCoroutine(CreateGridItem(file.filename, file.filetype, file.pathToIcon, file.path, index));
                index++;
            }
            files = newFilesList;
        }
        else if (files.Count > newFilesList.Count)
        {
            reloadMedia();
        }
        else
        {
            Debug.Log("No more files found in the database.");
        }
    }

    public void reloadMedia()
    {
        foreach (Transform child in gridParent.transform)
        {
            // Destroy the child grid item
            Destroy(child.gameObject);
        }
        files = new List<FileMetaData>();
        StartCoroutine(FetchMedia());
    }

    public IEnumerator CreateGridItem(string filename, string filetype, string iconPath, string storagePath, int index)
    {
        if (files.Any(x => x.filetype == filetype && x.pathToIcon == iconPath && x.path == storagePath))
        {
            Debug.Log("Grid item already exists for file: " + filename);
            yield break; // Exit the function early if the grid item already exists
        }

        GameObject gridItem = Instantiate(gridItemPrefab360, gridParent);

        GridLayoutGroup gridLayoutGroup = gridParent.GetComponent<GridLayoutGroup>();
        float cellSizeX = gridLayoutGroup.cellSize.x;
        float spacingX = gridLayoutGroup.spacing.x;
        int columnCount = Mathf.FloorToInt(gridParent.GetComponent<RectTransform>().rect.width / (cellSizeX + spacingX));

        gridItem.name = "GridItem" + index;

        Image iconImage = gridItem.transform.Find("IconImage").GetComponent<Image>();
        yield return StartCoroutine(LoadImageFromFirebase(iconPath, iconImage));

        TextMeshProUGUI nameText = gridItem.transform.Find("NameText").GetComponent<TextMeshProUGUI>();
        nameText.text = filename;

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

        Image fileTypeImage = gridItem.transform.Find("TypeImage").GetComponent<Image>();

        Texture2D texture = null;

        if (filetype == "360 image")
        {
            //texture = Resources.Load<Texture2D>("image-icon.png");
            yield return StartCoroutine(LoadImageFileType("image-icon", fileTypeImage));

        }
        else if (filetype == "360 video")
        {
            //texture = Resources.Load<Texture2D>("video-icon.png");
            yield return StartCoroutine(LoadImageFileType("video-icon", fileTypeImage));
        }

        if (texture != null)
        {
            Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
            fileTypeImage.sprite = sprite;
        }
        else
        {
            Debug.Log("texture is null");
        }

        Button button = gridItem.AddComponent<Button>();

        // Add an onclick listener for the grid item to load the model from Firebase Storage
        button.onClick.AddListener(async () =>
        {
            string downloadUrl = await dbService.GetDownloadURL(storagePath);
            if (downloadUrl != null)
            {
                if (filetype == "360 image")
                {
                    await mediaManager.HandleImageSelected(downloadUrl);
                }
                else if (filetype == "360 video")
                {
                    await mediaManager.HandleVideoSelected(downloadUrl);
                }               

                EditSceneUI.SetActive(true);
                MediaUI.SetActive(false);
            }
            else
            {
                Debug.Log("Download url is null!");
            }
            Debug.Log("Button for file " + filename + " was clicked!");
        });

        gridLayoutGroup.gameObject.SetActive(false);
        gridLayoutGroup.gameObject.SetActive(true);
    }

    public IEnumerator LoadImageFileType(string resourceName, Image image)
    {
        Task<Texture2D> loadTextureTask = LoadTextureFromResourcesAsync(resourceName);
        yield return new WaitUntil(() => loadTextureTask.IsCompleted);

        if (loadTextureTask.IsFaulted || loadTextureTask.IsCanceled)
        {
            Debug.Log("Error loading texture.");
        }
        else
        {
            Texture2D texture = loadTextureTask.Result;
            if (texture != null)
            {
                image.sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
            }
        }
    }

    private async Task<Texture2D> LoadTextureFromResourcesAsync(string resourceName)
    {
        ResourceRequest request = Resources.LoadAsync<Texture2D>(resourceName);

        while (!request.isDone)
        {
            await Task.Delay(10); // Add a small delay to avoid blocking the main thread
        }

        if (request.asset != null)
        {
            return (Texture2D)request.asset;
        }
        else
        {
            Debug.LogError("Failed to load texture from resources: " + resourceName);
            return null;
        }
    }


    public IEnumerator LoadImageFromFirebase(string iconPath, Image iconImage)
    {
        Task<Texture2D> loadTextureTask = LoadTextureAsync(iconPath);
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

    private async Task<Texture2D> LoadTextureAsync(string iconPath)
    {
        string downloadUrl = await dbService.GetDownloadURL(iconPath);
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
