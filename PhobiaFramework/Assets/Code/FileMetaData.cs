using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
using UnityEngine;

[Serializable]
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
}
