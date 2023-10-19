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

public class FirebaseService : Database
{
    FirebaseStorage storage;
    StorageReference storageRef;
    StorageReference gltfReference;
    DatabaseReference dbreference;
    public string databaseName;

    public void initialize()
    {
        storage = FirebaseStorage.DefaultInstance;
        storageRef = storage.GetReferenceFromUrl("gs://vr-framework-95ccc.appspot.com");

        dbreference = FirebaseDatabase.DefaultInstance.RootReference;
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

    public void addFile(string filePath, string fileName, string fileType)
    {
        StorageReference fileRef = null;

        if (fileType == "glb")
        {
            fileRef = storageRef.Child("models" + "/" + fileName + "." + fileType);
        }
        else if (fileType == "jpeg" || fileType == "png" || fileType == "jpg")
        {
            fileRef = storageRef.Child("photos" + "/" + fileName + "." + fileType);
        }
        else if (fileType == "mp4")
        {
            fileRef = storageRef.Child("videos" + "/" + fileName + "." + fileType);
        }
        //else if (fileType == "") // For animations

        if (fileRef != null)
        {
            // Upload the file to the path "images/rivers.jpg"
            fileRef.PutFileAsync(filePath)
                .ContinueWith((Task<StorageMetadata> task) => {
                    if (task.IsFaulted || task.IsCanceled)
                    {
                        Debug.Log(task.Exception.ToString());
                        // Uh-oh, an error occurred!
                    }
                    else
                    {
                        // Metadata contains file metadata such as size, content-type, and download URL.
                        StorageMetadata metadata = task.Result;
                        string md5Hash = metadata.Md5Hash;
                        Debug.Log("Finished uploading...");
                        Debug.Log("md5 hash = " + md5Hash);
                    }
            });
        }       
    }

    public void addFileData(string fileId, string fileName, string path, string filetype)
    {
        FileInfo fileinfo = new FileInfo(fileName, path, filetype);
        string json = JsonUtility.ToJson(fileinfo);

        dbreference.Child(filetype).Child(fileId).SetRawJsonValueAsync(json);
    }
}
