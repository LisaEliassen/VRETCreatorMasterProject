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

public class LoadGlb : MonoBehaviour
{
    GameObject trigger;
    List<GameObject> triggerCopies;
    DatabaseService dbService;
    AnimationController animController;
    public Toggle objectVisibility;
    public Slider moveSliderX;
    public Slider moveSliderY;
    public Slider sizeSlider;
    public TMP_InputField sizeInput;
    public TMP_Dropdown dropdown;
    public Button chooseFromDeviceButton;
    public Button addCopyButton;
    public Button removeCopyButton;
    public Button removeTriggerButton;
    public GameObject EditSceneUI;
    public GameObject ModelUI;
    public TextMeshProUGUI numCopiesText;
    Vector3 position;
    public GameObject posObject;
    string triggerName;
    string pathOfTrigger;
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

        chooseFromDeviceButton.onClick.AddListener(ImportModel);
        removeTriggerButton.onClick.AddListener(RemoveTrigger);

        /*removeCopyButton.interactable = false;
        addCopyButton.interactable = false;*/
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
                    //DontDestroyOnLoad(loadedModel);

                    animController.FindAnimations(trigger);

                    sizeSlider.value = 2;
                    ((TextMeshProUGUI)sizeInput.placeholder).text = "2";

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
                //numCopies = 0;
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
            //numCopies++;
            GameObject copy = new GameObject("Trigger_copy" + GetNumCopies());
            triggerCopies.Add(copy);
            bool success = await LoadGlbFile(copy, pathOfTrigger);
            if (success)
            {
                copy.transform.localScale = trigger.transform.localScale;
                Debug.Log("Successfully loaded model!");

                Animation animationComponent = trigger.GetComponent<Animation>();

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
            //numCopies--;
        }
    }

    //public void SpawnObject()
    public async void SpawnObject(string modelName, string path)
    {
        if (trigger != null)
        {
            RemoveTrigger();
        }
        trigger = new GameObject("Trigger");

        pathOfTrigger = path;
        bool success = await LoadGlbFile(trigger, path);
        if (success)
        {
            animController.FindAnimations(trigger);
            objectVisibility.isOn = true;
            objectVisibility.interactable = true;
            removeTriggerButton.interactable = true;
            sizeSlider.value = 2;
            ((TextMeshProUGUI)sizeInput.placeholder).text = "2";
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
                //DontDestroyOnLoad(loadedModel);

                // Add a Box Collider to the GameObject
                BoxCollider boxCollider = loadedModel.AddComponent<BoxCollider>();

                // You can also set various properties of the Box Collider
                boxCollider.isTrigger = false; // Set to true if you want it to be a trigger
                //boxCollider.size = new Vector3(1.0f, 1.0f, 1.0f); // Set the size of the collider

                loadedModel.AddComponent<DragObject>();

                sizeSlider.interactable = true;
                sizeInput.interactable = true;
                moveSliderX.interactable = true;
                moveSliderY.interactable = true;
                addCopyButton.interactable = true;
            }
            else
            {
                Debug.LogError("Loading glTF failed!");
            }
            return success;
        }
        return false;
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

    // Update is called once per frame
    /*void Update()
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
    }*/

    public void SetSelectedObject(GameObject obj)
    {
        trigger = obj;
    }

}
