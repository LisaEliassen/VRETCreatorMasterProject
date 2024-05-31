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
using UnityEngine;
using UnityEngine.UI;
using TMPro;

// The script links a TextMeshProUGUI component to a TextSizeManager for dynamic text size management, registering the text object with the manager upon Awake to ensure proper sizing updates.

public class TextObject : MonoBehaviour
{
    public GameObject databaseServiceObject;
    private TextMeshProUGUI myTextMeshProUI;
    TextSizeManager textSizeManager;

    private void Awake()
    {
        // Check if the GameObject was found
        if (databaseServiceObject != null)
        {
            textSizeManager = databaseServiceObject.GetComponent<TextSizeManager>();
        }
        else
        {
            Debug.LogError("GameObject with DatabaseService not found.");
        }

        myTextMeshProUI = GetComponent<TextMeshProUGUI>();

        if (myTextMeshProUI != null)
        {
            if (textSizeManager != null)
            {
                textSizeManager.RegisterTextObject(myTextMeshProUI);
            }
            else
            {
                Debug.Log("TextSizeManager is null");
            }
        }
        else
        {
            Debug.Log("TextMeshProUGUI was not found.");
        }
        
    }
}