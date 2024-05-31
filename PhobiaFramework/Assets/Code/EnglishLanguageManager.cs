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

