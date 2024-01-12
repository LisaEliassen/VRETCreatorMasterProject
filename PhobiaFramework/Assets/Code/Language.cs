using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Language : MonoBehaviour
{
    public TMP_Dropdown dropdownLanguage;

    private Dictionary<string, Dictionary<string, string>> translations;
    private string currentLanguage = "English"; // Default language

    // Start is called before the first frame update
    void Start()
    {
        InitializeTranslations();
        UpdateDropdownOptions();
        if (dropdownLanguage != null)
        {
            // Add a listener to the dropdown's onValueChanged event
            dropdownLanguage.onValueChanged.AddListener(delegate
            {
                SetLanguage(dropdownLanguage);
            });

            
        }
    }

    /*
    public void RegisterTextObject(TextMeshProUGUI textObject)
    {
        textObjects.Add(textObject);
    }*/

    void InitializeTranslations()
    {
        // Initialize your translations here
        translations = new Dictionary<string, Dictionary<string, string>>
        {
            {"English", new Dictionary<string, string> {{ "Standard", "Default"}, {"Medium", "Medium"}, { "Stor", "Large"}, 
                {"Engelsk", "English"}, { "Norsk", "Norwegian"}, {"Tilbake", "Back"}, {"Tekst størrelse", "Text size"},
                {"Inverter scrolling", "Invert Scrolling"}, {"Inverter rotasjon", "Invert Rotation"}, {"Inverter kamera", "Invert Cam"},
                {"Språk", "Language"}}},
            {"Norwegian", new Dictionary<string, string> {{"Default", "Standard"}, {"Medium", "Medium"}, {"Large", "Stor"}, 
                {"English", "Engelsk"}, {"Norwegian", "Norsk"}, {"Back", "Tilbake"}, {"Text size", "Tekst størrelse"},
                {"Invert Scrolling", "Inverter scrolling"}, { "Invert Rotation", "Inverter rotasjon"}, { "Invert Cam", "Inverter kamera"},
                {"Language", "Språk"}}}
            // Add more languages as needed
        };
    }

   

void SetLanguage(TMP_Dropdown change)
    {
        string languageChosen = change.options[change.value].text;
        Debug.Log(languageChosen);
        if (translations.ContainsKey(languageChosen))
        {
            currentLanguage = languageChosen;
            UpdateTexts();
        }
        else
        {
            Debug.LogError("Language not found: " + languageChosen);
        }
    }

    void UpdateTexts()
    {
        TextMeshProUGUI[] textObjects = FindObjectsOfType<TextMeshProUGUI>();
        foreach (TextMeshProUGUI textObject in textObjects)
        {
            // Assuming textObject.name is a unique identifier for each text
            string key = textObject.text;
            Debug.Log(key);

            if (translations[currentLanguage].ContainsKey(key))
            {
                textObject.text = translations[currentLanguage][key];
            }
            else
            {
                Debug.LogWarning($"Translation not found for key: {key} in language: {currentLanguage}");
            }
        }
    }

    void UpdateDropdownOptions()
    {
        if (currentLanguage == "English") { 
            dropdownLanguage.ClearOptions();
            List<TMP_Dropdown.OptionData> optionsLang = new List<TMP_Dropdown.OptionData>();

            optionsLang.Add(new TMP_Dropdown.OptionData("English"));
            optionsLang.Add(new TMP_Dropdown.OptionData("Norwegian"));

            dropdownLanguage.AddOptions(optionsLang);
        }
        else if (currentLanguage == "Norsk")
        {
            dropdownLanguage.ClearOptions();
            List<TMP_Dropdown.OptionData> optionsLang = new List<TMP_Dropdown.OptionData>();

            optionsLang.Add(new TMP_Dropdown.OptionData("Engelsk"));
            optionsLang.Add(new TMP_Dropdown.OptionData("Norsk"));

            dropdownLanguage.AddOptions(optionsLang);
        }
    }

    private void Update()
    {
        if (currentLanguage == "English")
        {
            dropdownLanguage.ClearOptions();
            List<TMP_Dropdown.OptionData> optionsLang = new List<TMP_Dropdown.OptionData>();

            optionsLang.Add(new TMP_Dropdown.OptionData("English"));
            optionsLang.Add(new TMP_Dropdown.OptionData("Norwegian"));

            dropdownLanguage.AddOptions(optionsLang);
        }
        else if (currentLanguage == "Norsk")
        {
            dropdownLanguage.ClearOptions();
            List<TMP_Dropdown.OptionData> optionsLang = new List<TMP_Dropdown.OptionData>();

            optionsLang.Add(new TMP_Dropdown.OptionData("Engelsk"));
            optionsLang.Add(new TMP_Dropdown.OptionData("Norsk"));

            dropdownLanguage.AddOptions(optionsLang);
        }
    }
}

