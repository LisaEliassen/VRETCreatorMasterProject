using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ObjectDropdownManager : MonoBehaviour
{
    DatabaseService dbService;
    LoadGlb loadGlb;
    public GameObject databaseServiceObject;
    public TMP_Dropdown dropdown;
    public GameObject animationDropdown;
    public Toggle objectVisibility;
    public Toggle interactableToggle;
    public Slider sizeSlider;
    public TMP_InputField sizeInput;
    public Button addCopyButton;
    public Button removeCopyButton;
    public Button removeTriggerButton;
    //public Button yesButton;
    //public Button noButton;
    public GameObject EditSceneUI;
    public GameObject ModelUI;
    //public GameObject RemovePanel;
    public GameObject copies;

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
            loadGlb = databaseServiceObject.GetComponent<LoadGlb>();
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
        scenery = new List<GameObject>();
        interactableToggle.interactable = false;
    }


    public GameObject GetCurrentObject()
    {
        return this.currentObject;
    }

    void DropdownValueChanged(TMP_Dropdown change)
    {
        setCurrentObject(change.options[change.value].text);
    }

    public void addDropdownOption(GameObject obj, string option)
    {
        if (option == "Trigger")
        {
            SetTrigger(obj);
            dropdown.options.Add(new TMP_Dropdown.OptionData(option));
            int index = dropdown.options.FindIndex((i) => { return i.text.Equals(option); });
            //int index = FindOptionIndexByText("Trigger");
            if (index != -1)
            {
                dropdown.value = index;
                dropdown.RefreshShownValue();
            }
            setCurrentObject(option);
        }
        else
        {
            //AddSceneryObject(obj);
            objects[option] = obj;
            dropdown.options.Add(new TMP_Dropdown.OptionData(option));
            int index = dropdown.options.FindIndex((i) => { return i.text.Equals(option); });
            //int index = FindOptionIndexByText(option);
            if (index != -1)
            {
                dropdown.value = index;
                dropdown.RefreshShownValue();
            }
            setCurrentObject(option);
        }
    }

    int FindOptionIndexByText(string searchText)
    {
        for (int i = 0; i < dropdown.options.Count; i++)
        {
            if (dropdown.options[i].text == searchText)
            {
                return i;
            }
        }
        return -1;
    }

    public void removeDropdownOption(GameObject obj)
    {
        if (obj.name == "Trigger")
        {
            TMP_Dropdown.OptionData option = dropdown.options.Find(s => string.Equals(s.text, "Trigger"));
            dropdown.options.Remove(option);
            dropdown.RefreshShownValue();
            interactableToggle.interactable = false;
        }

        else 
        {
            foreach (KeyValuePair<string, GameObject> pair in objects)
            {
                if (pair.Value == obj)
                {
                    objects.Remove(pair.Key);
                    TMP_Dropdown.OptionData option = dropdown.options.Find(s => string.Equals(s.text, pair.Key));
                    dropdown.options.Remove(option);
                    dropdown.RefreshShownValue();
                    break;
                }
            }
        }

        if (dropdown.options.Count > 0)
        {
            dropdown.value = 0;
            dropdown.RefreshShownValue();
            setCurrentObject(dropdown.options[0].text);
        }
        else
        {
            sizeSlider.interactable = false;
            objectVisibility.interactable = false;
            removeTriggerButton.interactable = false;
        }
    }

    public void RemoveObject(GameObject obj)
    {
        removeDropdownOption(obj);
    }

    public void setCurrentObject(string option)
    {
        if (option == "Trigger")
        {
            this.currentObject = this.trigger;
            copies.SetActive(true);
            addCopyButton.interactable = true;
            interactableToggle.gameObject.SetActive(true);
            interactableToggle.interactable = true;
            animationDropdown.SetActive(true);
            objectVisibility.interactable = true;
            objectVisibility.gameObject.SetActive(true);

            int size = loadGlb.GetObjectSizes()[this.trigger];
            sizeSlider.value = size;
            ((TextMeshProUGUI)sizeInput.placeholder).text = size.ToString();
        }
        else
        {
            this.currentObject = objects[option];
            copies.SetActive(false);
            interactableToggle.gameObject.SetActive(false);
            animationDropdown.SetActive(false);
            objectVisibility.gameObject.SetActive(false);

            int size = loadGlb.GetObjectSizes()[objects[option]];
            sizeSlider.value = size;
            ((TextMeshProUGUI)sizeInput.placeholder).text = size.ToString();
        }

        removeTriggerButton.interactable = true;
        sizeSlider.interactable = true;
        sizeInput.interactable = true;
    }

    public void SetTrigger(GameObject trigger)
    {
        this.trigger = trigger;
    }

    public void AddSceneryObject(GameObject obj)
    {
        this.scenery.Add(obj);
    }

    public void RemoveTrigger()
    {
        removeDropdownOption(this.trigger);
        this.trigger = null;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
