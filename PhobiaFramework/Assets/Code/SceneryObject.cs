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
