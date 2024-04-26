using System;
using System.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using SimpleFileBrowser;
using static SimpleFileBrowser.FileBrowser;
using UnityEngine;
using UnityEngine.UI;
using GLTFast;
using GLTFast.Schema;
using UnityEngine.Networking;
using TMPro;
using UnityEngine.XR.Interaction.Toolkit;
using System.Linq;

public class LoadGlb : MonoBehaviour
{
    GameObject trigger;
    GameObject newObject;
    List<GameObject> sceneryObjects;
    List<GameObject> triggerCopies;
    DatabaseService dbService;
    AnimationController animController;
    SceneSaver sceneSaver;
    public GameObject databaseServiceObject; 

    public UnityEngine.Camera mainCamera;
    public GameObject platform;
    
    Vector3 defaultPosition;
    public GameObject posObject;
    string triggerName;
    string pathOfTrigger;
    int numCopies = 0;

    // English UI elements:
    public Toggle objectVisibility;
    public Toggle interactableToggle;
    public Slider moveSliderX;
    public Slider moveSliderY;
    public Slider sizeSlider;
    public TMP_InputField sizeInput;
    public TMP_Dropdown dropdown;
    public Button chooseFromDeviceButton;
    public Button addCopyButton;
    public Button removeCopyButton;
    public Button removeTriggerButton;
    public Button yesButton;
    public Button noButton;
    public GameObject EditSceneUI;
    public GameObject ModelUI;
    public GameObject RemovePanel;
    public TextMeshProUGUI numCopiesText;
    public UnityEngine.Material lineMaterial;

    public GameObject ChosenObjectBox;

    ObjectDropdownManager objDropdownManager;

    Dictionary<GameObject, int> objectSizes;

