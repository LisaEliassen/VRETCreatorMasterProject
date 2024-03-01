using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
using UnityEngine;

[Serializable]
public class SceneryObject {

    //public string name;
    public string path;
    public string transform;
    public string size;

    public SceneryObject(string path, string postion, string size)
    {
        //this.name = name;
        this.path = path;
        this.transform = postion;
        this.size = size;
    }

    public string GetPath() { return this.path; }
    //public string GetName() { return this.name; }
    public string GetPosition() { return this.transform; }
    public string GetSize() { return this.size; }

}
