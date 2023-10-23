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
    string databaseURL;
    public string databaseName;

    public void initialize()
    {
        databaseURL = "gs://vr-framework-95ccc.appspot.com";
        storage = FirebaseStorage.DefaultInstance;
        storageRef = storage.GetReferenceFromUrl(databaseURL);

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

    public void addIcon(string filePath, string iconFileName, string fileType)
    {
        StorageReference fileRef = null;

        if (fileType == ".jpeg" || fileType == ".png" || fileType == ".jpg")
        {
            fileRef = storageRef.Child("models/icons/" + iconFileName + fileType);
        }

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
        else
        {
            Debug.Log("Failed to set StorageReference for file!");
        }
    }

    public void addFile(string filePath, string fileName, string fileType)
    {
        StorageReference fileRef = null;

        if (fileType == ".glb")
        {
            fileRef = storageRef.Child("models/" + fileName + fileType);
        }
        else if (fileType == ".jpeg" || fileType == ".png" || fileType == ".jpg")
        {
            fileRef = storageRef.Child("photos/" + fileName + fileType);
        }
        else if (fileType == ".mp4" || fileType == ".mov")
        {
            fileRef = storageRef.Child("videos/" + fileName + fileType);
        }
        else if (fileType == ".anim") 
        {
            fileRef = storageRef.Child("animations/" + fileName + fileType);
        }

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
        else
        {
            Debug.Log("Failed to set StorageReference for file!");
        }
    }

    public void addFileData(string fileName, string fileType, string iconFileType)
    {
        string uniqueID = Guid.NewGuid().ToString(); // Generating a unique ID
        string path = null;
        string pathToIcon = null;

        if (fileType == ".glb")
        {
            path = databaseURL + "/models/" + fileName + fileType;
            pathToIcon = databaseURL + "/models/icons/" + fileName + "_icon" + iconFileType;
        }
        else if (fileType == ".jpeg" || fileType == ".png" || fileType == ".jpg")
        {
            path = databaseURL + "/photos/" + fileName + fileType;
            pathToIcon = databaseURL + "/photos/icons/" + fileName + "_icon" + iconFileType;
        }
        else if (fileType == ".mp4" || fileType == ".mov")
        {
            path = databaseURL + "/videos/" + fileName + fileType;
            pathToIcon = databaseURL + "/videos/icons/" + fileName + "_icon" + iconFileType;
        }
        else if (fileType == ".anim")
        {
            path = databaseURL + "/animations/" + fileName + fileType;
            pathToIcon = databaseURL + "/animations/icons/" + fileName + "_icon" + iconFileType;
        }

        if (path != null && pathToIcon != null)
        {
            FileMetaData fileMetaData = new FileMetaData(fileName, fileType.Replace(".", ""), path, pathToIcon);
            string json = JsonUtility.ToJson(fileMetaData);

            dbreference.Child(fileType.Replace(".", "")).Child(uniqueID).SetRawJsonValueAsync(json);

            Debug.Log("After uploading file data");
        }
        else
        {
            Debug.Log("This filetype cannot be uploaded to database!");
        }
    }

    public void getAllModelFileData()
    {
        dbreference.GetValueAsync().ContinueWith(task =>
        {
            if (task.IsFaulted)
            {
                Debug.LogError("Error retrieving data: " + task.Exception);
            }
            else if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;
                // Iterate through the snapshot to access data
                foreach (var child in snapshot.Children)
                {
                    Debug.Log("Key: " + child.Key);
                    Debug.Log("Value: " + child.GetRawJsonValue());
                }
            }
        });
    }
}
