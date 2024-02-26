using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
using UnityEngine;

[Serializable]
public class SceneMetaData
{
    private string ID;
    private string sceneName;
    private string pathToTrigger;
    private string triggerLocation;
    private string triggerSize;
    private string pathTo360Media;
    private string pathToAudio;
    private string[] pathsToScenery;
    private string[] sceneryLocations;
    private string[] scenerySizes;

    public SceneMetaData() { 
    }

    public SceneMetaData(string ID, string sceneName, string pathToTrigger, string triggerLocation, string triggerSize,  string pathTo360Media, string pathToAudio, string[] pathsToScenery, string[] sceneryLocations, string[] scenerySizes)
    {
        this.ID = ID;
        this.sceneName = sceneName;
        this.pathToTrigger = pathToTrigger;
        this.pathTo360Media = pathTo360Media;
        this.pathToAudio = pathToAudio;
        this.pathsToScenery = pathsToScenery;
        this.triggerLocation = triggerLocation;
        this.sceneryLocations = sceneryLocations;
        this.triggerSize = triggerSize;
        this.scenerySizes = scenerySizes;
    }

    public string GetID()
    {
        return this.ID;
    }

    public string GetSceneName()
    {
        return this.sceneName;
    }

    public string GetTriggerPath()
    {
        return this.pathToTrigger;
    }

    public string GetTriggerLocation()
    {
        return this.triggerLocation;
    }

    public string GetTriggerSize()
    {
        return this.triggerSize;
    }

    public string Get360MediaPath() 
    { 
        return this.pathTo360Media; 
    }

    public string GetAudioPath()
    {
        return this.pathToAudio;
    }

    public string[] GetSceneryPaths()
    {
        return this.pathsToScenery;
    }

    public string[] GetSceneryLocations()
    {
        return this.sceneryLocations;
    }

    public string[] GetScenerySizes()
    {
        return this.scenerySizes;
    }
}
