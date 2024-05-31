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
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;
using System.IO;

// The class controls the visibility of various UI elements based on user interactions with buttons. 

public class ScreenManager : MonoBehaviour
{
    public Button uploadFileButton;
    public Button chooseMediaButton;
    public Button addTriggerButton;
    public Button backFromModelsButton;
    public Button backFromUploadButton;
    public Button backFrom360Media;
    public Button deleteFilesButton;
    public Button backFromDeleteButton;
    public Button chooseSoundButton;
    public Button backFromSoundButton;
    public Button chooseSceneryButton;
    public Button backFromSceneryButton;
    public Button settingsButton;
    public Button backFromSettingsButton;
    public Button loadSceneButton;
    public Button backFromScenesButton;
    public GameObject EditSceneUI;
    public GameObject ModelUI;
    public GameObject UploadUI;
    public GameObject ChooseMediaUI;
    public GameObject DeleteUI;
    public GameObject SettingsUI;
    public GameObject ControlsUI;
    public GameObject SoundUI;
    public GameObject SceneryUI;
    public GameObject ScenesUI;
    public GameObject HelpUI;
    public GameObject QuitUI;
    public GameObject UI_parent;

    // Start is called before the first frame update
    void Start()
    {
        uploadFileButton.onClick.AddListener(() => ShowUI("Upload"));
        backFromUploadButton.onClick.AddListener(() => ShowUI("EditScene"));

        addTriggerButton.onClick.AddListener(() => ShowUI("Models"));
        backFromModelsButton.onClick.AddListener(() => ShowUI("EditScene"));

        chooseMediaButton.onClick.AddListener(() => ShowUI("360Media"));
        backFrom360Media.onClick.AddListener(() => ShowUI("EditScene"));
        
        deleteFilesButton.onClick.AddListener(() => ShowUI("Delete"));
        backFromDeleteButton.onClick.AddListener(() => ShowUI("Upload"));
        
        chooseSoundButton.onClick.AddListener(() => ShowUI("Sound"));
        backFromSoundButton.onClick.AddListener(() => ShowUI("EditScene"));

        settingsButton.onClick.AddListener(() => ShowUI("Settings"));
        backFromSettingsButton.onClick.AddListener(() => ShowUI("EditScene"));

        chooseSceneryButton.onClick.AddListener(() => ShowUI("Scenery"));
        backFromSceneryButton.onClick.AddListener(() => ShowUI("EditScene"));

        loadSceneButton.onClick.AddListener(() => ShowUI("Scenes"));
        backFromScenesButton.onClick.AddListener(() => ShowUI("EditScene"));

        UI_parent.SetActive(true);

        ModelUI.SetActive(true);
        ChooseMediaUI.SetActive(true);
        UploadUI.SetActive(true);
        DeleteUI.SetActive(true);
        SettingsUI.SetActive(true);
        ControlsUI.SetActive(true);
        SoundUI.SetActive(true);
        HelpUI.SetActive(true);
        QuitUI.SetActive(true);
        SceneryUI.SetActive(true);
        ScenesUI.SetActive(true);

        ModelUI.SetActive(false);
        ChooseMediaUI.SetActive(false);
        UploadUI.SetActive(false);
        DeleteUI.SetActive(false);
        SettingsUI.SetActive(false);
        ControlsUI.SetActive(false);
        SoundUI.SetActive(false);
        QuitUI.SetActive(false);
        SceneryUI.SetActive(false);
        ScenesUI.SetActive(false);

        UI_parent.SetActive(false);
    }

