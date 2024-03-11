using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using GLTFast.Export;
using System.Threading.Tasks;
using GLTFast.Logging;
using GLTFast;

public class SceneSaver : MonoBehaviour
{
    string path;
    UploadFiles uploadFiles;
    DatabaseService dbService;
    public GameObject databaseServiceObject;
    public Button confirmButton;
    public Button saveSceneButton;
    public Button cancelButton;
    public TMP_InputField sceneNameInput;
    public TextMeshProUGUI message;
    public TextMeshProUGUI warningOrErrorMessage;
    public Camera captureCamera;

    public GameObject SceneNameUI;

    private string sceneName = string.Empty;
    private string pathToTrigger;
    private string triggerTransform;
    private string triggerSize;
    private string pathTo360Media;
    private string pathToAudio;
    private List<SceneryObject> sceneryObjects = new List<SceneryObject>();
    private string[] pathsToScenery;
    private string[] sceneryLocations;
    private string[] scenerySizes;
    List<string> test = new List<string>();

    Dictionary<string, GameObject> objects;

    void Awake()
    {
         path = Application.dataPath + "/Assets";
    }

    void Start()
    {
        if (databaseServiceObject != null)
        {
            // Get the DatabaseService component from the found GameObject
            dbService = databaseServiceObject.GetComponent<DatabaseService>();
        }
        else
        {
            Debug.LogError("GameObject with DatabaseService not found.");
        }

        confirmButton.onClick.AddListener(ExportScene);

        saveSceneButton.onClick.AddListener(() =>
        {
            TakeScreenShotAndSave();
            SceneNameUI.SetActive(true);
        });

        cancelButton.onClick.AddListener(() =>
        {
            DeleteScreenshotFile("screenshot.png");
            SceneNameUI.SetActive(false);
        });

        sceneNameInput.onValueChanged.AddListener((x) => checkInput(sceneNameInput.text));

        objects = new Dictionary<string, GameObject>();
    }

    public void checkInput(string input)
    {
        if (!string.IsNullOrEmpty(input))
        {
            warningOrErrorMessage.text = "";
            confirmButton.interactable = true;
            this.sceneName = input;
        }
    }

    //public void ExportScene(string sceneName, string pathToTrigger, string triggerTransform, string triggerSize, string pathTo360Media, string pathToAudio, string[] pathsToScenery, string[] sceneryLocations, string[] scenerySizes)
    public async void ExportScene()
    {
        if (!string.IsNullOrEmpty(sceneNameInput.text))
        {
            warningOrErrorMessage.text = "";

            Trigger trigger = new Trigger(this.pathToTrigger, this.triggerTransform, this.triggerSize);
            
            await dbService.addIcon(Path.Combine(Application.persistentDataPath, "screenshot.png"), sceneName + "_icon", "Scene", Path.GetExtension("screenshot.png"));
            bool success = await dbService.addSceneData(sceneNameInput.text, trigger, this.pathTo360Media, this.pathToAudio, this.sceneryObjects.ToArray());

            if (success)
            {
                Debug.Log("Scene exported!");
                DeleteScreenshotFile("screenshot.png");
            }
            else
            {
                Debug.Log("Scene could not be exported!");
                DeleteScreenshotFile("screenshot.png");
            }

            SceneNameUI.SetActive(false);
        }
        else
        {
            warningOrErrorMessage.text = "Name cannot be empty!";
        }
    }

    public void TakeScreenShotAndSave()
    {
        // Remember the original target texture of the camera
        RenderTexture originalTargetTexture = captureCamera.targetTexture;

        // Create a RenderTexture with the dimensions of the screen
        RenderTexture renderTexture = new RenderTexture(Screen.width, Screen.height, 24);

        // Set the camera's target texture to the RenderTexture
        captureCamera.targetTexture = renderTexture;

        // Render the entire screen to the RenderTexture
        captureCamera.Render();

        // Create a Texture2D to read the RenderTexture data
        Texture2D screenshotTexture = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);

        // Read the RenderTexture data into the Texture2D
        RenderTexture.active = renderTexture;
        screenshotTexture.ReadPixels(new Rect(0, 0, renderTexture.width, renderTexture.height), 0, 0);
        screenshotTexture.Apply();

        // Clean up
        RenderTexture.active = null;
        captureCamera.targetTexture = originalTargetTexture; // Restore original target texture
        Destroy(renderTexture);

        // Save the screenshot texture to a file
        SaveTextureToFile(screenshotTexture, "screenshot.png");

