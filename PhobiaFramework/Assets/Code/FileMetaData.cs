using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
using UnityEngine;

[Serializable]
public class FileMetaData
{
    public string filename;
    public string filetype;
    public string path;
    public string pathToIcon;

    public FileMetaData() {
    }

    public FileMetaData(string filename, string filetype, string path, string pathToIcon)
    {
        this.filename = filename;
        this.filetype = filetype;
        this.path = path;
        this.pathToIcon = pathToIcon;
    }
}
