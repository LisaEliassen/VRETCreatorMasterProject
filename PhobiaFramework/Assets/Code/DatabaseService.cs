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
using System.Threading.Tasks;
using static System.Net.WebRequestMethods;

// The script acts as a mediator between different database implementations (such as Firebase and Azure) and the rest of the application. 

public class DatabaseService : MonoBehaviour
{
    Database database;
    public string databaseName;

    void Start()
    {
        if (databaseName == null)
        {

        }
        else if (databaseName == "Firebase")
        {
            database = new FirebaseService();
            database.initialize();
        }
        else if (databaseName == "Azure")
        {

        }
    }

    // Returns the download URL of given Database file URL
    public async Task<string> GetDownloadURL(string fileUrl)
    {
        if (database != null)
        {
            return await database.GetDownloadURL(fileUrl);
        }
        else { return null; }
    }

    public async Task<byte[]> getFile(string downloadUrl)
    {
        if (database != null)
        {
            return await database.getFile(downloadUrl);
        }
        else { return null; }
    }

    public async Task<AudioClip> getAudioClip(string downloadUrl)
    {
        if (database != null)
        {
            return await database.getAudioClip(downloadUrl);
        }
        else
        {
            return null;
        }
    }

    public async Task<bool> addIcon(string filePath, string iconFileName, string fileType, string iconExtension)
    {
        if (database != null)
        {
            return await database.addIcon(filePath, iconFileName, fileType, iconExtension);
        }
        return false;
    }

    public bool addFile(string filePath, string fileName, string fileType, string extension)
    {
        if (database != null)
        {
            return database.addFile(filePath, fileName, fileType, extension);
        }
        return false;
    }

    public void addFileData(string fileName, string fileType, string extension, string iconExtension)
    {
        if (database != null)
        {
            database.addFileData(fileName, fileType, extension, iconExtension);
        }
    }

    public async Task<bool> addSceneData(string sceneName, Trigger[] triggers, string pathTo360Media, string pathToAudio, SceneryObject[] scenery)
    {
        if (database != null)
        {
            return await database.addSceneData(sceneName, triggers, pathTo360Media, pathToAudio, scenery);
        }
        return false;
    }


    public IEnumerator getAllModelFileData(Action<List<FileMetaData>> callback)
    {
        if (database != null)
        {
            yield return database.getAllModelFileData(callback);
        }
    }

    public IEnumerator getAllSceneryFileData(Action<List<FileMetaData>> callback)
    {
        if (database != null)
        {
            yield return database.getAllSceneryFileData(callback);
        }
    }

    public IEnumerator getAllScenesFileData(Action<List<SceneMetaData>> callback)
    {
        if (database != null)
        {
            yield return database.getAllScenesFileData(callback);
        }
    }

    public IEnumerator getAll360Media(Action<List<FileMetaData>> callback)
    {
        if (database != null)
        {
            yield return database.getAll360Media(callback);
        }
    }

    public IEnumerator getAllSoundMedia(Action<List<FileMetaData>> callback)
    {
        if (database != null)
        {
            yield return database.getAllSoundMedia(callback);
        }
    }

    public void deleteFile(string fileName, string fileType, FileMetaData fileData)
    {
        if (database != null)
        {
            database.deleteFile(fileName, fileType, fileData);
        }
    }
}
