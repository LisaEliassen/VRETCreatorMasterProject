using System.Collections;
using System.Collections.Generic;
using SimpleFileBrowser;
using static SimpleFileBrowser.FileBrowser;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.IO;

public class UploadFiles : MonoBehaviour
{
    public Button choosefileButton;
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

        FileBrowser.SetFilters(true, new FileBrowser.Filter("Models", ".glb"), new FileBrowser.Filter("Animations", ".anim"),
            new FileBrowser.Filter("Images", ".jpg", ".png"), new FileBrowser.Filter("Videos", ".mp4", ".mov"));
        FileBrowser.SetDefaultFilter(".glb");

        choosefileButton.onClick.AddListener(chooseFile);
    }

    public void chooseFile()
    {
        StartCoroutine(ShowLoadDialogCoroutine(PickMode.Files, uploadSelectedFile));
    }

    IEnumerator ShowLoadDialogCoroutine(PickMode pickMode, Action<string[]> callback)
    {
        yield return FileBrowser.WaitForLoadDialog(pickMode, true, null, null, "Load Files", "Load");

        if (FileBrowser.Success)
        {
            callback(FileBrowser.Result);
        }
    }

    private void uploadSelectedFile(string[] paths)
    {
        if (paths.Length > 0)
        {
            string path = paths[0];

            if(File.Exists(path))
            {
                string extension = Path.GetExtension(path);
                dbService.addFileData(Path.GetFileNameWithoutExtension(path), extension);
                dbService.addFile(path, Path.GetFileNameWithoutExtension(path), extension);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
