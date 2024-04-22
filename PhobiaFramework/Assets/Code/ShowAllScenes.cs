using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Collections;
using TMPro;
using System.Linq;
using System.Threading.Tasks;
using System.Globalization;

public class ShowAllScenes : MonoBehaviour
{
    DatabaseService dbService;
    LoadGlb loadGlbScript;
    MediaManager mediaManager;
    SoundManager soundManager;
    SceneSaver sceneSaver;
    public Button yesButton;
    public Button noButton;
    public Button refreshButton;
    public GameObject databaseServiceObject;
    public GameObject gridItemPrefab;
    public Transform gridParent;
    public ScrollRect scrollView;
    public Button showScenesButton;
    public GameObject EditSceneUI;
    public GameObject ScenesUI;
    public GameObject ScenesUICanvas;
    public GameObject LoadingUI;
    List<SceneMetaData> files;

    // Start is called before the first frame update
    void Start()
    {
        // Check if the GameObject was found
        if (databaseServiceObject != null)
        {
            // Get the DatabaseService component from the found GameObject
            dbService = databaseServiceObject.GetComponent<DatabaseService>();

            loadGlbScript = databaseServiceObject.GetComponent<LoadGlb>();
            mediaManager = databaseServiceObject.GetComponent<MediaManager>();
            soundManager = databaseServiceObject.GetComponent<SoundManager>();
            sceneSaver = databaseServiceObject.GetComponent<SceneSaver>();
        }
        else
        {
            Debug.LogError("GameObject with DatabaseService not found.");
        }

        files = new List<SceneMetaData>();

        yesButton.onClick.AddListener(() =>
        {
            StartCoroutine(FetchScenes());
        });
        noButton.onClick.AddListener(() =>
        {
            StartCoroutine(FetchScenes());
        });

        refreshButton.onClick.AddListener(() =>
        {
            reload();
        });
    }

    public IEnumerator FetchScenes()
    {
        List<SceneMetaData> newFilesList = new List<SceneMetaData>();

        Debug.Log("before getting scenes");

        yield return StartCoroutine(dbService.getAllScenesFileData((data) =>
        {
            newFilesList = data;
        }));
        Debug.Log("Number of scenes: " + newFilesList.Count);

        if (files.Count < newFilesList.Count)
        {
            int index = 1;
            foreach (var file in newFilesList)
            {
                StartCoroutine(CreateGridItem(file.sceneName, file.pathToSceneIcon, file.triggers, file.pathTo360Media, file.pathToAudio, file.scenery, index));
                index++;
            }
            files = newFilesList;
        }
        else if (files.Count > newFilesList.Count)
        {
            reload();
        }
        else
        {
            Debug.Log("No more scenes found in the database.");
        }

        /*yield return dbService.getAllScenesFileData((data) =>
        {
            Debug.Log("test");

            newFilesList = data;
        });

        Debug.Log(newFilesList.Count);

        if (files.Count < newFilesList.Count)
        {
            int index = 1;
            foreach (var file in newFilesList)
            {
                yield return StartCoroutine(CreateGridItem(file.sceneName, file.pathToSceneIcon, file.trigger, file.pathTo360Media, file.pathToAudio, file.scenery, index));
                index++;
            }
            files = newFilesList;
        }
        else if (files.Count > newFilesList.Count)
        {
            reload();
        }
        else
        {
            Debug.Log("No more scenes found in the database.");
        }*/
    }

    public void reload()
    {

        Debug.Log("Reloading scenes");
        foreach (Transform child in gridParent.transform)
        {
            // Destroy the child grid item
            Destroy(child.gameObject);
        }
        files = new List<SceneMetaData>();
        StartCoroutine(FetchScenes());
    }

