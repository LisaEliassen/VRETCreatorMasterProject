using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
using UnityEngine;

[Serializable]

// The script is a simple data structure used to store metadata about files in the Firebase database. 

public class FileMetaData
{
    private string ID;
    public string filename;
    public string filetype;
    public string path;
    public string pathToIcon;

    public FileMetaData() {
    }

    public FileMetaData(string ID, string filename, string filetype, string path, string pathToIcon)
    {
        this.ID = ID;
        this.filename = filename;
        this.filetype = filetype;
        this.path = path;
        this.pathToIcon = pathToIcon;
    }

    public string GetID()
    {
        return this.ID;
    }

    public string GetFilename()
    {
        return this.filename;
    }

    public string GetFileType()
    {
        return this.filetype;
    }

    public string GetPath() 
    { 
        return this.path;
    }

    public string GetPathToIcon() 
    {
        return this.pathToIcon;
    }
}
