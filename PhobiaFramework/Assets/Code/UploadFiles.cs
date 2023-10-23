using System.Collections;
using System.Collections.Generic;
using SimpleFileBrowser;
using static SimpleFileBrowser.FileBrowser;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using System.IO;

public class UploadFiles : MonoBehaviour
{
    public Button chooseFileButton;
    public Button chooseIconButton;
    public Button uploadButton;
    public TextMeshProUGUI warningOrErrorMessage;
    public TextMeshProUGUI filePathText;
    public TextMeshProUGUI iconPathText;

    public TMP_InputField fileNameInput;

    string fileName = string.Empty;
    string filePath = string.Empty;
    string iconPath = string.Empty;

    bool fileChosen = false;
    bool iconChosen = false;

    DatabaseService dbService;

    // Start is called before the first frame update
    void Start()
    {
        // Find the GameObject with the DatabaseService script
        GameObject databaseServiceObject = GameObject.Find("DatabaseService");
        //dbService = new DatabaseService("Firebase");

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
    }

    public void chooseIcon()
    {
        iconChosen = false;

        FileBrowser.SetFilters(true, new FileBrowser.Filter("Images", ".jpg", ".png", ".jpeg"));
        FileBrowser.SetDefaultFilter(".jpg");

        StartCoroutine(ShowLoadDialogCoroutine(PickMode.Files, setIconPath));
    }

    public void setIconPath(string[] paths)
    {
        iconPath = paths[0];
        iconChosen = true;
        iconPathText.text = "Path of file icon: " + iconPath;
    }

    public void chooseFile()
    {
        iconChosen = false;

        FileBrowser.SetFilters(true, new FileBrowser.Filter("Models", ".glb"), new FileBrowser.Filter("Animations", ".anim"),
            new FileBrowser.Filter("Images", ".jpg", ".png"), new FileBrowser.Filter("Videos", ".mp4", ".mov"));
        FileBrowser.SetDefaultFilter(".glb");

        StartCoroutine(ShowLoadDialogCoroutine(PickMode.Files, setFilePath));
    }

    public void setFilePath(string[] paths)
    {
        filePath = paths[0];
        fileChosen = true;
        filePathText.text = "Path of file: " + filePath;
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
                /*dbService.addFileData(Path.GetFileNameWithoutExtension(path), extension);
                dbService.addFile(path, Path.GetFileNameWithoutExtension(path), extension);*/
                dbService.addFile(filePath, fileName, fileExtension);
                dbService.addIcon(iconPath, fileName+"_icon", iconExtension);
                dbService.addFileData(fileName, fileExtension, iconExtension);
                yield return null;
            }
        }

        else if (string.IsNullOrEmpty(fileName)) 
        {
            warningOrErrorMessage.text = "File name cannot be empty!";
        }

        else if (string.IsNullOrEmpty(filePath) || string.IsNullOrEmpty(iconPath)) 
        {
            warningOrErrorMessage.text = "You need to choose both a file and an icon!";
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!string.IsNullOrEmpty(fileNameInput.text) || (!string.IsNullOrEmpty(filePath) && !string.IsNullOrEmpty(iconPath)))
        {
            warningOrErrorMessage.text = "";
        }
    }
}
