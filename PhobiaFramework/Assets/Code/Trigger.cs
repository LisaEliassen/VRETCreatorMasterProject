using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
using UnityEngine;

[Serializable]
public class Trigger {

    public string path;
    //private string name;
    public string position;
    public string rotation;
    public string size;

    public Trigger() {}

    public Trigger(string path, string positon, string rotation, string size)
    {
        //this.name = name;
        this.path = path;
        this.position = positon;
        this.rotation = rotation;
        this.size = size;
    }

    public string GetPath() { return this.path; }
    //public string GetName() { return this.name; }
    public string GetPosition() { return this.position; }
    public string GetRotation() { return this.rotation; }
    public string GetSize() { return this.size; }

    public void SetPath(string path) { this.path = path; }
    public void SetPosition(string positon) { this.position = positon;}
    public void SetRotation(string rotation) {  this.rotation = rotation;}
    public void SetSize(string size) {  this.size = size; }
}
