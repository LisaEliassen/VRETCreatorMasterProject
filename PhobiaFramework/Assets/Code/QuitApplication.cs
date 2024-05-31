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
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

// This script provides functionality to quit the application from within the game. 

public class QuitApplication : MonoBehaviour
{
    public GameObject EditScene;
    public GameObject HelpUI;
    public GameObject QuitUI;
    public Button quitButton;
    public Button yesButton;
    public Button noButton;

    // Start is called before the first frame update
    void Start()
    {
        quitButton.onClick.AddListener(() =>
        {
            EditScene.SetActive(false);
            HelpUI.SetActive(false);
            QuitUI.SetActive(true);
        });
        yesButton.onClick.AddListener(Quit);
        noButton.onClick.AddListener(() =>
        {
            QuitUI.SetActive(false);
            HelpUI.SetActive(true);
            EditScene.SetActive(true);
        });
    }

    public void Quit()
    {
        #if UNITY_EDITOR
                    UnityEditor.EditorApplication.isPlaying = false;
        #else
                Application.Quit();
        #endif
    }
}
