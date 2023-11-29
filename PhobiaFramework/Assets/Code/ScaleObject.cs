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
    GameObject trigger;
    List<GameObject> triggerCopies;

    void Start()
    {
        loadGlb = databaseServiceObject.GetComponent<LoadGlb>();

        // Add an event listener to the slider's value changed event
        sizeSlider.onValueChanged.AddListener(ChangeObjectSize);
        sizeInput.onValueChanged.AddListener((x) => ChangeObjectSizeInput(sizeInput.text));
        sizeSlider.interactable = false;
        sizeInput.interactable = false;
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
    }

    public void ChangeObjectSizeInput(string text)
    {
        if (!string.IsNullOrEmpty(text)) 
        {
            sizeSlider.value = int.Parse(text);
        }
    }
}
