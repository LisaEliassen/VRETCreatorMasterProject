using Firebase;
using Firebase.Extensions;
using Firebase.Database;
using Firebase.Storage;
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GLTFast;
using GLTFast.Schema;
using UnityEngine.Networking;
using System;
using System.Threading.Tasks;
using static System.Net.WebRequestMethods;

// The Database interface defines a contract for interacting with different types of databases within the application. 

public interface Database
{
    void initialize();

    Task<string> GetDownloadURL(string fileUrl);

    Task<byte[]> getFile(string downloadUrl);

    Task<AudioClip> getAudioClip(string downloadUrl);

    Task<bool> addIcon(string filePath, string iconFileName, string fileType, string iconExtension);

    bool addFile(string filePath, string fileName, string fileType, string extension);

    void addFileData(string fileName, string fileType, string extension, string iconExtension);

    Task<bool> addSceneData(string sceneName, Trigger[] triggers, string pathTo360Media, string pathToAudio, SceneryObject[] scenery);

    IEnumerator getAllModelFileData(System.Action<List<FileMetaData>> callback);

    IEnumerator getAllSceneryFileData(System.Action<List<FileMetaData>> callback);

    IEnumerator getAllScenesFileData(Action<List<SceneMetaData>> callback);

    IEnumerator getAll360Media(Action<List<FileMetaData>> callback);

    IEnumerator getAllSoundMedia(Action<List<FileMetaData>> callback);

    void deleteFile(string fileName, string fileType, FileMetaData fileData);
}