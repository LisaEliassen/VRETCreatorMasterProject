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

using Firebase.Storage;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityGLTF;
using System.Threading.Tasks;
using GLTFast;
using GLTFast.Schema;
using Firebase.Extensions;
using UnityEngine.Networking;
using System;

// This script is similar to LoadGltfWeb, but with a few differences in how it loads and handles the GLTF model. 
// The script enables the dynamic loading and instantiation of GLTF models from a remote server (Firebase Storage) and provides a straightforward method for integrating GLTF assets into Unity scenes.

public class LoadGltfFromDatabase : MonoBehaviour
{
    FirebaseStorage storage;
    StorageReference gltfReference;
    GameObject loadedModel;
    public Vector3 position;
    public string triggerName;

    public void spawnObject()
    {
        loadedModel = new GameObject(triggerName);
        var gltf = loadedModel.AddComponent<GLTFast.GltfAsset>();

        loadGltf();
        loadedModel.transform.position = position;
        loadedModel.SetActive(true);
    }

    public void loadGltf()
    {
        var gltFastImport = new GLTFast.GltfImport();

        storage = FirebaseStorage.DefaultInstance;

        gltfReference =
            storage.GetReferenceFromUrl("gs://vr-framework-95ccc.appspot.com/models/blueJay.gltf");

        gltfReference.GetDownloadUrlAsync().ContinueWithOnMainThread(DownloadGltf);

    }

    async void DownloadGltf(Task<Uri> task)
    {
        if (!task.IsFaulted && !task.IsCanceled)
        {
            var gltf = new GLTFast.GltfImport();

            var settings = new ImportSettings
            {
                GenerateMipMaps = true,
                AnisotropicFilterLevel = 3,
                NodeNameMethod = NameImportMethod.OriginalUnique
            };

            string downloadUrl = task.Result.ToString();
            Debug.Log(downloadUrl);
            var success = await gltf.Load("https://firebasestorage.googleapis.com/v0/b/vr-framework-95ccc.appspot.com/o/models%2FblueJay.gltf", settings);

            if (success)
            {
                await gltf.InstantiateMainSceneAsync(loadedModel.transform);
                loadedModel.SetActive(false);
            }
            else
            {
                Debug.LogError("Loading glTF failed!");
            }
        }
        else
        {
            Debug.LogError("Failed to fetch download URL: " + task.Exception);
        }
    }
}
