using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class TextSizeManagerNorsk : MonoBehaviour
{
    public TMP_Dropdown dropdownSize;

    private List<TextMeshProUGUI> textObjects = new List<TextMeshProUGUI>();

    void Start()
    {

        if (dropdownSize != null)
        {
            dropdownSize.ClearOptions();
            List<TMP_Dropdown.OptionData> optionsLang = new List<TMP_Dropdown.OptionData>();

            optionsLang.Add(new TMP_Dropdown.OptionData("Standard"));
            optionsLang.Add(new TMP_Dropdown.OptionData("Medium"));
            optionsLang.Add(new TMP_Dropdown.OptionData("Stor"));

            dropdownSize.AddOptions(optionsLang);
            // Add a listener to the dropdown's onValueChanged event
            dropdownSize.onValueChanged.AddListener(delegate {
                DropdownValueChanged(dropdownSize);
            });

        }
    }

    public void RegisterTextObject(TextMeshProUGUI textObject)
    {
        textObjects.Add(textObject);
        if (!textObjects.Contains(textObject))
        {
            textObjects.Add(textObject);
        }
    }

    public void SetTextSize(float newSize)
    {
        foreach (TextMeshProUGUI textObject in textObjects)
        {
            textObject.fontSize = newSize;
        }
    }

    void DropdownValueChanged(TMP_Dropdown change)
    {
        string textSizeChosen = change.options[change.value].text;
        Debug.Log(textSizeChosen);

        Debug.Log(textObjects.Count.ToString());

        if (textSizeChosen == "Standard")
        {
            SetTextSize(24f);
        }
        else if (textSizeChosen == "Medium")
        {
            SetTextSize(32f);
        }
        else if (textSizeChosen == "Stor")
        {
            SetTextSize(45f);
        }

    }
}
