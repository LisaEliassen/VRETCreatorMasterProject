using System.Collections;
using System.Collections.Generic;
using System;
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

using System.IO;
using UnityEngine;

[Serializable]

//The script represents an object with attributes for path, position, rotation, size, and isCopy, offering methods to get and set each attribute.
public class Trigger {

    public string path;
    public string position;
    public string rotation;
    public string size;
    public string isCopy;

    public Trigger() {}

    public Trigger(string path, string positon, string rotation, string size, string isCopy)
    {
        this.path = path;
        this.position = positon;
        this.rotation = rotation;
        this.size = size;
        this.isCopy = isCopy;
    }

    public string GetPath() { return this.path; }
    public string GetPosition() { return this.position; }
    public string GetRotation() { return this.rotation; }
    public string GetSize() { return this.size; }

    public string IsCopy() { return this.isCopy; }

    public void SetPath(string path) { this.path = path; }
    public void SetPosition(string positon) { this.position = positon;}
    public void SetRotation(string rotation) {  this.rotation = rotation;}
    public void SetSize(string size) {  this.size = size; }
    public void SetIsCopy(string isCopy) { this.isCopy = isCopy; }
}
