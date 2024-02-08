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
    string path = "C://Users//Student/Desktop/export.glb";
    UploadFiles uploadFiles;
    DatabaseService dbService;
    public GameObject databaseServiceObject;

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
        bool success = await export.SaveToFileAndDispose(path);

        if (!success)
        {
            Debug.LogError("Something went wrong exporting a glTF");
        }
    }

    public async void AdvancedExport()
    {

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
        var success = await export.SaveToFileAndDispose(path);

        if (!success)
        {
            Debug.LogError("Something went wrong exporting a glTF");
            // Log all exporter messages
            logger.LogAll();
        }
        else
        {
            string iconPath = "C://Users//Student/Downloads/crowAnimated_icon.png";
            string fileName = "export_test";
            string fileType = "Model";

            string fileExtension = Path.GetExtension(path);
            string iconExtension = Path.GetExtension(iconPath);

            bool exportsuccess = dbService.addFile(path, fileName, "Model", fileExtension);
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
