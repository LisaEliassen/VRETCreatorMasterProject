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
using System.Threading.Tasks;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

// The script manages the size of TextMeshProUGUI objects based on a TMP_Dropdown selection, allowing for switching between English and Norwegian options, with "Default", "Medium", and "Large" sizes mapped to 24f, 32f, and 40f respectively, and providing methods to register text objects and change text size accordingly.

public class TextSizeManager : MonoBehaviour
{
    public TMP_Dropdown dropdown;

    private List<TextMeshProUGUI> textObjects = new List<TextMeshProUGUI>();

    void Start()
    {
        if (dropdown != null) 
        {
            dropdown.ClearOptions();
            List<TMP_Dropdown.OptionData> optionsLang = new List<TMP_Dropdown.OptionData>();

            optionsLang.Add(new TMP_Dropdown.OptionData("Default"));
            optionsLang.Add(new TMP_Dropdown.OptionData("Medium"));
            optionsLang.Add(new TMP_Dropdown.OptionData("Large"));

            dropdown.AddOptions(optionsLang);
            // Add a listener to the dropdown's onValueChanged event
            dropdown.onValueChanged.AddListener(delegate {
                DropdownValueChanged(dropdown);
            });
        }
    }

    public void ChangeToNorwegianOptions()
    {
        dropdown.options[0].text = "Standard";
        dropdown.options[1].text = "Medium";
        dropdown.options[2].text = "Stor";

        TMP_Text selectedText = dropdown.GetComponentInChildren<TMP_Text>();

        if (selectedText.text == "Default")
        {
            selectedText.text = "Standard";
        }
        else if (selectedText.text == "Large")
        {
            selectedText.text = "Stor";
        }
    }

    public void ChangeToEnglishOptions()
    {
        dropdown.options[0].text = "Default";
        dropdown.options[1].text = "Medium";
        dropdown.options[2].text = "Large";

        TMP_Text selectedText = dropdown.GetComponentInChildren<TMP_Text>();

        if (selectedText.text == "Standard")
        {
            selectedText.text = "Default";
        }
        else if (selectedText.text == "Stor")
        {
            selectedText.text = "Large";
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

        if (textSizeChosen == "Default" || textSizeChosen == "Standard")
        {
            SetTextSize(24f);
        }
        else if (textSizeChosen == "Medium" || textSizeChosen == "Medium")
        {
            SetTextSize(32f);
        }
        else if (textSizeChosen == "Large" || textSizeChosen == "Stor")
        {
            SetTextSize(40f);
        }
    }
}


