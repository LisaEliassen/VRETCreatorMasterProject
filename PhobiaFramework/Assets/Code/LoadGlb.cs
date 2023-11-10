using System;
using System.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using GLTFast;
using GLTFast.Schema;
using UnityEngine.Networking;

public class LoadGlb : MonoBehaviour
{
    GameObject trigger;
    List<GameObject> triggerCopies;
    DatabaseService dbService;
    AnimationController animController;
    public Slider moveSliderX;
    public Slider moveSliderY;
    public Slider sizeSlider;
    Vector3 position;
    public GameObject posObject;
    string triggerName;
    private Vector3 offset;
    private bool isDragging = false;
    int numCopies = 0; 

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

        // Find the GameObject with the DatabaseService script
        GameObject animationControllerObject = GameObject.Find("AnimationController");

        // Check if the GameObject was found
        if (animationControllerObject != null)
        {
            // Get the DatabaseService component from the found GameObject
            animController = animationControllerObject.GetComponent<AnimationController>();
        }
        else
        {
            Debug.LogError("GameObject with AnimationController not found.");
        }

        position = posObject.transform.position;

        triggerCopies = new List<GameObject>();
    }


    public void makeCopy(string modelName, string path)
    {
        numCopies++;
        GameObject copy = new GameObject("Trigger_copy" + numCopies);
        triggerCopies.Add(copy);
        LoadGlbFile(copy, path, modelName);
    }


    //public void SpawnObject()
    public void SpawnObject(string modelName, string path)
    {
        if (trigger != null)
        {
            DestroyImmediate(trigger);
        }
        
        trigger = new GameObject("Trigger");

        // Add a Box Collider to the GameObject
        BoxCollider boxCollider = trigger.AddComponent<BoxCollider>();

        // You can also set various properties of the Box Collider
        boxCollider.isTrigger = false; // Set to true if you want it to be a trigger
        //boxCollider.size = new Vector3(1.0f, 1.0f, 1.0f); // Set the size of the collider

        LoadGlbFile(trigger, path, modelName);
    }

    //public async void LoadGlbFile(GameObject loadedModel, string path)
    public async void LoadGlbFile(GameObject loadedModel, string path, string modelName)
    {
        var gltFastImport = new GLTFast.GltfImport();
        string downloadUrl = await dbService.GetDownloadURL(path);

        // Get byte data from database
        byte[] glbData = await dbService.getFile(downloadUrl);

        if (glbData != null)
        {
            // Create a new GLTFast loader
            var gltfImport = new GLTFast.GltfImport();

            var settings = new ImportSettings
            {
                GenerateMipMaps = true,
                AnisotropicFilterLevel = 3,
                NodeNameMethod = NameImportMethod.OriginalUnique
            };

            // Load the GLTF model from the byte array
            bool success = await LoadGltfBinaryFromMemory(glbData, loadedModel, downloadUrl, gltfImport);

            if (success)
            {
                loadedModel.transform.position = position;
                loadedModel.SetActive(true);
                //DontDestroyOnLoad(loadedModel);

                animController.FindAnimations(loadedModel);

                sizeSlider.value = 1;

                sizeSlider.interactable = true;
                moveSliderX.interactable = true;
                moveSliderY.interactable = true;
            }
            else
            {
                Debug.LogError("Loading glTF failed!");
            }
        }
    }

    async Task<bool> LoadGltfBinaryFromMemory(byte[] data, GameObject gameObject, string downloadUrl, GltfImport gltfImport)
    {
        var filePath = GetFilepath(downloadUrl);
        
        //Debug.Log(BitConverter.ToString(data));
        bool success = await gltfImport.LoadGltfBinary(
            data,
            // The URI of the original data is important for resolving relative URIs within the glTF
            new Uri(filePath)
            );
        if (success)
        {
            await gltfImport.InstantiateMainSceneAsync(gameObject.transform);
            gameObject.SetActive(false);
        }
        return success;
    }

    public string GetFilepath(string downloadURL)
    {
        return downloadURL.Replace("https", "file");
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = UnityEngine.Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.gameObject == trigger)
                {
                    // Select the object
                    isDragging = true;
                    offset = trigger.transform.position - hit.point;
                }
            }
        }

        if (isDragging && Input.GetMouseButton(0))
        {
            Ray ray = UnityEngine.Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                // Move the selected object to the mouse position on the plane
                Vector3 newPosition = hit.point + offset;
                newPosition.y = trigger.transform.position.y;
                trigger.transform.position = newPosition;
            }
        }

        if (Input.GetMouseButtonUp(0) && isDragging)
        {
            // Deselect the object
            isDragging = false;
        }
    }

    public void SetSelectedObject(GameObject obj)
    {
        trigger = obj;
    }

}
