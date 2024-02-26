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

public interface Database
{
    void initialize();

    Task<string> GetDownloadURL(string fileUrl);

    Task<byte[]> getFile(string downloadUrl);

    Task<AudioClip> getAudioClip(string downloadUrl);

    void addIcon(string filePath, string iconFileName, string fileType, string iconExtension);

    bool addFile(string filePath, string fileName, string fileType, string extension);

    void addFileData(string fileName, string fileType, string extension, string iconExtension);

    void addSceneData(string sceneName, string pathToTrigger, string triggerTransform, string triggerSize, string pathTo360Media, string pathToAudio, string[] pathsToScenery, string[] sceneryLocations, string[] scenerySizes);

    IEnumerator getAllModelFileData(System.Action<List<FileMetaData>> callback);

    IEnumerator getAllSceneryFileData(System.Action<List<FileMetaData>> callback);

    IEnumerator getAll360Media(Action<List<FileMetaData>> callback);

    IEnumerator getAllSoundMedia(Action<List<FileMetaData>> callback);

    void deleteFile(string fileName, string fileType, FileMetaData fileData);
}