    // Start is called before the first frame update
    void Start()
    {

        objectSizes = new Dictionary<GameObject, int>();

        RemovePanel.SetActive(true);
        RemovePanel.SetActive(false);

        // Check if the GameObject was found
        if (databaseServiceObject != null)
        {
            // Get the DatabaseService component from the found GameObject
            dbService = databaseServiceObject.GetComponent<DatabaseService>();
            sceneSaver = databaseServiceObject.GetComponent<SceneSaver>();
            objDropdownManager = databaseServiceObject.GetComponent<ObjectDropdownManager>();
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

        defaultPosition = posObject.transform.position;

        triggerCopies = new List<GameObject>();
        sceneryObjects = new List<GameObject>();

        chooseFromDeviceButton.onClick.AddListener(ImportModel);
        removeTriggerButton.onClick.AddListener(() =>
        {
            RemovePanel.SetActive(true);
        });
        yesButton.onClick.AddListener(() =>
        {
            RemoveObject();
            RemovePanel.SetActive(false);
        });
        noButton.onClick.AddListener(() =>
        {
            RemovePanel.SetActive(false);
        });
        interactableToggle.onValueChanged.AddListener(MakeInteractable);
    }

    public void MakeInteractable(bool visible)
    {
        if (trigger != null)
        {
            if (visible)
            {

                trigger.AddComponent<XRGrabInteractable>();
                trigger.AddComponent<OnGrabOnRelease>();
                if (triggerCopies != null && triggerCopies.Count > 0)
                {
                    foreach (GameObject copy in triggerCopies)
                    {
                        copy.AddComponent<XRGrabInteractable>();
                        copy.AddComponent<OnGrabOnRelease>();

                    }
                }
            }
            else
            {
                Destroy(trigger.GetComponent<XRGrabInteractable>());
                //trigger.GetComponent<Rigidbody>().isKinematic = true;
                if (triggerCopies != null && triggerCopies.Count > 0)
                {
                    foreach (GameObject copy in triggerCopies)
                    {
                        Destroy(copy.GetComponent<XRGrabInteractable>());
                        //copy.GetComponent<Rigidbody>().isKinematic = true;
                    }
                }
            }
        }
    }

    public UnityEngine.Camera getCamera()
    {
        return this.mainCamera;
    }

    public string GetModelPath()
    {
        return this.pathOfTrigger;
    }

    public List<GameObject> GetCopies()
    {
        return this.triggerCopies;
    }

    public int GetNumCopies()
    {
        return this.triggerCopies.Count;
    }

    public GameObject GetTrigger()
    {
        return this.trigger;
    }

    public List<GameObject> GetSceneryObjectList()
    {
        return this.sceneryObjects;
    }

    public Dictionary<GameObject, int> GetObjectSizes()
    {
        return this.objectSizes;
    }

    public void ImportModel()
    {
        FileBrowser.SetFilters(true, new FileBrowser.Filter("Models", ".glb"));
        FileBrowser.SetDefaultFilter(".glb");
        StartCoroutine(ShowLoadDialogCoroutine(PickMode.Files, HandleModelSelected));
    }

    public async void HandleModelSelected(string[] paths)
    {
        if (trigger != null)
        {
            DestroyImmediate(trigger);
            if (triggerCopies != null && triggerCopies.Count > 0)  
            {
                foreach (GameObject copy in triggerCopies)
                {
                    DestroyImmediate(copy);
                    triggerCopies = new List<GameObject>();
                    numCopies = 0;
                    numCopiesText.text = numCopies.ToString();
                }
            }
        }

        trigger = new GameObject("Trigger");
        trigger.tag = "Export";

        if (paths.Length > 0)
        {
            string path = paths[0];
            pathOfTrigger = path;
            
            var gltf = new GltfImport();
            byte[] data = File.ReadAllBytes(path);
            bool success = await gltf.LoadGltfBinary(
                data,
                // The URI of the original data is important for resolving relative URIs within the glTF
                new Uri(path)
            );
            if (success)
            {
                success = await gltf.InstantiateMainSceneAsync(trigger.transform);
                if (success)
                {
                    sceneSaver.SetPathToTrigger(pathOfTrigger);
                    objDropdownManager.addDropdownOption(trigger, "Trigger");
                    objDropdownManager.SetTrigger(trigger);
                    Vector3 pos = new Vector3(0.0f,0.0f, 0.0f);
                    trigger.transform.position = defaultPosition;
                    trigger.SetActive(true);

                    animController.FindAnimations(trigger);

                    sizeSlider.value = 2;
                    ((TextMeshProUGUI)sizeInput.placeholder).text = "2";

                    Debug.Log(sizeSlider.name);

                    sizeSlider.interactable = true;
                    sizeInput.interactable = true;
                    moveSliderX.interactable = true;
                    moveSliderY.interactable = true;
                    addCopyButton.interactable = true;
                    removeTriggerButton.interactable = true;

                    objectVisibility.isOn = true;
                    objectVisibility.interactable = true;

                    EditSceneUI.SetActive(true);
                    ModelUI.SetActive(false);
                }
                else
                {
                    Debug.LogError("Loading glTF failed!");
                }
            }
        }     
    }

    public void RemoveObject()
    {
        GameObject currentObject = objDropdownManager.GetCurrentObject();
        if (currentObject.name == "Trigger")
        {
            RemoveTrigger();
        }
        else
        {
            RemoveSceneryObject(currentObject);
            sceneryObjects.Remove(currentObject);
        }
    }

    public void RemoveSceneryObject(GameObject obj)
    {
        sceneSaver.RemoveSceneryObject(obj);
        objDropdownManager.RemoveObject(obj);
        DestroyImmediate(obj);
    }

    public void RemoveTrigger()
    {
        sceneSaver.SetPathToTrigger("");
        sceneSaver.SetTriggerPosition("");
        sceneSaver.SetTriggerRotation("");

        objDropdownManager.RemoveTrigger();

        DestroyImmediate(trigger);
        trigger = null;
        if (triggerCopies != null && GetNumCopies() > 0)
        {
            foreach (GameObject copy in triggerCopies)
            {
                DestroyImmediate(copy);
                triggerCopies = new List<GameObject>();
                numCopiesText.text = GetNumCopies().ToString();
            }
        }
        objectVisibility.interactable = false;
        //removeTriggerButton.interactable = false;

        dropdown.ClearOptions();
        dropdown.interactable = false;
        addCopyButton.interactable = false;
        removeCopyButton.interactable = false;
        sizeInput.interactable = false;

        /*sizeSlider.interactable = false;
        moveSliderX.interactable = false;
        moveSliderY.interactable = false;
        dropdown.ClearOptions();
        */
    }

    public async Task<bool> MakeCopy(Vector3 position, Quaternion rotation)
    {
        if (trigger != null && !string.IsNullOrEmpty(pathOfTrigger))
        {
            string name = "Trigger_copy" + GetNumCopies();
            GameObject copy = new GameObject(name);
            triggerCopies.Add(copy);
            int size = objectSizes[this.trigger];
            bool success = await LoadGlbFile(copy, pathOfTrigger, position, rotation);
            if (success)
            {
                copy.transform.localScale = trigger.transform.localScale;
                Debug.Log("Successfully loaded model!");

                UnityEngine.Animation animationComponent = trigger.GetComponent<UnityEngine.Animation>();

                if (animationComponent != null)
                {
                    string clipname = animController.GetCurrentAnimation();
                    if (clipname != null && clipname != "None")
                    {
                        animController.PlayAnimation(copy, clipname);
                    }
                }
                objDropdownManager.addDropdownOption(copy, name);

                return true;
            }
            else
            {
                Debug.Log("Could not load model!");
                return false;
            }

        }
        else
        {
            return false;
        }
    }

    public async void MakeCopy()
    {
        if (trigger != null && !string.IsNullOrEmpty(pathOfTrigger)) 
        {
            string name = "Trigger_copy" + GetNumCopies();
            GameObject copy = new GameObject(name);
            //copy.tag = "Export";
            triggerCopies.Add(copy);
            Vector3 position = new Vector3(GetNumCopies(), 0.0f, 0.0f);
            Quaternion rotation = Quaternion.identity;
            int size = objectSizes[this.trigger];
            bool success = await LoadGlbFile(copy, pathOfTrigger, position, rotation);
            if (success)
            {
                copy.transform.localScale = trigger.transform.localScale;
                Debug.Log("Successfully loaded model!");

                UnityEngine.Animation animationComponent = trigger.GetComponent<UnityEngine.Animation>();

                if (animationComponent != null)
                {
                    string clipname = animController.GetCurrentAnimation();
                    if (clipname != null && clipname != "None")
                    {
                        animController.PlayAnimation(copy, clipname);
                    }
                }

                objDropdownManager.addDropdownOption(copy, name);
            }
            else
            {
                Debug.Log("Could not load model!");
            }
        }
    }

    public void UpdateObjectSize(GameObject obj, int size)
    {
        this.objectSizes[obj] = size;
    }
    
    public void RemoveCopy()
    {
        if (triggerCopies != null && GetNumCopies() > 0)
        {
            Debug.Log(GetNumCopies());
            DestroyImmediate(triggerCopies[GetNumCopies() - 1]);
            objDropdownManager.DeleteCopy(triggerCopies.ElementAt(GetNumCopies() - 1));
            triggerCopies.RemoveAt(GetNumCopies() - 1);

        }
    }

    public void ResetScene()
    {
        if (trigger != null)
        {
            RemoveTrigger();
        }
        foreach (GameObject sceneryObj in sceneryObjects)
        {
            RemoveSceneryObject(sceneryObj);
            sceneryObjects = new List<GameObject>();
        }
    }

    public async Task<bool> SpawnObject(string modelName, string path, Vector3 position, Quaternion rotation, int size)
    {
        Debug.Log(size.ToString());

        if (trigger != null)
        {
            RemoveTrigger();
        }
        trigger = new GameObject("Trigger");
        trigger.tag = "Export";
        int LayerTrigger = LayerMask.NameToLayer("Trigger");
        trigger.layer = LayerTrigger;

        pathOfTrigger = path;
        bool success = await LoadGlbFile(trigger, path, position, rotation);
        if (success)
        {
            interactableToggle.isOn = false; //set Interactable Toggle to false when loading model
            sceneSaver.SetPathToTrigger(pathOfTrigger);

            Debug.Log("Successfully loaded model!");
            animController.FindAnimations(trigger);
            objectVisibility.isOn = true;
            objectVisibility.interactable = true;
            removeTriggerButton.interactable = true;

            Debug.Log(sizeSlider.name);

            sizeSlider.interactable = true;
            sizeInput.interactable = true;
            moveSliderX.interactable = true;
            moveSliderY.interactable = true;
            addCopyButton.interactable = true;

            UpdateObjectSize(trigger, size);

            objDropdownManager.addDropdownOption(trigger, "Trigger");

            objDropdownManager.SetTrigger(trigger);

            return true;
        }
        else
        {
            Debug.Log("The glb model could not be loaded!");

            return false;
        }
    }

    public async Task<bool> SpawnSceneryObject(string modelName, string path, Vector3 position, Quaternion rotation, int size)
    {
        string name = modelName;
        if (sceneSaver.sceneryPaths.ContainsKey(path))
        {
            int count = sceneSaver.sceneryPaths[path].Count;
            name = modelName + "_" + count;
            newObject = new GameObject(modelName + count);
        }
        else
        {
            newObject = new GameObject(modelName);
        }

        newObject.tag = "Scenery";
        int LayerObject = LayerMask.NameToLayer("Object");
        newObject.layer = LayerObject;

        bool success = await LoadGlbFile(newObject, path, position, rotation);
        if (success)
        {
            sceneryObjects.Add(newObject);
            Debug.Log("Successfully loaded model!");
            SceneryObject obj = new SceneryObject(name, path, newObject.transform.position.ToString(), newObject.transform.rotation.ToString(), size.ToString());
            sceneSaver.AddSceneryObject(newObject, obj);
            
            UpdateObjectSize(newObject, size);
            objDropdownManager.addDropdownOption(newObject, name);

            return true;
        }
        else
        {
            Debug.Log("The glb model could not be loaded!");
            return false;
        }

    }

    //public async void LoadGlbFile(GameObject loadedModel, string path)
    public async Task<bool> LoadGlbFile(GameObject loadedModel, string path, Vector3 position, Quaternion rotation)
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
                loadedModel.transform.rotation = rotation;
                loadedModel.SetActive(true);

                // Add a Box Collider to the GameObject
                BoxCollider boxCollider = loadedModel.AddComponent<BoxCollider>();

                // Calculate the bounds of the loaded model
                Bounds modelBounds = CalculateModelBounds(loadedModel);

                // Set the default size of the collider based on the model's bounds
                //boxCollider.size = new Vector3(modelBounds.size.x, 0.001f, modelBounds.size.z); //flat collider
                boxCollider.size = modelBounds.size;

                Vector3 boundsCenterOffset = modelBounds.center - loadedModel.transform.position;

                // Set the default position of the collider to the center of the model
                //boxCollider.center = new Vector3(modelBounds.center.x - loadedModel.transform.position.x, 0, modelBounds.center.z - loadedModel.transform.position.z);
                boxCollider.center = boundsCenterOffset;

                // You can also set various properties of the Box Collider
                boxCollider.isTrigger = false; // Set to true if you want it to be a trigger

                /*LineRenderer lineRenderer;

                lineRenderer = GetComponent<LineRenderer>();
                if (lineRenderer == null)
                    lineRenderer = loadedModel.AddComponent<LineRenderer>();

                // Set line properties
                lineRenderer.positionCount = 16; // Eight points to form a closed box
                lineRenderer.loop = true; // Close the loop to form a box
                lineRenderer.useWorldSpace = false;
                lineRenderer.startWidth = lineRenderer.endWidth = 0.05f; // Adjust line width as needed


                if (lineMaterial != null)
                {
                    lineRenderer.material = lineMaterial;
                }

                // Get the world space bounds of the collider
                Bounds bounds = boxCollider.bounds;
                Vector3 center = bounds.center;
                Vector3 extents = bounds.extents;

                // Calculate box vertices in world space
                Vector3[] vertices = new Vector3[]
                {
                    center + new Vector3(-extents.x, -extents.y, -extents.z), // bottom-left-back
                    center + new Vector3(extents.x, -extents.y, -extents.z),  // bottom-right-back
                    center + new Vector3(extents.x, -extents.y, extents.z),   // bottom-right-front
                    center + new Vector3(-extents.x, -extents.y, extents.z),  // bottom-left-front
                    center + new Vector3(-extents.x, extents.y, -extents.z),  // top-left-back
                    center + new Vector3(extents.x, extents.y, -extents.z),   // top-right-back
                    center + new Vector3(extents.x, extents.y, extents.z),    // top-right-front
                    center + new Vector3(-extents.x, extents.y, extents.z),   // top-left-front
                };

                // Set the positions of the LineRenderer to draw the edges of the box
                lineRenderer.SetPositions(new Vector3[]
                {
                    // Bottom edges
                    vertices[0], vertices[1], vertices[2], vertices[3], vertices[0],
                    vertices[4], vertices[5], vertices[1], vertices[5], vertices[6],
                    vertices[2], vertices[6], vertices[7], vertices[3], vertices[7], vertices[4]

                });*/

                GameObject selectionBox = Instantiate(ChosenObjectBox, loadedModel.transform.position, Quaternion.identity);
                GameObject box = selectionBox.transform.GetChild(0).gameObject;

                box.transform.localScale = new Vector3(
                    boxCollider.size.x,
                    boxCollider.size.y,
                    boxCollider.size.z);

                selectionBox.transform.parent = loadedModel.transform;
                selectionBox.transform.localPosition = boxCollider.center;

                //selectionBox.transform.localPosition = new Vector3(selectionBox.transform.position.x, selectionBox.transform.position.y, selectionBox.transform.position.z);

                /*GameObject circle = selectionBox.transform.GetChild(1).gameObject;
                GameObject arrow = circle.transform.GetChild(0).gameObject;
                arrow = arrow.transform.GetChild(0).gameObject;

                ScalingArrow scalingArrowScript = arrow.GetComponent<ScalingArrow>();
                scalingArrowScript.SetCamera(mainCamera);*/

                int LayerUser = LayerMask.NameToLayer("User");
                foreach (Transform child in selectionBox.transform)
                {
                    child.gameObject.layer = LayerUser;
                }
                selectionBox.layer = LayerUser;

                selectionBox.SetActive(false);

                Rigidbody rb = loadedModel.GetComponent<Rigidbody>();
                if (rb == null)
                {
                    rb = loadedModel.AddComponent<Rigidbody>();
                }
                rb.isKinematic = true;  // Set to false if you want physics interactions

                loadedModel.AddComponent<DragObject>();
                DragObject dragObject = loadedModel.GetComponent<DragObject>();
                dragObject.SetCamera(mainCamera);
                dragObject.SetPlatform(platform);

                
            }
            else
            {
                Debug.LogError("Loading glTF failed!");
            }
            return success;
        }
        return false;
    }

    private Bounds CalculateModelBounds(GameObject model)
    {
        Renderer[] renderers = model.GetComponentsInChildren<Renderer>();
        Bounds bounds = renderers.Length > 0 ? renderers[0].bounds : new Bounds(Vector3.zero, Vector3.zero);

        foreach (Renderer renderer in renderers)
        {
            bounds.Encapsulate(renderer.bounds);
        }

        return bounds;
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

    IEnumerator ShowLoadDialogCoroutine(PickMode pickMode, Action<string[]> callback)
    {
        yield return FileBrowser.WaitForLoadDialog(pickMode, true, null, null, "Load Files", "Load");

        if (FileBrowser.Success)
        {
            callback(FileBrowser.Result);
        }
    }

    public void SetSelectedObject(GameObject obj)
    {
        trigger = obj;
    }

    void Update()
    {

    }
}
