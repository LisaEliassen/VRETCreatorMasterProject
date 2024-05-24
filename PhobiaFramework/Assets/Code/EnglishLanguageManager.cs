using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

// The script is designed to manage language settings in the Unity application.

public class EnglishLanguageManager : MonoBehaviour
{
    public TMP_Dropdown dropdownLanguage;
    public GameObject englishSettingsUI;
    public GameObject norwegianSettingsUI;

    public GameObject englishParent;
    public GameObject norwegianParent;

    public GameObject editScene;
    public GameObject editSceneNorsk;

    public GameObject helpUI;
    public GameObject helpUINorsk;


    // Start is called before the first frame update
    void Start()
    {
        if (dropdownLanguage != null)
        {
            dropdownLanguage.ClearOptions();
            List<TMP_Dropdown.OptionData> optionsLang = new List<TMP_Dropdown.OptionData>();

            optionsLang.Add(new TMP_Dropdown.OptionData("English"));
            optionsLang.Add(new TMP_Dropdown.OptionData("Norwegian"));

            dropdownLanguage.AddOptions(optionsLang);
            // Add a listener to the dropdown's onValueChanged event
            dropdownLanguage.onValueChanged.AddListener(delegate {
                DropdownValueChanged(dropdownLanguage);
            });

        }
    }

    void DropdownValueChanged(TMP_Dropdown change)
    {
        string languageChosen = change.options[change.value].text;
        Debug.Log(languageChosen);

        // You may add logic here to change other settings based on the selected language if needed

        // Activate/deactivate UI elements based on the selected language
        if (languageChosen == "English")
        {
            englishSettingsUI.SetActive(true);
            englishParent.SetActive(true);
            editScene.SetActive(false);
            helpUI.SetActive(false);
            norwegianSettingsUI.SetActive(false);
            norwegianParent.SetActive(false);
            editSceneNorsk.SetActive(false);
            helpUINorsk.SetActive(false);
        }
        else if (languageChosen == "Norwegian")
        {
            englishSettingsUI.SetActive(false);
            englishParent.SetActive(false);
            editScene.SetActive(false);
            helpUI.SetActive(false);
            norwegianSettingsUI.SetActive(true);
            norwegianParent.SetActive(true);
            editSceneNorsk.SetActive(false);
            helpUINorsk.SetActive(false);
        }
    }

}

