using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class TextSizeManager : MonoBehaviour
{
    public TMP_Dropdown dropdownSize;
    //public TMP_Dropdown language;

    private List<TextMeshProUGUI> textObjects = new List<TextMeshProUGUI>();

    void Start()
    {

        if (dropdownSize != null) //&& language != null)
        {
            
            //string languageChosen = language.options[language.value].text;
            //Debug.Log(languageChosen);

            //SetDropdownOptions(languageChosen);

            // Add a listener to the dropdown's onValueChanged event
            dropdownSize.onValueChanged.AddListener(delegate {
                DropdownValueChanged(dropdownSize);
            });

        }
    }

    /*
    void SetDropdownOptions(string languageChosen)
    {
        if (languageChosen == "English")
        {
            dropdownSize.ClearOptions();
            List<TMP_Dropdown.OptionData> options = new List<TMP_Dropdown.OptionData>();
            options.Add(new TMP_Dropdown.OptionData("Default"));
            options.Add(new TMP_Dropdown.OptionData("Medium"));
            options.Add(new TMP_Dropdown.OptionData("Large"));
            dropdownSize.AddOptions(options);
        }
        else if (languageChosen == "Norsk")
        {
            dropdownSize.ClearOptions();
            List<TMP_Dropdown.OptionData> options = new List<TMP_Dropdown.OptionData>();
            options.Add(new TMP_Dropdown.OptionData("Standard"));
            options.Add(new TMP_Dropdown.OptionData("Medium"));
            options.Add(new TMP_Dropdown.OptionData("Stor"));
            dropdownSize.AddOptions(options);
        }
    }*/


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

        if (textSizeChosen == "Default")
        {
            SetTextSize(24f);
        }
        else if (textSizeChosen == "Medium")
        {
            SetTextSize(32f);
        }
        else if (textSizeChosen == "Large")
        {
            SetTextSize(45f);
        }
        /*
        if (textSizeChosen == "Standard")
        {
            SetTextSize(24f);
        }
        else if (textSizeChosen == "Stor")
        {
            SetTextSize(45f);
        }*/

    }
}


