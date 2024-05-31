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

// The script represents an object in the scenery of a scene. 

public class SceneryObject {

    public string name;
    public string path;
    public string position;
    public string rotation;
    public string size;

    public SceneryObject(string name, string path, string postion, string rotation, string size)
    {
        this.name = name;
        this.path = path;
        this.position = postion;
        this.rotation = rotation;
        this.size = size;
    }

    public string GetPath() { return this.path; }
    public string GetName() { return this.name; }
    public string GetPosition() { return this.position; }
    public string GetRotation() { return this.rotation; }
    public string GetSize() { return this.size; }

}
