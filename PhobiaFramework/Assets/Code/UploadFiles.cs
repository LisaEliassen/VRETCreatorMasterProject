using System.Collections;
using System.Collections.Generic;
using SimpleFileBrowser;
using static SimpleFileBrowser.FileBrowser;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using System;
using System.Linq;
using System.IO;

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

    // Start is called before the first frame update
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

    public void uploadFile()
    {
        StartCoroutine(uploadSelectedFile());
    }

    IEnumerator ShowLoadDialogCoroutine(PickMode pickMode, Action<string[]> callback)
    {
        yield return FileBrowser.WaitForLoadDialog(pickMode, true, null, null, "Load Files", "Load");

        if (FileBrowser.Success)
        {
            callback(FileBrowser.Result);
        }
    }

    IEnumerator uploadSelectedFile()
    {
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
                    dbService.addIcon(iconPath, fileName + "_icon", fileType, iconExtension);
                    dbService.addFileData(fileName, fileType, fileExtension, iconExtension);
                    warningOrErrorMessage.text = "";
                    message.text = "File has been uploaded!";
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

                yield return null;
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
    }

}
