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

    public void addFile(string filePath, string fileName, string fileType)
    {
        if (database != null)
        {
            database.addFile(filePath, fileName, fileType);
        }
    }

    public void addFileData(string fileName, string filetype)
    {
        if (database != null)
        {
            database.addFileData(fileName, filetype);
        }
    }
}
