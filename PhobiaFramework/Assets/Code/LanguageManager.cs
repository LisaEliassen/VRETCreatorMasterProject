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
    TextSizeManager textSizeManager;

    public GameObject databaseServiceObject;

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
        if (databaseServiceObject != null)
        {
            textSizeManager = databaseServiceObject.GetComponent<TextSizeManager>();
        }
        else
        {
            Debug.LogError("GameObject with DatabaseService not found.");
        }

        EnglishButton.onClick.AddListener(() =>
        {
            LocaleSelected(0);

            UIparent.SetActive(true);
            LanguageUI.SetActive(false);

            languageDropdown.value = 0;
        });

        NorwegianButton.onClick.AddListener(() =>
        {
            LocaleSelected(1);

            UIparent.SetActive(true);
            LanguageUI.SetActive(false);

            textSizeManager.ChangeToNorwegianOptions();

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
        Debug.Log(languageDropdown.captionText.text);
        return languageDropdown.captionText.text;
    }

    void DropdownValueChanged(TMP_Dropdown change)
    {
        string languageChosen = change.options[change.value].text;
        //Debug.Log(languageChosen);

        if (change.value == 0)
        {
            textSizeManager.ChangeToEnglishOptions();
        }
        else if (change.value == 1)
        {
            textSizeManager.ChangeToNorwegianOptions();
        }

        int selectedIndex = change.value;
        LocaleSelected(selectedIndex);
    }
}
