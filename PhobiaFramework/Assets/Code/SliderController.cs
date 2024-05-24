using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

// The SliderController script manages the text display of a slider's value.
// The SliderChange method updates the text displayed on a TextMeshProUGUI component whenever the slider value changes.
// This script is designed to be attached to a GameObject with a slider component and a TextMeshProUGUI component to display the slider's current value.

public class SliderController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI sliderText = null;
    //[SerializeField] private float maxSliderValue = 100.0f;

    public void SliderChange(float value) 
    {
        sliderText.text = value.ToString("0");
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
