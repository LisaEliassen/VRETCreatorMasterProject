using Firebase;
using Firebase.Extensions;
using Firebase.Database;
using Firebase.Storage;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GLTFast;
using GLTFast.Schema;
using UnityEngine.Networking;
using System;
using System.Threading.Tasks;
using static System.Net.WebRequestMethods;

public class DatabaseService : MonoBehaviour
{
    FirebaseStorage storage;
    StorageReference storageRef;
    StorageReference gltfReference;
    DatabaseReference dbreference;
    public string databaseName;

    void Start()
    {
        if (databaseName == null)
        {

        }
        else if (databaseName == "Firebase")
        {
            storage = FirebaseStorage.DefaultInstance;
            storageRef = storage.GetReferenceFromUrl("gs://vr-framework-95ccc.appspot.com");

            dbreference = FirebaseDatabase.DefaultInstance.RootReference;

            addDataForAllModels();
        }
        else if (databaseName == "Azure")
        {

        }
    }

    // Returns the download URL of given Database file URL
    public async Task<string> GetDownloadURL(string fileUrl)
    {
        string downloadUrl = "";
        gltfReference = storage.GetReferenceFromUrl(fileUrl);

        await gltfReference.GetDownloadUrlAsync().ContinueWithOnMainThread(task =>
        {
            if (!task.IsFaulted && !task.IsCanceled)
            {
                downloadUrl = task.Result.ToString();
                //Debug.Log("Download URL: " + downloadUrl);
            }
            else
            {
                Debug.Log("Failed to get download URL from reference!");
            }
        });
        return downloadUrl;
    }

    public void addFileData(string fileId, string fileName, string path, string filetype)
    {
        FileInfo fileinfo = new FileInfo(fileName, path, filetype);
        string json = JsonUtility.ToJson(fileinfo);

        dbreference.Child(filetype).Child(fileId).SetRawJsonValueAsync(json);
    }

    public void addDataForAllModels()
    {

    }
}
