using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ObjectDropdownManager : MonoBehaviour
{
    DatabaseService dbService;
    public GameObject databaseServiceObject;
    public TMP_Dropdown dropdown;
    public Toggle objectVisibility;
    public Toggle interactableToggle;
    public Slider sizeSlider;
    public TMP_InputField sizeInput;
    public Button chooseFromDeviceButton;
    public Button addCopyButton;
    public Button removeCopyButton;
    public Button removeTriggerButton;
    public Button yesButton;
    public Button noButton;
    public GameObject EditSceneUI;
    public GameObject ModelUI;
    public GameObject RemovePanel;
    public GameObject Copies;

    List<TMP_Dropdown.OptionData> options;

    GameObject trigger;
    List<GameObject> triggerCopies;
    List<GameObject> scenery;

    Dictionary<string, GameObject> objects;

    GameObject currentObject;

    // Start is called before the first frame update
    void Start()
    {
        triggerCopies = new List<GameObject>();
        scenery = new List<GameObject>();

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

        if (dropdown != null)
        {
            dropdown.onValueChanged.AddListener(delegate {
                DropdownValueChanged(dropdown);
            });
        }
        else
        {
            Debug.LogError("TMP Dropdown is not assigned.");
        }

        objects = new Dictionary<string, GameObject>();
        dropdown.ClearOptions();
    }

    void DropdownValueChanged(TMP_Dropdown change)
    {
        setCurrentObject(change.options[change.value].text);
    }

    public void addDropdownOption(GameObject obj, string option)
    {
        if (!objects.ContainsKey(option))
        {
            objects[option] = obj;
            dropdown.options.Add(new TMP_Dropdown.OptionData(option));
        }
    }

    public void removeDropdownOption(GameObject obj)
    {
        foreach (KeyValuePair<string, GameObject> pair in objects)
        {
            if (pair.Value == obj)
            {
                objects.Remove(pair.Key);
                TMP_Dropdown.OptionData option = dropdown.options.Find(s => string.Equals(s.text, pair.Key));
                dropdown.options.Remove(option);
            }
        }
    }

    public void setCurrentObject(string option)
    {
        currentObject = objects[option];
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
