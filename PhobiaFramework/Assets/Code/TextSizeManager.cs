using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class TextSizeManager : MonoBehaviour
{
    public Slider textSizeSlider;
    public TMP_Text[] tmpTexts; // Use TMP_Text for TextMeshPro text
    public Text[] texts;

    void Start()
    {
        // Subscribe to the slider's OnValueChanged event
        if (textSizeSlider != null)
        {
            textSizeSlider.onValueChanged.AddListener(ChangeTextSize);
            textSizeSlider.onValueChanged.AddListener(ChangeTextSize1);
        }
    }

    void ChangeTextSize(float newSize)
    {
        // Update the size of all TextMeshPro text components
        foreach (TMP_Text tmpText in tmpTexts)
        {
            if (tmpText != null)
            {
                tmpText.fontSize = Mathf.RoundToInt(newSize);
            }
        }
    }

    void ChangeTextSize1(float newSize)
    {
        // Update the size of all Text components
        foreach (Text text in texts)
        {
            if (text != null)
            {
                text.fontSize = Mathf.RoundToInt(newSize);
            }
        }
    }

}


