using Firebase.Storage;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GLTFast;
using GLTFast.Schema;
using Firebase.Extensions;
using UnityEngine.Networking;
using System;
using System.Threading.Tasks;
using static System.Net.WebRequestMethods;

public class DatabaseService : MonoBehaviour
{
    FirebaseStorage storage;
    StorageReference gltfReference;

    void Start()
    {
        storage = FirebaseStorage.DefaultInstance;
    }

    // Returns the download URL of given Database file URL
    public async Task<string> GetDownloadURL(string fileUrl)
    {
        string downloadUrl = "";
        gltfReference =
            storage.GetReferenceFromUrl(fileUrl);
        await gltfReference.GetDownloadUrlAsync().ContinueWithOnMainThread(task =>
        {
            if (!task.IsFaulted && !task.IsCanceled)
            {
                downloadUrl = task.Result.ToString();
                Debug.Log("Download URL: " + downloadUrl);

                // Download the file via UnityWebRequest
            }
            else
            {
                Debug.Log("Failed to get download URL from reference!");
            }
        });
        return downloadUrl;
    }
}
