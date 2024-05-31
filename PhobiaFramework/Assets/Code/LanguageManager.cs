#region License
// Copyright (C) 2024 Lisa Maria Eliassen & Olesya Pasichnyk
// 
// This program is free software: you can redistribute it and/or modify
// it under the terms of the Commons Clause License version 1.0 with GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or any later version.
// 
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// Commons Clause License and GNU General Public License for more details.
// 
// You should have received a copy of the Commons Clause License and GNU General Public License
// along with this program. If not, see <https://commonsclause.com/> and <https://www.gnu.org/licenses/>.
#endregion

using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;

// The script handles switching between English and Norwegian languages in a Unity application using UI elements like buttons and a dropdown menu. 

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