    public IEnumerator CreateGridItem(string sceneName, string pathToSceneIcon, Trigger[] triggers, string pathTo360Media, string pathToAudio, SceneryObject[] scenery, int index)
    {
        if (files.Any(x => x.sceneName == sceneName && x.pathToSceneIcon == pathToSceneIcon))
        {
            Debug.Log("Grid item already exists for file: " + sceneName);
            yield break; // Exit the function early if the grid item already exists
        }

        GameObject gridItem = Instantiate(gridItemPrefab, gridParent);

        GridLayoutGroup gridLayoutGroup = gridParent.GetComponent<GridLayoutGroup>();
        float cellSizeX = gridLayoutGroup.cellSize.x;
        float spacingX = gridLayoutGroup.spacing.x;
        int columnCount = Mathf.FloorToInt(gridParent.GetComponent<RectTransform>().rect.width / (cellSizeX + spacingX));

        gridItem.name = "GridItem" + index;

        Image iconImage = gridItem.transform.Find("IconImage").GetComponent<Image>();
        yield return StartCoroutine(LoadImageFromFirebase(pathToSceneIcon, iconImage));

        TextMeshProUGUI nameText = gridItem.transform.Find("NameText").GetComponent<TextMeshProUGUI>();
        nameText.text = sceneName;

        int row = index / columnCount;
        int column = index % columnCount;

        // Adjust the anchored position to include the top padding
        float paddingTop = 100; // Adjust this value as needed
        float adjustedPosY = -(cellSizeX + spacingX) * row - paddingTop;

        RectTransform rectTransform = gridItem.GetComponent<RectTransform>();
        rectTransform.anchorMin = new Vector2(0, 1);
        rectTransform.anchorMax = new Vector2(0, 1);
        rectTransform.pivot = new Vector2(0, 1);
        rectTransform.anchoredPosition = new Vector2((cellSizeX + spacingX) * column, adjustedPosY);
        //rectTransform.anchoredPosition = new Vector2((cellSizeX + spacingX) * column, -(cellSizeX + spacingX) * row);

        Button button = gridItem.AddComponent<Button>();

        // Add an onclick listener for the grid item to load the model from Firebase Storage
        button.onClick.AddListener(async () =>
        {
            loadGlbScript.ResetScene();

            ScenesUICanvas.GetComponent<GraphicRaycaster>().enabled = false;
            LoadingUI.SetActive(true);

            foreach (Trigger trigger in triggers)
            {
                if (trigger != null)
                {
                    if (!System.String.IsNullOrEmpty(trigger.path))
                    {
                        Vector3 position;
                        Quaternion rotation;

                        string[] positionComponents = trigger.position.Split(',');
                        string[] rotationComponents = trigger.rotation.Split(',');

                        position = ParseVector3(positionComponents[0], positionComponents[1], positionComponents[2]);
                        rotation = ParseQuaternion(rotationComponents[0], rotationComponents[1], rotationComponents[2], rotationComponents[3]);

                        int size = 2;
                        if (int.TryParse(trigger.size, out _))
                        {
                            size = int.Parse(trigger.size);
                        }

                        if (trigger.isCopy == "0")
                        {
                            await loadGlbScript.SpawnObject("Trigger", trigger.path, position, rotation, size);
                        }
                        else if (trigger.isCopy == "1")
                        {
                            await loadGlbScript.MakeCopy(position, rotation);
                        }
                        else
                        {
                            Debug.Log("Something went wrong!");
                        }
                    }
                }
                else
                {
                    Debug.Log("trigger is null");
                }

            }

            if (!System.String.IsNullOrEmpty(pathTo360Media))
            {
                string mediaDownloadUrl = await dbService.GetDownloadURL(pathTo360Media);

                if (mediaManager.IsVideoFile(pathTo360Media))
                {
                    await mediaManager.HandleImageSelected(mediaDownloadUrl);
                }
                else if (mediaManager.IsImageFile(pathTo360Media))
                {
                    await mediaManager.HandleImageSelected(mediaDownloadUrl);
                }
            }

            if (!System.String.IsNullOrEmpty(pathToAudio))
            {
                string audioDownloadUrl = await dbService.GetDownloadURL(pathToAudio);
                await soundManager.HandleSoundSelected(audioDownloadUrl);
            }

            foreach (SceneryObject sceneryObj in scenery)
            {
                Vector3 position;
                Quaternion rotation;

                string[] positionComponents = sceneryObj.position.Split(',');
                string[] rotationComponents = sceneryObj.rotation.Split(',');

                position = ParseVector3(positionComponents[0], positionComponents[1], positionComponents[2]);
                rotation = ParseQuaternion(rotationComponents[0], rotationComponents[1], rotationComponents[2], rotationComponents[3]);

                int size = 2;
                if (int.TryParse(sceneryObj.size, out _))
                {
                    size = int.Parse(sceneryObj.size);
                }

                await loadGlbScript.SpawnSceneryObject(sceneryObj.name, sceneryObj.path, position, rotation, size);
            }

            EditSceneUI.SetActive(true);
            ScenesUI.SetActive(false);
            Debug.Log("Button for scene " + sceneName + " was clicked!");

            ScenesUICanvas.GetComponent<GraphicRaycaster>().enabled = true;
            LoadingUI.SetActive(false);
        });

        LayoutRebuilder.ForceRebuildLayoutImmediate(gridParent.GetComponent<RectTransform>());
        //scrollView.verticalNormalizedPosition = 0f; // scroll to bottom after adding an item

        gridLayoutGroup.gameObject.SetActive(false);
        gridLayoutGroup.gameObject.SetActive(true);
    }

    private static Vector3 ParseVector3(string xStr, string yStr, string zStr)
    {
        float x = float.Parse(xStr.Trim('('), CultureInfo.InvariantCulture);
        float y = float.Parse(yStr.Trim(), CultureInfo.InvariantCulture);
        float z = float.Parse(zStr.Trim(')'), CultureInfo.InvariantCulture);

        return new Vector3(x, y, z);
    }

    private static Quaternion ParseQuaternion(string xStr, string yStr, string zStr, string wStr)
    {
        float x = float.Parse(xStr.Trim('('), CultureInfo.InvariantCulture);
        float y = float.Parse(yStr.Trim(), CultureInfo.InvariantCulture);
        float z = float.Parse(zStr.Trim(), CultureInfo.InvariantCulture);
        float w = float.Parse(wStr.Trim(')'), CultureInfo.InvariantCulture);

        return new Quaternion(x, y, z, w);
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
