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

    public async Task<byte[]> getFile(string downloadUrl)
    {
        using (UnityWebRequest www = UnityWebRequest.Get(downloadUrl))
        {
            var asyncOperation = www.SendWebRequest();

            // Wait until the request is completed or times out
            while (!asyncOperation.isDone)
            {
                await Task.Yield();
            }

            if (www.result == UnityWebRequest.Result.Success)
            {
                byte[] binaryData = www.downloadHandler.data;
                return binaryData;
            }
            else
            {
                Debug.LogError("Error downloading file: " + www.error);
                return null; // or throw an exception
            }
        }
    }

    public void addIcon(string filePath, string iconFileName, string fileType, string iconExtension)
    {
        StorageReference fileRef = null;

        if (fileType == "Model")
        {
            fileRef = storageRef.Child("models/icons/" + iconFileName + iconExtension);
        }
        else if (fileType == "360 image")
        {
            fileRef = storageRef.Child("photos/icons/" + iconFileName + iconExtension);
        }
        else if (fileType == "360 video")
        {
            fileRef = storageRef.Child("videos/icons/" + iconFileName + iconExtension);
        }
        else if (fileType == "Texture")
        {
            fileRef = storageRef.Child("textures/icons/" + iconFileName + iconExtension);
        }

        if (fileRef != null)
        {
            // Upload the file to the path "images/rivers.jpg"
            fileRef.PutFileAsync(filePath)
                .ContinueWith((Task<StorageMetadata> task) =>
                {
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

    public void addFile(string filePath, string fileName, string fileType, string extension)
    {
        StorageReference fileRef = null;

        if (fileType == "Model")
        {
            fileRef = storageRef.Child("models/" + fileName + extension);
        }
        else if (fileType == "360 image")
        {
            fileRef = storageRef.Child("photos/" + fileName + extension);
        }
        else if (fileType == "360 video")
        {
            fileRef = storageRef.Child("videos/" + fileName + extension);
        }
        else if (fileType == "Texture")
        {
            fileRef = storageRef.Child("textures/" + fileName + extension);
        }

        if (fileRef != null)
        {
            fileRef.PutFileAsync(filePath)
                .ContinueWith((Task<StorageMetadata> task) =>
                {
                    if (task.IsFaulted || task.IsCanceled)
                    {
                        Debug.Log(task.Exception.ToString());
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

    public void addFileData(string fileName, string fileType, string extension, string iconExtension)
    {
        string uniqueID = Guid.NewGuid().ToString(); // Generating a unique ID
        string path = null;
        string pathToIcon = null;
        string childStr = null;

        if (fileType == "Model")
        {
            path = databaseURL + "/models/" + fileName + extension;
            pathToIcon = databaseURL + "/models/icons/" + fileName + "_icon" + iconExtension;
            childStr = "glb";
        }
        else if (fileType == "360 image")
        {
            path = databaseURL + "/photos/" + fileName + extension;
            pathToIcon = databaseURL + "/photos/icons/" + fileName + "_icon" + iconExtension;
            childStr = "360images";
        }
        else if (fileType == "360 video")
        {
            path = databaseURL + "/videos/" + fileName + extension;
            pathToIcon = databaseURL + "/videos/icons/" + fileName + "_icon" + iconExtension;
            childStr = "360videos";
        }
        else if (fileType == "Texture")
        {
            path = databaseURL + "/textures/" + fileName + extension;
            pathToIcon = databaseURL + "/textures/icons/" + fileName + "_icon" + iconExtension;
            childStr = "textures";
        }

        if (path != null && pathToIcon != null && childStr != null)
        {
            FileMetaData fileMetaData = new FileMetaData(fileName, fileType.Replace(".", ""), path, pathToIcon);
            string json = JsonUtility.ToJson(fileMetaData);

            dbreference.Child(childStr).Child(uniqueID).SetRawJsonValueAsync(json);

            Debug.Log("After uploading file data");
        }
        else
        {
            Debug.Log("This filetype cannot be uploaded to database!");
        }
    }

    public IEnumerator getAllModelFileData(Action<List<FileMetaData>> callback)
    {
        var task = dbreference.Child("glb").GetValueAsync();
        yield return new WaitUntil(() => task.IsCompleted);

        if (task.Exception != null)
        {
            Debug.LogError("Error retrieving data: " + task.Exception);
        }
        else if (task.Result != null)
        {
            List<FileMetaData> files = new List<FileMetaData>();
            DataSnapshot snapshot = task.Result;

            foreach (var child in snapshot.Children)
            {
                if (child != null && child.Child("filename") != null && child.Child("filetype") != null && child.Child("path") != null && child.Child("pathToIcon") != null)
                {
                    Debug.Log("Key: " + child.Key);
                    Debug.Log("Value: " + child.GetRawJsonValue());

                    string filename = child.Child("filename").Value.ToString();
                    string filetype = child.Child("filetype").Value.ToString();
                    string path = child.Child("path").Value.ToString();
                    string pathToIcon = child.Child("pathToIcon").Value.ToString();

                    FileMetaData fileData = new FileMetaData(filename, filetype, path, pathToIcon);
                    files.Add(fileData);
                }
                else
                {
                    Debug.LogError("One of the child properties is null.");
                }
            }

            callback(files);

        }
    }

    public IEnumerator getAll360Media(Action<List<FileMetaData>> callback)
    {
        var task = dbreference.Child("glb").GetValueAsync();
        yield return new WaitUntil(() => task.IsCompleted);

        if (task.Exception != null)
        {
            Debug.LogError("Error retrieving data: " + task.Exception);
        }
        else if (task.Result != null)
        {
            List<FileMetaData> files = new List<FileMetaData>();
            DataSnapshot snapshot = task.Result;

            foreach (var child in snapshot.Children)
            {
                if (child != null && child.Child("filename") != null && child.Child("filetype") != null && child.Child("path") != null && child.Child("pathToIcon") != null)
                {
                    Debug.Log("Key: " + child.Key);
                    Debug.Log("Value: " + child.GetRawJsonValue());

                    string filename = child.Child("filename").Value.ToString();
                    string filetype = child.Child("filetype").Value.ToString();
                    string path = child.Child("path").Value.ToString();
                    string pathToIcon = child.Child("pathToIcon").Value.ToString();

                    FileMetaData fileData = new FileMetaData(filename, filetype, path, pathToIcon);
                    files.Add(fileData);
                }
                else
                {
                    Debug.LogError("One of the child properties is null.");
                }
            }

            callback(files);
        }

    }
}
