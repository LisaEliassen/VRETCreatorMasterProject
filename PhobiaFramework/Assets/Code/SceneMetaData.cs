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
    public string pathToSceneIcon;
    public Trigger trigger;
    public string pathTo360Media;
    public string pathToAudio;
    public SceneryObject[] scenery;

    public SceneMetaData() { 
    }

    public SceneMetaData(string ID, string sceneName, string pathToSceneIcon, Trigger trigger, string pathTo360Media, string pathToAudio, SceneryObject[] scenery)
    {
        this.ID = ID;
        this.sceneName = sceneName;
        this.pathToSceneIcon = pathToSceneIcon;
        this.trigger = trigger;
        this.pathTo360Media = pathTo360Media;
        this.pathToAudio = pathToAudio;
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
}