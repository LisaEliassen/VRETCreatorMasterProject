using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityGLTF;
using UnityEngine.UI;
using System.Threading.Tasks;
using TMPro;
using GLTFast;

public class ScaleObject : MonoBehaviour
{
    public Slider sizeSlider;
    public GameObject databaseServiceObject;
    public TMP_InputField sizeInput;
    LoadGlb loadGlb;
    SceneSaver sceneSaver;
    ObjectDropdownManager objDropdownManager;
    GameObject trigger;
    List<GameObject> triggerCopies;

    GameObject objectToScale;

    void Start()
    {
        objDropdownManager = databaseServiceObject.GetComponent<ObjectDropdownManager>();
        loadGlb = databaseServiceObject.GetComponent<LoadGlb>();
        sceneSaver = databaseServiceObject.GetComponent<SceneSaver>();

        // Add an event listener to the slider's value changed event
        sizeSlider.onValueChanged.AddListener(ChangeSize);
        sizeInput.onValueChanged.AddListener((x) => ChangeObjectSizeInput(sizeInput.text));
        sizeSlider.interactable = false;
        sizeInput.interactable = false;

        triggerCopies = new List<GameObject>();
    }


    private void ChangeSize(float scaleValue)
    {
        GameObject objectToScale = objDropdownManager.GetCurrentObject();
        if (objectToScale != null)
        {
            float scaledValue = scaleValue / 3;
            Vector3 newScale = new Vector3(scaledValue, scaledValue, scaledValue);
            objectToScale.transform.localScale = newScale;

            if (objectToScale.name == "Trigger")
            {
                triggerCopies = loadGlb.GetCopies();
                if (triggerCopies.Count > 0)
                {
                    foreach (GameObject copy in triggerCopies)
                    {
                        copy.transform.localScale = newScale;
                    }
                }
            }
            
            ((TextMeshProUGUI)sizeInput.placeholder).text = scaleValue.ToString();
            sizeInput.text = scaleValue.ToString();

            if (objectToScale.name == "Trigger")
            {
                sceneSaver.SetTriggerSize(scaleValue.ToString());
                loadGlb.UpdateObjectSize(objectToScale, (int) scaleValue);
            }
            else
            {
                sceneSaver.SetSceneryObjectSize(objectToScale, scaleValue.ToString());
                loadGlb.UpdateObjectSize(objectToScale, (int) scaleValue);
            }
        }
    }

    // Callback method to adjust object size based on the slider's value
    private void ChangeObjectSize(float scaleValue)
    {
        // Assuming you want to change the scale of the loaded object
        // You can adjust this to your specific use case
        if (trigger == null || triggerCopies == null)
        {
            trigger = loadGlb.GetTrigger();
            triggerCopies = loadGlb.GetCopies();
        }
        /// Map the slider value (0-100) to the desired scale range (minScale-maxScale)
        float scaledValue = scaleValue/3;
        Vector3 newScale = new Vector3(scaledValue, scaledValue, scaledValue);
        trigger.transform.localScale = newScale;
        if (triggerCopies.Count > 0)
        {
            foreach (GameObject copy in triggerCopies) {
                copy.transform.localScale = newScale;
            }
        }
        ((TextMeshProUGUI)sizeInput.placeholder).text = scaleValue.ToString();
        sizeInput.text = scaleValue.ToString();

        sceneSaver.SetTriggerSize(scaleValue.ToString());
    }

    public void ChangeObjectSizeInput(string text)
    {
        if (!string.IsNullOrEmpty(text)) 
        {
            sizeSlider.value = int.Parse(text);
        }
    }
}
