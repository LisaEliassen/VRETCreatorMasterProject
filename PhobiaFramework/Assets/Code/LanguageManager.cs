using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;

public class LanguageManager : MonoBehaviour
{
    string language;
    
    public TMP_Dropdown languageDropdown;

    public Button EnglishButton;
    public Button NorwegianButton;

    public GameObject SettingsUI;
    public GameObject UIparent;
    public GameObject LanguageUI;
    public GameObject editScene;
    public GameObject helpUI;

    // Start is called before the first frame update
    void Start()
    {
        EnglishButton.onClick.AddListener(() =>
        {
            LocaleSelected(0);

            UIparent.SetActive(true);
            LanguageUI.SetActive(false);

            language = "EN";

            languageDropdown.value = 0;
        });

        NorwegianButton.onClick.AddListener(() =>
        {
            LocaleSelected(1);

            UIparent.SetActive(true);
            LanguageUI.SetActive(false);

            language = "NO";

            languageDropdown.value = 1;
        });


        if (languageDropdown != null)
        {
            languageDropdown.ClearOptions();
            List<TMP_Dropdown.OptionData> optionsLang = new List<TMP_Dropdown.OptionData>();

            optionsLang.Add(new TMP_Dropdown.OptionData("English / Engelsk"));
            optionsLang.Add(new TMP_Dropdown.OptionData("Norwegian / Norsk"));

            languageDropdown.AddOptions(optionsLang);

            languageDropdown.onValueChanged.AddListener(delegate {
                DropdownValueChanged(languageDropdown);
            });
        }
    }

    static void LocaleSelected(int index)
    {
        LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[index];
    }

    public string getLanguage()
    {
        return language;
    }

    void DropdownValueChanged(TMP_Dropdown change)
    {
        string languageChosen = change.options[change.value].text;
        Debug.Log(languageChosen);

        int selectedIndex = change.value;
        LocaleSelected(selectedIndex);
    }
}