        // Destroy the screenshot texture
        Destroy(screenshotTexture);
    }

    // Method to save a Texture2D as a PNG file
    private void SaveTextureToFile(Texture2D texture, string fileName)
    {
        byte[] bytes = texture.EncodeToPNG();
        string filePath = Path.Combine(Application.persistentDataPath, fileName);
        File.WriteAllBytes(filePath, bytes);
        Debug.Log("Screenshot saved as: " + filePath);
    }

    private void DeleteScreenshotFile(string fileName)
    {
        string filePath = Path.Combine(Application.persistentDataPath, fileName);
        if (File.Exists(filePath))
        {
            File.Delete(filePath);
            Debug.Log("Screenshot file deleted: " + filePath);
        }
        else
        {
            Debug.LogWarning("Screenshot file not found: " + filePath);
        }
    }

    public void SetSceneName(string sceneName)
    {
        this.sceneName = sceneName;
    }

    public void SetPathToTrigger(string pathToTrigger)
    {
        this.pathToTrigger = pathToTrigger;
    }

    public void SetTriggerTransform(string triggerTransform)
    {
        this.triggerTransform = triggerTransform;
    }

    public void SetTriggerSize(string triggerSize)
    {
        this.triggerSize = triggerSize;
    }

    public void SetPathTo360Media(string pathTo360Media)
    {
        this.pathTo360Media = pathTo360Media;
    }

    public void SetPathToAudio(string pathToAudio)
    {
        this.pathToAudio = pathToAudio;
    }

    public void AddSceneryObject(GameObject obj, SceneryObject sceneryObject)
    {
        this.sceneryObjects.Add(sceneryObject);
        this.objects[sceneryObject.path] = obj;
    }

    public void RemoveObject(GameObject obj)
    {
        foreach (KeyValuePair<string, GameObject> pair in this.objects)
        {
            if (pair.Value == obj)
            {
                objects.Remove(pair.Key);
            }
        }
    }

    public void SetPathsToScenery(string[] pathsToScenery)
    {
        this.pathsToScenery = pathsToScenery;
    }

    public void AddPathToScenery(string path)
    {
        this.pathsToScenery[this.pathsToScenery.Length] = path;
    }

    public void SetSceneryLocations(string[] sceneryLocations)
    {
        this.sceneryLocations = sceneryLocations;
    }

    public void SetScenerySizes(string[] scenerySizes)
    {
        this.scenerySizes = scenerySizes;
    }

    public async void SimpleExport()
    {
        if (databaseServiceObject != null)
        {
            // Get the DatabaseService component from the found GameObject
            dbService = databaseServiceObject.GetComponent<DatabaseService>();
        }
        if (dbService != null)
        {
            Debug.Log("Dbservice is null!");
        }
        if (databaseServiceObject != null)
        {
            uploadFiles = databaseServiceObject.GetComponent<UploadFiles>();
        }

        // Example of gathering GameObjects to be exported (recursively)
        var rootLevelNodes = GameObject.FindGameObjectsWithTag("Export");

        // GameObjectExport lets you create glTFs from GameObject hierarchies
        var export = new GameObjectExport();

        // Add a scene
        export.AddScene(rootLevelNodes);

        // Async glTF export
        bool success = await export.SaveToFileAndDispose(path+"/export.glb");

        if (!success)
        {
            Debug.LogError("Something went wrong exporting a glTF");
        }
    }

    public async void AdvancedExport()
    {
        if (databaseServiceObject != null)
        {
            // Get the DatabaseService component from the found GameObject
            dbService = databaseServiceObject.GetComponent<DatabaseService>();
        }
        if (dbService != null)
        {
            Debug.Log("Dbservice is null!");
        }


        // CollectingLogger lets you programmatically go through
        // errors and warnings the export raised
        var logger = new CollectingLogger();

        // ExportSettings and GameObjectExportSettings allow you to configure the export
        // Check their respective source for details

        // ExportSettings provides generic export settings
        var exportSettings = new ExportSettings
        {
            Format = GltfFormat.Binary,
            FileConflictResolution = FileConflictResolution.Overwrite,
            // Export everything except cameras or animation
            ComponentMask = ~(ComponentType.Camera),
            // Boost light intensities
            LightIntensityFactor = 100f,
        };

        // GameObjectExportSettings provides settings specific to a GameObject/Component based hierarchy
        var gameObjectExportSettings = new GameObjectExportSettings
        {
            // Include inactive GameObjects in export
            OnlyActiveInHierarchy = false,
            // Also export disabled components
            DisabledComponents = true,
            // Only export GameObjects on certain layers
            LayerMask = LayerMask.GetMask("Default", "MyCustomLayer"),
        };

        // GameObjectExport lets you create glTFs from GameObject hierarchies
        var export = new GameObjectExport(exportSettings, gameObjectExportSettings, logger: logger);

        // Example of gathering GameObjects to be exported (recursively)
        var rootLevelNodes = GameObject.FindGameObjectsWithTag("Export");

        // Add a scene
        export.AddScene(rootLevelNodes, "My new glTF scene");

        // Async glTF export
        var success = await export.SaveToFileAndDispose(path + "/export.glb");

        if (!success)
        {
            Debug.LogError("Something went wrong exporting a glTF");
            // Log all exporter messages
            logger.LogAll();
        }
        else
        {
            string iconPath = path + "/export_test_icon.png";
            string fileName = "export_test";
            string fileType = "Model";

            string fileExtension = Path.GetExtension(path + "/export.glb");
            string iconExtension = Path.GetExtension(iconPath);

            bool exportsuccess = dbService.addFile(path + "/export.glb", fileName, "Model", fileExtension);
            if (exportsuccess)
            {
                //dbService.addIcon(iconPath, fileName + "_icon", fileType, iconExtension);
                dbService.addFileData(fileName, fileType, fileExtension, iconExtension);
                Debug.Log("File exported to Firebase!");
            }
            else
            {
                Debug.Log("File could not be exported to Firebase!");
            }
        }
    }

}
