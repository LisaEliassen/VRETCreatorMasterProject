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

    void addFile(string filePath, string fileName, string fileType);

    void addFileData(string fileId, string fileName, string path, string filetype);


}
