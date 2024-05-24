using System.Collections;
using System.Collections.Generic;
using System;
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
