using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FileInfo
{
    public string filename;
    public string path;
    public string filetype;

    public FileInfo() {
    }

    public FileInfo(string filename, string path, string filetype)
    {
        this.filename = filename;
        this.path = path;
        this.filetype = filetype;
    }
}
