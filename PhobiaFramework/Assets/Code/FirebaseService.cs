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
using System.IO;
using System.Threading.Tasks;
using static System.Net.WebRequestMethods;
using System.Linq;

// The script demonstrates a FirebaseService class for managing files and data on Firebase Storage and Firebase Realtime Database in a Unity application.


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

    public async Task<AudioClip> getAudioClip(string downloadUrl)
    {
        string extension = Path.GetExtension(downloadUrl);
        AudioType audioType = GetAudioTypeFromExtension(extension);

        using (UnityWebRequest www = UnityWebRequestMultimedia.GetAudioClip(downloadUrl, audioType))
        {
            var asyncOperation = www.SendWebRequest();

            // Wait until the request is completed or times out
            while (!asyncOperation.isDone)
            {
                await Task.Yield();
            }

            if (www.result == UnityWebRequest.Result.ConnectionError)
            {
                Debug.Log(www.error);
                return null;
            }
            else
            {
                AudioClip myClip = DownloadHandlerAudioClip.GetContent(www);
                return myClip;
            }
        }
    }

    private AudioType GetAudioTypeFromExtension(string extension)
    {
        if (extension.Equals(".mp3", StringComparison.OrdinalIgnoreCase))
        {
            return AudioType.MPEG;
        }
        else if (extension.Equals(".wav", StringComparison.OrdinalIgnoreCase))
        {
            return AudioType.WAV;
        }
        else
        {
            // Default to MP3 if the extension is not recognized
            return AudioType.MPEG;
        }
    }

    public async Task<bool> addIcon(string filePath, string iconFileName, string fileType, string iconExtension)
    {
        bool success = false;
        StorageReference fileRef = null;

        if (fileType == "Model")
        {
            fileRef = storageRef.Child("models/icons/" + iconFileName + iconExtension);
        }
        else if (fileType == "360 image")
        {
            fileRef = storageRef.Child("images/icons/" + iconFileName + iconExtension);
        }
        else if (fileType == "360 video")
        {
            fileRef = storageRef.Child("videos/icons/" + iconFileName + iconExtension);
        }
        else if (fileType == "Texture")
        {
            fileRef = storageRef.Child("textures/icons/" + iconFileName + iconExtension);
        }
        else if (fileType == "Sound")
        {
            fileRef = storageRef.Child("sound/icons/" + iconFileName + iconExtension);
        }
        else if (fileType == "Scenery")
        {
            fileRef = storageRef.Child("scenery/icons/" + iconFileName + iconExtension);
        }
        else if (fileType == "Scene")
        {
            fileRef = storageRef.Child("scenes/icons/" + iconFileName + iconExtension);
        }

        if (fileRef != null)
        {
            await fileRef.PutFileAsync(filePath)
                .ContinueWith((Task<StorageMetadata> task) =>
                {
                    if (task.IsFaulted)
                    {
                        Debug.Log(task.Exception.ToString());
                        success = false;
                    }
                    else if (task.IsCanceled)
                    {
                        success = true;
                    }
                    // Metadata contains file metadata such as size, content-type, and download URL.
                    StorageMetadata metadata = task.Result;
                    string md5Hash = metadata.Md5Hash;
                    Debug.Log("Finished uploading...");
                    Debug.Log("md5 hash = " + md5Hash);
                });
        }
        else
        {
            Debug.Log("Failed to set StorageReference for file!");
        }
        return success;
    }

    public bool addFile(string filePath, string fileName, string fileType, string extension)
    {
        StorageReference fileRef = null;

        if (fileType == "Model")
        {
            fileRef = storageRef.Child("models/" + fileName + extension);
        }
        else if (fileType == "360 image")
        {
            fileRef = storageRef.Child("images/" + fileName + extension);
        }
        else if (fileType == "360 video")
        {
            fileRef = storageRef.Child("videos/" + fileName + extension);
        }
        else if (fileType == "Texture")
        {
            fileRef = storageRef.Child("textures/" + fileName + extension);
        }
        else if (fileType == "Sound")
        {
            fileRef = storageRef.Child("sound/" + fileName + extension);
        }
        else if (fileType == "Scenery")
        {
            fileRef = storageRef.Child("scenery/" + fileName + extension);
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
            return true;
        }
        else
        {
            Debug.Log("Failed to set StorageReference for file!");
            return false;
        }
    }

    public async void addFileData(string fileName, string fileType, string extension, string iconExtension)
    {
        string uniqueID = Guid.NewGuid().ToString(); // Generating a unique ID
        string path = null;
        string pathToIcon = null;
        string childStr = null;

        if (fileType == "Model")
        {
            path = databaseURL + "/models/" + fileName + extension;
            pathToIcon = databaseURL + "/models/icons/" + fileName + "_icon" + iconExtension;
            childStr = "models";
        }
        else if (fileType == "360 image")
        {
            path = databaseURL + "/images/" + fileName + extension;
            pathToIcon = databaseURL + "/images/icons/" + fileName + "_icon" + iconExtension;
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
        else if (fileType == "Sound")
        {
            path = databaseURL + "/sound/" + fileName + extension;
            pathToIcon = databaseURL + "/sound/icons/" + fileName + "_icon" + iconExtension;
            childStr = "sound";
        }
        else if (fileType == "Scenery")
        {
            path = databaseURL + "/scenery/" + fileName + extension;
            pathToIcon = databaseURL + "/scenery/icons/" + fileName + "_icon" + iconExtension;
            childStr = "scenery";
        }

        if (path != null && pathToIcon != null && childStr != null)
        {
            FileMetaData fileMetaData = new FileMetaData(uniqueID, fileName, fileType.Replace(".", ""), path, pathToIcon);

            // Check if the entry already exists
            bool entryExists = await FileDataExists(fileMetaData, childStr);

            if (entryExists)
            {
                Debug.Log("File already exists, and has been updated!");
            }
            else
            {
                string json = JsonUtility.ToJson(fileMetaData);
                await dbreference.Child(childStr).Child(uniqueID).SetRawJsonValueAsync(json);
                Debug.Log("After uploading file data");
            }
        }
        else
        {
            Debug.Log("This filetype cannot be uploaded to database!");
        }
    }

    public async Task<bool> addSceneData(string sceneName, Trigger[] triggers, string pathTo360Media, string pathToAudio, SceneryObject[] scenery)
    {
        bool success = false;
        string uniqueID = Guid.NewGuid().ToString(); // Generating a unique ID
        string iconExtension = Path.GetExtension("screenshot.png");
        string pathToSceneIcon = databaseURL + "/scenes/icons/" + sceneName + "_icon" + iconExtension;

        SceneMetaData sceneData = new SceneMetaData(uniqueID, sceneName, pathToSceneIcon, triggers, pathTo360Media, pathToAudio, scenery);
        bool entryExists = await SceneDataExists(sceneData, "scenes");
        
        if (entryExists)
        {
            Debug.Log("File already exists, and has been updated!");
        }
        else
        {
            string json = JsonUtility.ToJson(sceneData);
            await dbreference.Child("scenes").Child(uniqueID).SetRawJsonValueAsync(json).ContinueWith(task =>
            {
                if (task.IsFaulted)
                {
                    // There was an error while setting the data
                    Debug.LogError("Error setting data: " + task.Exception);
                    success = false;
                }
                else if (task.IsCompleted)
                {
                    // Data setting succeeded
                    Debug.Log("Data set successfully");
                    success = true;
                }
            });
            Debug.Log("After uploading file data");
        }
        return success;
    }

    public IEnumerator getAllScenesFileData(Action<List<SceneMetaData>> callback)
    {
        var task = dbreference.Child("scenes").GetValueAsync();
        yield return new WaitUntil(() => task.IsCompleted);

        if(task.Exception != null)
        {
            Debug.LogError("Error retrieving data: " + task.Exception);
        }
        else if (task.Result != null)
        {
            List<SceneMetaData> files = new List<SceneMetaData>();
            DataSnapshot snapshot = task.Result;
            
            foreach (var child in snapshot.Children)
            {
                if (child != null && child.Child("sceneName") != null && child.Child("pathToTrigger") != null && child.Child("triggerLocation") != null && child.Child("triggerSize") != null && child.Child("pathTo360Media") != null
                    && child.Child("pathToAudio") != null && child.Child("pathsToScenery") != null && child.Child("sceneryLocations") != null && child.Child("scenerySizes") != null)
                {
                    //Debug.Log("Key: " + child.Key);
                    //Debug.Log("Value: " + child.GetRawJsonValue());

                    string uniqueID = child.Key;
                    string sceneName = child.Child("sceneName").Value.ToString();
                    string pathToSceneIcon = child.Child("pathToSceneIcon").Value.ToString();

                    List<Trigger> triggers = new List<Trigger>();
                    var triggerArray = child.Child("triggers");

                    foreach (var triggerChild in triggerArray.Children)
                    {
                        if (triggerChild != null)
                        {
                            string triggerPath = triggerChild.Child("path").Value.ToString();
                            string triggerSize = triggerChild.Child("size").Value.ToString();
                            string triggerPosition = triggerChild.Child("position").Value.ToString();
                            string triggerRotation = triggerChild.Child("rotation").Value.ToString();
                            string isCopy = triggerChild.Child("isCopy").Value.ToString();

                            Trigger trigger = new Trigger(triggerPath, triggerPosition, triggerRotation, triggerSize, isCopy);
                            triggers.Add(trigger);
                        }
                    }

                    string pathTo360Media = child.Child("pathTo360Media").Value.ToString();
                    string pathToAudio = child.Child("pathToAudio").Value.ToString();

                    List<SceneryObject> sceneryObjects = new List<SceneryObject>();
                    var sceneryArray = child.Child("scenery");
                    foreach (var sceneryChild in sceneryArray.Children)
                    {
                        if (sceneryChild != null) 
                        {
                            string sceneryName = sceneryChild.Child("name").Value.ToString();
                            string sceneryPath = sceneryChild.Child("path").Value.ToString();
                            string sceneryPosition = sceneryChild.Child("position").Value.ToString();
                            string sceneryRotation = sceneryChild.Child("rotation").Value.ToString();
                            string scenerySize = sceneryChild.Child("size").Value.ToString();

                            SceneryObject sceneryObject = new SceneryObject(sceneryName, sceneryPath, sceneryPosition, sceneryRotation, scenerySize);
                            sceneryObjects.Add(sceneryObject);
                        }
                    }

                    SceneMetaData sceneData = new SceneMetaData(uniqueID, sceneName, pathToSceneIcon, triggers.ToArray(), pathTo360Media, pathToAudio, sceneryObjects.ToArray());
                    files.Add(sceneData);
                }
                else
                {
                    Debug.LogError("One of the child properties is null.");
                }
            }

            callback(files);
        }
        else if (task.Result == null)
        {
            Debug.Log("Error retrieving scenes!");
        }
    }


    public async Task<bool> SceneDataExists(SceneMetaData sceneData, string databaseGroup)
    {
        var query = dbreference.Child(databaseGroup)
                               .OrderByChild("sceneName")
                               .EqualTo(sceneData.GetSceneName());

        var snapshot = await query.GetValueAsync();

        if (!snapshot.Exists)
        {
            return false;
        }

        foreach (var child in snapshot.Children)
        {
            // Compare each property except ID
            if (child.Child("sceneName").Value.ToString() == sceneData.GetSceneName()) 
            {
                // All properties match, consider it as a match
                return true;
            }
        }

        return false;
    }

    public async Task<bool> FileDataExists(FileMetaData fileData, string databaseGroup)
    {
        var query = dbreference.Child(databaseGroup)
                               .OrderByChild("filename")
                               .EqualTo(fileData.GetFilename());

        var snapshot = await query.GetValueAsync();

        if (!snapshot.Exists)
        {
            return false;
        }

        foreach (var child in snapshot.Children)
        {
            // Compare each property except ID
            if (child.Child("filename").Value.ToString() == fileData.GetFilename() &&
                child.Child("filetype").Value.ToString() == fileData.GetFileType() &&
                child.Child("path").Value.ToString() == fileData.GetPath() &&
                child.Child("pathToIcon").Value.ToString() == fileData.GetPathToIcon())
            {
                // All properties match, consider it as a match
                return true;
            }
        }

        return false;
    }

    public IEnumerator getAllModelFileData(Action<List<FileMetaData>> callback)
    {
        var task = dbreference.Child("models").GetValueAsync();
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

                    string uniqueID = child.Key;
                    string filename = child.Child("filename").Value.ToString();
                    string filetype = child.Child("filetype").Value.ToString();
                    string path = child.Child("path").Value.ToString();
                    string pathToIcon = child.Child("pathToIcon").Value.ToString();

                    FileMetaData fileData = new FileMetaData(uniqueID, filename, filetype, path, pathToIcon);
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

    public IEnumerator getAllSceneryFileData(Action<List<FileMetaData>> callback)
    {
        var task = dbreference.Child("scenery").GetValueAsync();
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
                    //Debug.Log("Key: " + child.Key);
                    //Debug.Log("Value: " + child.GetRawJsonValue());

                    string uniqueID = child.Key;
                    string filename = child.Child("filename").Value.ToString();
                    string filetype = child.Child("filetype").Value.ToString();
                    string path = child.Child("path").Value.ToString();
                    string pathToIcon = child.Child("pathToIcon").Value.ToString();

                    FileMetaData fileData = new FileMetaData(uniqueID, filename, filetype, path, pathToIcon);
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
        List<FileMetaData> files = new List<FileMetaData>();

        var taskImages = dbreference.Child("360images").GetValueAsync();
        yield return new WaitUntil(() => taskImages.IsCompleted);

        var taskVideos = dbreference.Child("360videos").GetValueAsync();
        yield return new WaitUntil(() => taskVideos.IsCompleted);

        if (taskImages.Exception != null)
        {
            Debug.LogError("Error retrieving data: " + taskImages.Exception);
        }
        else if (taskVideos.Exception != null)
        {
            Debug.LogError("Error retrieving data: " + taskVideos.Exception);
        }
        else {
            if (taskImages.Result != null) {
                DataSnapshot snapshot = taskImages.Result;

                foreach (var child in snapshot.Children)
                {
                    if (child != null && child.Child("filename") != null && child.Child("filetype") != null && child.Child("path") != null && child.Child("pathToIcon") != null)
                    {
                        Debug.Log("Key: " + child.Key);
                        Debug.Log("Value: " + child.GetRawJsonValue());

                        string uniqueID = child.Key;
                        string filename = child.Child("filename").Value.ToString();
                        string filetype = child.Child("filetype").Value.ToString();
                        string path = child.Child("path").Value.ToString();
                        string pathToIcon = child.Child("pathToIcon").Value.ToString();

                        FileMetaData fileData = new FileMetaData(uniqueID, filename, filetype, path, pathToIcon);
                        files.Add(fileData);
                    }
                    else
                    {
                        Debug.LogError("One of the child properties is null.");
                    }
                }
            }
            if (taskVideos.Result != null)
            {
                DataSnapshot snapshot = taskVideos.Result;

                foreach (var child in snapshot.Children)
                {
                    if (child != null && child.Child("filename") != null && child.Child("filetype") != null && child.Child("path") != null && child.Child("pathToIcon") != null)
                    {
                        Debug.Log("Key: " + child.Key);
                        Debug.Log("Value: " + child.GetRawJsonValue());

                        string uniqueID = child.Key;
                        string filename = child.Child("filename").Value.ToString();
                        string filetype = child.Child("filetype").Value.ToString();
                        string path = child.Child("path").Value.ToString();
                        string pathToIcon = child.Child("pathToIcon").Value.ToString();

                        FileMetaData fileData = new FileMetaData(uniqueID, filename, filetype, path, pathToIcon);
                        files.Add(fileData);
                    }
                    else
                    {
                        Debug.LogError("One of the child properties is null.");
                    }
                }
            }

            callback(files);
        }
    }

    public IEnumerator getAllSoundMedia(Action<List<FileMetaData>> callback)
    {
        var task = dbreference.Child("sound").GetValueAsync();
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
                    //Debug.Log("Key: " + child.Key);
                    //Debug.Log("Value: " + child.GetRawJsonValue());

                    string uniqueID = child.Key;
                    string filename = child.Child("filename").Value.ToString();
                    string filetype = child.Child("filetype").Value.ToString();
                    string path = child.Child("path").Value.ToString();
                    string pathToIcon = child.Child("pathToIcon").Value.ToString();

                    FileMetaData fileData = new FileMetaData(uniqueID, filename, filetype, path, pathToIcon);
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

    public void deleteFile(string fileName, string fileType, FileMetaData fileData)
    {
        StorageReference fileRef = null;
        DatabaseReference fileDataRef = null;
        string extension = Path.GetExtension(fileData.path);

        if (fileType == "Model")
        {
            fileRef = storageRef.Child("models/" + fileName + extension);
            fileDataRef = dbreference.Child("models/" + fileData.GetID());
        }
        else if (fileType == "360 image")
        {
            fileRef = storageRef.Child("images/" + fileName + extension);
            fileDataRef = dbreference.Child("360images/" + fileData.GetID());
        }
        else if (fileType == "360 video")
        {
            fileRef = storageRef.Child("videos/" + fileName + extension);
            fileDataRef = dbreference.Child("360videos/" + fileData.GetID());
        }
        else if (fileType == "Texture")
        {
            fileRef = storageRef.Child("textures/" + fileName + extension);
            fileDataRef = dbreference.Child("textures/" + fileData.GetID());
        }

        if (fileRef != null)
        {
            fileRef.DeleteAsync().ContinueWithOnMainThread(task => {
                if (task.IsCompleted)
                {
                    Debug.Log("File deleted successfully.");
                }
                else
                {
                    Debug.LogError("Datareference for file could not be found!");
                }
            });

            deleteFileIcon(fileName, fileType, fileData.GetPathToIcon());
        }
        else
        {
            Debug.Log("Failed to set StorageReference for file!");
        }

        if (fileDataRef != null)
        {
            fileDataRef.RemoveValueAsync().ContinueWithOnMainThread(task => {
                if (task.IsCompleted)
                {
                    Debug.Log("Value deleted successfully");
                }
                else if (task.IsFaulted)
                {
                    Debug.LogError("Failed to delete value: " + task.Exception);
                }
            });
        }
    }

    public void deleteFileIcon(string fileName, string fileType, string iconPath)
    {
        StorageReference fileRef = null;
        string extension = Path.GetExtension(iconPath);

        if (fileType == "Model")
        {
            fileRef = storageRef.Child("models/icons/" + fileName + "_icon" + extension);
        }
        else if (fileType == "360 image")
        {
            fileRef = storageRef.Child("images/icons/" + fileName + "_icon" + extension);
        }
        else if (fileType == "360 video")
        {
            fileRef = storageRef.Child("videos/icons/" + fileName + "_icon" + extension);
        }
        else if (fileType == "Texture")
        {
            fileRef = storageRef.Child("textures/icons/" + fileName + "_icon" + extension);
        }

        if (fileRef != null)
        {
            fileRef.DeleteAsync().ContinueWithOnMainThread(task => {
                if (task.IsCompleted)
                {
                    Debug.Log("File deleted successfully.");
                }
                else
                {
                    Debug.LogError("Datareference for file could not be found!");
                }
            });
        }
        else
        {
            Debug.Log("Failed to set StorageReference for file!");
        }
    }
}
