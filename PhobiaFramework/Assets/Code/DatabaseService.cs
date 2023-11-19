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

    public void addIcon(string filePath, string iconFileName, string fileType, string iconExtension)
    {
        if (database != null)
        {
            database.addIcon(filePath, iconFileName, fileType, iconExtension);
        }
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

    public IEnumerator getAllModelFileData(System.Action<List<FileMetaData>> callback)
    {
        if (database != null)
        {
            yield return database.getAllModelFileData(callback);
        }
    }

    public IEnumerator getAll360Media(Action<List<FileMetaData>> callback)
    {
        if (database != null)
        {
            yield return database.getAll360Media(callback);
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
