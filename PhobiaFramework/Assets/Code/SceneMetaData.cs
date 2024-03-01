using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
using UnityEngine;

[Serializable]
public class SceneMetaData
{
    public string ID;
    public string sceneName;
    public Trigger trigger;
    /*private string pathToTrigger;
    private string triggerLocation;
    private string triggerSize;*/
    public string pathTo360Media;
    public string pathToAudio;
    public SceneryObject[] scenery;
    /*private string[] pathsToScenery;
    private string[] sceneryLocations;
    private string[] scenerySizes;*/

    public SceneMetaData() { 
    }

    public SceneMetaData(string ID, string sceneName, Trigger trigger, string pathTo360Media, string pathToAudio, SceneryObject[] scenery)
    {
        this.ID = ID;
        this.sceneName = sceneName;
        this.trigger = trigger;
        /*this.pathToTrigger = pathToTrigger;
        this.triggerSize = triggerSize;
        this.triggerLocation = triggerLocation;*/
        this.pathTo360Media = pathTo360Media;
        this.pathToAudio = pathToAudio;
        /*this.pathsToScenery = pathsToScenery;
        this.sceneryLocations = sceneryLocations;
        this.scenerySizes = scenerySizes;*/
        this.scenery = scenery;
    }

    public string GetID()
    {
        return this.ID;
    }

    public string GetSceneName()
    {
        return this.sceneName;
    }

    /*public string GetTriggerPath()
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
    }*/

    public string Get360MediaPath() 
    { 
        return this.pathTo360Media; 
    }

    public string GetAudioPath()
    {
        return this.pathToAudio;
    }

    public SceneryObject[] GetScenery()
    {
        return this.scenery;
    }

    /*public string[] GetSceneryPaths()
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
    }*/
}