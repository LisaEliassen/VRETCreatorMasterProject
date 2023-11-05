using UnityEngine;
using UnityGLTF;
using UnityEngine.UI;
using System.Threading.Tasks;
using GLTFast;

public class ScaleObject : MonoBehaviour
{
    public Slider sizeSlider;
    GameObject trigger;

    void Start()
    {
        // Add an event listener to the slider's value changed event
        sizeSlider.onValueChanged.AddListener(ChangeObjectSize);
        sizeSlider.interactable = false;
    }

    // Callback method to adjust object size based on the slider's value
    private void ChangeObjectSize(float scaleValue)
    {
        // Assuming you want to change the scale of the loaded object
        // You can adjust this to your specific use case
        if (trigger == null)
        {
            trigger = GameObject.Find("Trigger");
        }
        /// Map the slider value (0-100) to the desired scale range (minScale-maxScale)
        float scaledValue = scaleValue/2;
        Vector3 newScale = new Vector3(scaledValue, scaledValue, scaledValue);
        trigger.transform.localScale = newScale;
    }
}
