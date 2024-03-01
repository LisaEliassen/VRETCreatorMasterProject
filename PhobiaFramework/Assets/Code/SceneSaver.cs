using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using GLTFast.Export;
using System.Threading.Tasks;
using GLTFast.Logging;
using GLTFast;

public class SceneSaver : MonoBehaviour
{
    //[SerializeField]
    string path;
    UploadFiles uploadFiles;
    DatabaseService dbService;
    public GameObject databaseServiceObject;

    private string sceneName;
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
    }

    //public void ExportScene(string sceneName, string pathToTrigger, string triggerTransform, string triggerSize, string pathTo360Media, string pathToAudio, string[] pathsToScenery, string[] sceneryLocations, string[] scenerySizes)
    public void ExportScene()
    {
        Debug.Log(this.sceneName);
        Debug.Log(this.pathToTrigger);
        Debug.Log(this.triggerTransform);
        Debug.Log(this.triggerSize);
        Debug.Log(this.pathTo360Media);
        Debug.Log(this.pathToAudio);
        Debug.Log(this.pathsToScenery);
        Debug.Log(this.sceneryLocations);
        Debug.Log(this.scenerySizes);

        Trigger trigger = new Trigger(this.pathToTrigger, this.triggerTransform, this.triggerSize);

        dbService.addSceneData(this.sceneName, trigger, this.pathTo360Media, this.pathToAudio, this.sceneryObjects.ToArray());
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

    public void AddSceneryObject(SceneryObject sceneryObject)
    {
        this.sceneryObjects.Add(sceneryObject);
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
                dbService.addIcon(iconPath, fileName + "_icon", fileType, iconExtension);
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
