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
