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

// The script encapsulates metadata about a scene. 

public class SceneMetaData
{
    public string ID;
    public string sceneName;
    public string pathToSceneIcon;
    public Trigger[] triggers;
    public string pathTo360Media;
    public string pathToAudio;
    public SceneryObject[] scenery;

    public SceneMetaData() { 
    }

    public SceneMetaData(string ID, string sceneName, string pathToSceneIcon, Trigger[] triggers, string pathTo360Media, string pathToAudio, SceneryObject[] scenery)
    {
        this.ID = ID;
        this.sceneName = sceneName;
        this.pathToSceneIcon = pathToSceneIcon;
        this.triggers = triggers;
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