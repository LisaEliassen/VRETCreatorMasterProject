using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class LanguageManager : MonoBehaviour
{

    string language;
    public TMP_Dropdown languageDropdownEN;
    public TMP_Dropdown languageDropdownNO;

    public Button EnglishButton;
    public Button NorwegianButton;

    public GameObject englishSettingsUI;
    public GameObject norwegianSettingsUI;
    public GameObject englishParent;
    public GameObject norwegianParent;
    public GameObject LanguageUI;
    public GameObject editScene;
    public GameObject editSceneNO;
    public GameObject helpUI;
    public GameObject helpUI_NO;

    // Start is called before the first frame update
    void Start()
    {
        EnglishButton.onClick.AddListener(() =>
        {

            englishParent.SetActive(true);
            norwegianParent.SetActive(false);
            LanguageUI.SetActive(false);

            language = "EN";

            /*int optionIndex = languageDropdownEN.options.FindIndex(option => option.text == "English");
            languageDropdownEN.value = optionIndex;*/
        });

        NorwegianButton.onClick.AddListener(() =>
        {
            englishParent.SetActive(false);
            norwegianParent.SetActive(true);
            LanguageUI.SetActive(false);

            language = "NO";

            int optionIndex = languageDropdownNO.options.FindIndex(option => option.text == "Norsk");
            languageDropdownNO.value = optionIndex;
        });


        if (languageDropdownEN != null)
        {
            languageDropdownEN.ClearOptions();
            List<TMP_Dropdown.OptionData> optionsLang = new List<TMP_Dropdown.OptionData>();

            optionsLang.Add(new TMP_Dropdown.OptionData("English"));
            optionsLang.Add(new TMP_Dropdown.OptionData("Norwegian"));

            languageDropdownEN.AddOptions(optionsLang);
            // Add a listener to the dropdown's onValueChanged event
            languageDropdownEN.onValueChanged.AddListener(delegate {
                DropdownValueChangedEN(languageDropdownEN);
            });
        }

        if (languageDropdownNO != null)
        {
            languageDropdownNO.ClearOptions();
            List<TMP_Dropdown.OptionData> optionsLang = new List<TMP_Dropdown.OptionData>();

            optionsLang.Add(new TMP_Dropdown.OptionData("Engelsk"));
            optionsLang.Add(new TMP_Dropdown.OptionData("Norsk"));

            languageDropdownNO.AddOptions(optionsLang);
            // Add a listener to the dropdown's onValueChanged event
            languageDropdownNO.onValueChanged.AddListener(delegate {
                DropdownValueChangedNO(languageDropdownNO);
            });
        }
    }

    public string getLanguage()
    {
        return language;
    }

    void DropdownValueChangedEN(TMP_Dropdown change)
    {
        string languageChosen = change.options[change.value].text;
        Debug.Log(languageChosen);

        // You may add logic here to change other settings based on the selected language if needed

        // Activate/deactivate UI elements based on the selected language
        if (languageChosen == "English")
        {
            if (englishSettingsUI.activeSelf)
            {
                englishSettingsUI.SetActive(true);
                norwegianSettingsUI.SetActive(false);
                editScene.SetActive(false);
                helpUI.SetActive(false);
            } 

            // Norwegian UIs:
            norwegianParent.SetActive(false);
            editSceneNO.SetActive(false);
            helpUI_NO.SetActive(false);

            englishParent.SetActive(true);

            language = "EN";

            int optionIndex = languageDropdownNO.options.FindIndex(option => option.text == "Engelsk");
            languageDropdownNO.value = optionIndex;
        }
        else if (languageChosen == "Norwegian")
        {
            if (englishSettingsUI.activeSelf)
            {
                englishSettingsUI.SetActive(false);
                norwegianSettingsUI.SetActive(true);
                editSceneNO.SetActive(false);
                helpUI_NO.SetActive(false);
            }

            // English UIs:
            englishParent.SetActive(false);
            editScene.SetActive(false);
            helpUI.SetActive(false);

            norwegianParent.SetActive(true);

            language = "NO";

            int optionIndex = languageDropdownNO.options.FindIndex(option => option.text == "Norsk");
            languageDropdownNO.value = optionIndex;
        }
    }

    void DropdownValueChangedNO(TMP_Dropdown change)
    {
        string languageChosen = change.options[change.value].text;
        Debug.Log(languageChosen);

        // You may add logic here to change other settings based on the selected language if needed

        // Activate/deactivate UI elements based on the selected language
        if (languageChosen == "Engelsk")
        {
            if (norwegianSettingsUI.activeSelf)
            {
                norwegianSettingsUI.SetActive(false);
                englishSettingsUI.SetActive(true);
                editScene.SetActive(false);
                helpUI.SetActive(false);
            }

            // Norwegian UIs:
            norwegianParent.SetActive(false);
            editSceneNO.SetActive(false);
            helpUI_NO.SetActive(false);

            englishParent.SetActive(true);

            language = "EN";

            int optionIndex = languageDropdownEN.options.FindIndex(option => option.text == "English");
            languageDropdownEN.value = optionIndex;
            
        }
        else if (languageChosen == "Norsk")
        {
            if (norwegianSettingsUI.activeSelf)
            {
                englishSettingsUI.SetActive(false);
                norwegianSettingsUI.SetActive(true);
                editSceneNO.SetActive(false);
                helpUI_NO.SetActive(false);
            }

            // English UIs:
            englishParent.SetActive(false);
            editScene.SetActive(false);
            helpUI.SetActive(false);

            norwegianParent.SetActive(true);

            language = "NO";

            int optionIndex = languageDropdownEN.options.FindIndex(option => option.text == "Norwegian");
            languageDropdownEN.value = optionIndex;
            
        }
    }
}
