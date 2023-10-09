using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SliderController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI sliderText = null;
    //[SerializeField] private float maxSliderValue = 100.0f;

    public void SliderChange(float value) 
    {
        float localValue = value; // * maxSliderValue;
        sliderText.text = localValue.ToString("0");
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
