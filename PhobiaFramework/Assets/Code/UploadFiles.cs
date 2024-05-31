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
using SimpleFileBrowser;
using static SimpleFileBrowser.FileBrowser;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Threading.Tasks;
using TMPro;
using System;
using System.Linq;
using System.IO;

// This script encapsulates attributes related to a trigger object, such as its path, position, rotation, size, and a flag indicating whether it's a copy or not.

public class UploadFiles : MonoBehaviour
{
    public Button chooseFileButton;
    public Button chooseIconButton;
    public Button uploadButton;
    public TMP_Dropdown dropdown;
    public TextMeshProUGUI message;
    public TextMeshProUGUI warningOrErrorMessage;
    public TextMeshProUGUI filePathText;
    public TextMeshProUGUI iconPathText;

    public TMP_InputField fileNameInput;

    string fileName = string.Empty;
    string filePath = null; 
    string iconPath = null;
    string fileType = string.Empty;

    bool fileChosen = false;
    bool iconChosen = false;

    DatabaseService dbService;

    void Start()
    {
        // Find the GameObject with the DatabaseService script
        GameObject databaseServiceObject = GameObject.Find("DatabaseService");

        // Check if the GameObject was found
        if (databaseServiceObject != null)
        {
            // Get the DatabaseService component from the found GameObject
            dbService = databaseServiceObject.GetComponent<DatabaseService>();
        }
        else
        {
            Debug.LogError("GameObject with DatabaseService not found.");
        }

        chooseIconButton.onClick.AddListener(chooseIcon);
        chooseFileButton.onClick.AddListener(chooseFile);
        uploadButton.onClick.AddListener(uploadFile);

        // Ensure that the dropdown is assigned in the Inspector
        if (dropdown != null)
        {
            // Add a listener to the dropdown's onValueChanged event
            dropdown.onValueChanged.AddListener(delegate {
                DropdownValueChanged(dropdown);
            });

            // Get the index of the initial option without user interaction
            int initialIndex = dropdown.value;

            // Get the text of the initial option
            fileType = dropdown.options[initialIndex].text;

            FileBrowser.SetFilters(true, new FileBrowser.Filter("Models", ".glb"));
            FileBrowser.SetDefaultFilter(".glb");
        }
        else
        {
            Debug.LogError("TMP Dropdown is not assigned.");
        }

        fileNameInput.onValueChanged.AddListener((x) => checkInput(fileNameInput.text));
    }

    public void checkInput(string input)
    {
        if (fileChosen && iconChosen)
        {
            if (!string.IsNullOrEmpty(input))
            {
                warningOrErrorMessage.text = "";
            }
        }
    }

    public void chooseIcon()
    {
        message.text = "";
        iconChosen = false;

        FileBrowser.SetFilters(true, new FileBrowser.Filter("Images", ".jpg", ".png", ".jpeg"));
        FileBrowser.SetDefaultFilter(".jpg");

        StartCoroutine(ShowLoadDialogCoroutine(PickMode.Files, setIconPath));
    }

    public void setIconPath(string[] paths)
    {
        if (paths != null || paths.Count() > 0)
        {
            iconPath = paths[0];
            iconChosen = true;
            iconPathText.text = "Path of file icon: " + iconPath;
        }
        else
        {
            iconChosen = false;
        }

        if (fileChosen && iconChosen)
        {
            warningOrErrorMessage.text = "";
        }
    }

    void DropdownValueChanged(TMP_Dropdown change)
    {
        message.text = "";
        fileType = change.options[change.value].text;
        filePathText.text = "";
        iconPathText.text = "";
        filePath = null;
        iconPath = null;
        fileChosen = false;
        iconChosen = false;
    }

    public void chooseFile()
    {
        message.text = "";
        iconChosen = false;

        if (fileType == "Model")
        {
            FileBrowser.SetFilters(true, new FileBrowser.Filter("Models", ".glb"));
            FileBrowser.SetDefaultFilter(".glb");
        }
        else if (fileType == "Texture" || fileType == "360 image")
        {
            FileBrowser.SetFilters(true, new FileBrowser.Filter("Images", ".jpg", ".png", ".jpeg"));
            FileBrowser.SetDefaultFilter(".jpg");
        }
        else if(fileType == "360 video")
        {
            FileBrowser.SetFilters(true, new FileBrowser.Filter("Videos", ".mp4", ".mov"));
            FileBrowser.SetDefaultFilter(".mp4");
        }
        else if(fileType == "Sound")
        {
            FileBrowser.SetFilters(true, new FileBrowser.Filter("Sound", ".mp3", ".WAV"));
            FileBrowser.SetDefaultFilter(".mp3");
        }
        else if (fileType == "Scenery")
        {
            FileBrowser.SetFilters(true, new FileBrowser.Filter("Scenery", ".glb"));
            FileBrowser.SetDefaultFilter(".glb");
        }

        StartCoroutine(ShowLoadDialogCoroutine(PickMode.Files, setFilePath));
    }

    public void setFilePath(string[] paths)
    {
        if (paths != null || paths.Count() > 0)
        {
            filePath = paths[0];
            fileChosen = true;
            filePathText.text = "Path of file: " + filePath;
        }
        else
        {
            fileChosen = false;
        }

        if (fileChosen && iconChosen)
        {
            warningOrErrorMessage.text = "";
        }
    }

    public async void uploadFile()
    {
        bool success = await uploadSelectedFile();
    }

    IEnumerator ShowLoadDialogCoroutine(PickMode pickMode, Action<string[]> callback)
    {
        yield return FileBrowser.WaitForLoadDialog(pickMode, true, null, null, "Load Files", "Load");

        if (FileBrowser.Success)
        {
            callback(FileBrowser.Result);
        }
    }

    public async Task<bool> uploadSelectedFile()
    {
        bool uploaded = false;
        fileName = fileNameInput.text;

        if ((fileChosen && iconChosen) && (!string.IsNullOrEmpty(fileName) && !string.IsNullOrEmpty(filePath) && !string.IsNullOrEmpty(iconPath)))
        {
            //string path = paths[0];

            if(File.Exists(filePath))
            {
                string fileExtension = Path.GetExtension(filePath);
                string iconExtension = Path.GetExtension(iconPath);

                /*
                dbService.addFile(filePath, fileName, fileExtension);
                dbService.addIcon(iconPath, fileName+"_icon", iconExtension);
                dbService.addFileData(fileName, fileExtension, iconExtension);
                */

                Debug.Log(fileType);

                bool success = dbService.addFile(filePath, fileName, fileType, fileExtension);
                
                if (success)
                {
                    await dbService.addIcon(iconPath, fileName + "_icon", fileType, iconExtension);
                    dbService.addFileData(fileName, fileType, fileExtension, iconExtension);
                    warningOrErrorMessage.text = "";
                    message.text = "File has been uploaded!";
                    uploaded = true;
                }
                else
                {
                    message.text = "";
                    warningOrErrorMessage.text = "Something went wrong!";
                }

                fileNameInput.text = "";
                filePathText.text = "";
                iconPathText.text = "";
                filePath = null; 
                iconPath = null;
                fileChosen = false;
                iconChosen = false;

                return uploaded;
            }
        }

        else if (string.IsNullOrEmpty(fileName)) 
        {
            message.text = "";
            warningOrErrorMessage.text = "File name cannot be empty!";
        }

        else if (string.IsNullOrEmpty(filePath) || string.IsNullOrEmpty(iconPath) || !fileChosen || !iconChosen) 
        {
            message.text = "";
            warningOrErrorMessage.text = "You need to choose both a file and an icon!";
        }

        return uploaded;
    }

}