    public void ShowUI(string UIname)
    {
        if (UIname == "EditScene")
        {
            EditSceneUI.SetActive(true);
            ModelUI.SetActive(false);
            ChooseMediaUI.SetActive(false);
            UploadUI.SetActive(false);
            DeleteUI.SetActive(false);
            SettingsUI.SetActive(false);
            ControlsUI.SetActive(false);
            SoundUI.SetActive(false);
            SceneryUI.SetActive(false);
            ScenesUI.SetActive(false);
            //HelpUI.SetActive(false);
        }
        else if (UIname == "Upload")
        {
            EditSceneUI.SetActive(false);
            ModelUI.SetActive(false);
            ChooseMediaUI.SetActive(false);
            UploadUI.SetActive(true);
            DeleteUI.SetActive(false);
            SettingsUI.SetActive(false);
            ControlsUI.SetActive(false);
            SoundUI.SetActive(false);
            SceneryUI.SetActive(false);
            ScenesUI.SetActive(false);
            //HelpUI.SetActive(false);
        }
        else if (UIname == "360Media")
        {
            EditSceneUI.SetActive(false);
            ModelUI.SetActive(false);
            ChooseMediaUI.SetActive(true);
            UploadUI.SetActive(false);
            DeleteUI.SetActive(false);
            SettingsUI.SetActive(false);
            ControlsUI.SetActive(false);
            SoundUI.SetActive(false);
            SceneryUI.SetActive(false);
            ScenesUI.SetActive(false);
            //HelpUI.SetActive(false);
        }
        else if(UIname == "Delete")
        {
            EditSceneUI.SetActive(false);
            ModelUI.SetActive(false);
            ChooseMediaUI.SetActive(false);
            UploadUI.SetActive(false);
            DeleteUI.SetActive(true);
            SettingsUI.SetActive(false);
            ControlsUI.SetActive(false);
            SoundUI.SetActive(false);
            SceneryUI.SetActive(false);
            ScenesUI.SetActive(false);
            //HelpUI.SetActive(false);
        }
        else if (UIname == "Sound")
        {
            EditSceneUI.SetActive(false);
            ModelUI.SetActive(false);
            ChooseMediaUI.SetActive(false);
            UploadUI.SetActive(false);
            DeleteUI.SetActive(false);
            SettingsUI.SetActive(false);
            ControlsUI.SetActive(false);
            SoundUI.SetActive(true);
            SceneryUI.SetActive(false);
            ScenesUI.SetActive(false);
            //HelpUI.SetActive(false);
        }
        else if (UIname == "Models")
        {
            EditSceneUI.SetActive(false);
            ModelUI.SetActive(true);
            ChooseMediaUI.SetActive(false);
            UploadUI.SetActive(false);
            DeleteUI.SetActive(false);
            SettingsUI.SetActive(false);
            ControlsUI.SetActive(false);
            SoundUI.SetActive(false);
            SceneryUI.SetActive(false);
            ScenesUI.SetActive(false);
            //HelpUI.SetActive(false);
        }
        else if (UIname == "Settings")
        {
            EditSceneUI.SetActive(false);
            ModelUI.SetActive(false);
            ChooseMediaUI.SetActive(false);
            UploadUI.SetActive(false);
            DeleteUI.SetActive(false);
            SettingsUI.SetActive(true);
            ControlsUI.SetActive(false);
            SoundUI.SetActive(false);
            SceneryUI.SetActive(false);
            ScenesUI.SetActive(false);
            //HelpUI.SetActive(false);
        }
        else if (UIname == "Scenery")
        {
            EditSceneUI.SetActive(false);
            ModelUI.SetActive(false);
            ChooseMediaUI.SetActive(false);
            UploadUI.SetActive(false);
            DeleteUI.SetActive(false);
            SettingsUI.SetActive(false);
            ControlsUI.SetActive(false);
            SoundUI.SetActive(false);
            SceneryUI.SetActive(true);
            ScenesUI.SetActive(false);
            //HelpUI.SetActive(false);
        }
        else if (UIname == "Scenes")
        {
            EditSceneUI.SetActive(false);
            ModelUI.SetActive(false);
            ChooseMediaUI.SetActive(false);
            UploadUI.SetActive(false);
            DeleteUI.SetActive(false);
            SettingsUI.SetActive(false);
            ControlsUI.SetActive(false);
            SoundUI.SetActive(false);
            SceneryUI.SetActive(false);
            ScenesUI.SetActive(true);
            //HelpUI.SetActive(false);
        }
    }

}
