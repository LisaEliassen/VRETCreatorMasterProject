using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
using UnityEngine;

[Serializable]
public class Trigger {

    public string path;
    //private string name;
    public string transform;
    public string size;

    public Trigger() {}

    public Trigger(string path, string positon, string size)
    {
        //this.name = name;
        this.path = path;
        this.transform = positon;
        this.size = size;
    }

    public string GetPath() { return this.path; }
    //public string GetName() { return this.name; }
    public string GetPosition() { return this.transform; }
    public string GetSize() { return this.size; }

    public void SetPath(string path) { this.path = path; }
    public void SetPosition(string positon) { this.transform = positon;}
    public void SetSize(string size) {  this.size = size; }
}
