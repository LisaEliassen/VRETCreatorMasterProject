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

public class LoadGlb : MonoBehaviour
{
    GameObject trigger;
    List<GameObject> triggerCopies;
    DatabaseService dbService;
    AnimationController animController;
    LanguageManager languageManager;
    public GameObject databaseServiceObject; 

    public UnityEngine.Camera mainCamera;
    
    Vector3 position;
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


    // Start is called before the first frame update
    void Start()
    {
        RemovePanel.SetActive(true);
        RemovePanel.SetActive(false);

        // Check if the GameObject was found
        if (databaseServiceObject != null)
        {
            // Get the DatabaseService component from the found GameObject
            dbService = databaseServiceObject.GetComponent<DatabaseService>();
            //languageManager = databaseServiceObject.GetComponent<LanguageManager>();
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

        chooseFromDeviceButton.onClick.AddListener(ImportModel);
        removeTriggerButton.onClick.AddListener(() =>
        {
            RemovePanel.SetActive(true);
        });
        yesButton.onClick.AddListener(() =>
        {
            RemoveTrigger();
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
                if (triggerCopies != null && triggerCopies.Count > 0)
                {
                    foreach (GameObject copy in triggerCopies)
                    {
                        copy.AddComponent<XRGrabInteractable>();
                    }
                }
            }
            else
            {
                Destroy(trigger.GetComponent<XRGrabInteractable>());
                if (triggerCopies != null && triggerCopies.Count > 0)
                {
                    foreach (GameObject copy in triggerCopies)
                    {
                        Destroy(copy.GetComponent<XRGrabInteractable>());
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
                    trigger.transform.position = position;
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

    public void RemoveTrigger()
    {
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
        removeTriggerButton.interactable = false;

        sizeSlider.interactable = false;
        sizeInput.interactable = false;
        moveSliderX.interactable = false;
        moveSliderY.interactable = false;
        dropdown.interactable = false;
        dropdown.ClearOptions();
        addCopyButton.interactable = false;
        removeCopyButton.interactable = false;
    }

    public async void MakeCopy()
    {
        if (trigger != null && !string.IsNullOrEmpty(pathOfTrigger)) 
        {
            GameObject copy = new GameObject("Trigger_copy" + GetNumCopies());
            copy.tag = "Export";
            triggerCopies.Add(copy);
            bool success = await LoadGlbFile(copy, pathOfTrigger);
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
            }
            else
            {
                Debug.Log("Could not load model!");
            }
        }
    }
    
    public void RemoveCopy()
    {
        if (triggerCopies != null && GetNumCopies() > 0)
        {
            Debug.Log(GetNumCopies());
            DestroyImmediate(triggerCopies[GetNumCopies() - 1]);
            triggerCopies.RemoveAt(GetNumCopies() - 1);
        }
    }

    public async void SpawnObject(string modelName, string path)
    {
        if (trigger != null)
        {
            RemoveTrigger();
        }
        trigger = new GameObject("Trigger");
        trigger.tag = "Export";

        pathOfTrigger = path;
        bool success = await LoadGlbFile(trigger, path);
        if (success)
        {

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

            sizeSlider.value = 2;
            ((TextMeshProUGUI)sizeInput.placeholder).text = "2";
        }
        else
        {
            Debug.Log("The glb model could not be loaded!");
        }
    }

    public async void SpawnSceneryObject(string modelName, string path)
    {
        GameObject newObject = new GameObject(modelName);
        newObject.tag = "Scenery";

        bool success = await LoadGlbFile(newObject, path);
        if (success)
        {
            Debug.Log("Successfully loaded model!");
        }
        else
        {
            Debug.Log("The glb model could not be loaded!");
        }

    }

    //public async void LoadGlbFile(GameObject loadedModel, string path)
    public async Task<bool> LoadGlbFile(GameObject loadedModel, string path)
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

                // Add a Box Collider to the GameObject
                BoxCollider boxCollider = loadedModel.AddComponent<BoxCollider>();

                // Calculate the bounds of the loaded model
                Bounds modelBounds = CalculateModelBounds(loadedModel);

                // Set the default size of the collider based on the model's bounds
                boxCollider.size = modelBounds.size;

                // Set the default position of the collider to the center of the model
                boxCollider.center = modelBounds.center - loadedModel.transform.position;

                // You can also set various properties of the Box Collider
                boxCollider.isTrigger = false; // Set to true if you want it to be a trigger

                Rigidbody rb = loadedModel.GetComponent<Rigidbody>();
                if (rb == null)
                {
                    rb = loadedModel.AddComponent<Rigidbody>();
                }
                rb.isKinematic = true;  // Set to false if you want physics interactions


                loadedModel.AddComponent<DragObject>();
                DragObject dragObject = loadedModel.GetComponent<DragObject>();
                dragObject.SetCamera(mainCamera);

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

}
