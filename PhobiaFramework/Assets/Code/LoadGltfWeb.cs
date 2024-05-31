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
using GLTFast;
using GLTFast.Schema;
using Firebase.Extensions;
using UnityEngine.Networking;
using System;
using System.Threading.Tasks;
using static System.Net.WebRequestMethods;

// This script is designed to load GLTF (GL Transmission Format) models from a remote server (in this case, Firebase Storage) and instantiate them in a Unity scene.
// It allows for the dynamic loading and instantiation of GLTF models from a remote server, enabling the creation of interactive and dynamic scenes in Unity.

public class LoadGltfWeb : MonoBehaviour
{

    FirebaseStorage storage;
    StorageReference gltfReference;
    GameObject loadedModel;
    DatabaseService dbService;
    public Vector3 position;
    public string triggerName;

    // Start is called before the first frame update
    void Start()
    {

    }

    public void spawnObject()
    {
        loadedModel = new GameObject(triggerName);
        //var gltf = loadedModel.AddComponent<GLTFast.GltfAsset>();
        //gltf.Url = "https://firebasestorage.googleapis.com/v0/b/vr-framework-95ccc.appspot.com/o/models%2FblueJay.glb?alt=media&token=fa864308-df4e-4da7-96ee-db6d0846b36d";

        loadGlb(loadedModel);
    }

    public void loadGlb(GameObject loadedModel)
    {
        var gltFastImport = new GLTFast.GltfImport();

        storage = FirebaseStorage.DefaultInstance;

        gltfReference =
            storage.GetReferenceFromUrl("gs://vr-framework-95ccc.appspot.com/models/blueJay.glb");

        // Fetch the download URL
        gltfReference.GetDownloadUrlAsync().ContinueWithOnMainThread(task =>
        {
            if (!task.IsFaulted && !task.IsCanceled)
            {
                string downloadUrl = task.Result.ToString();
                Debug.Log("Download URL: " + downloadUrl);

                // Download the file via UnityWebRequest
                StartCoroutine(DownloadGltfFile(downloadUrl, loadedModel));
            }
        });

    }

    IEnumerator DownloadGltfFile(string downloadUrl, GameObject loadedModel)
    {
        using (UnityWebRequest www = UnityWebRequest.Get(downloadUrl))
        {
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.Success)
            {
                byte[] gltfData = www.downloadHandler.data;

                // Create a new GLTFast loader
                var gltfImport = new GLTFast.GltfImport();

                var settings = new ImportSettings
                {
                    GenerateMipMaps = true,
                    AnisotropicFilterLevel = 3,
                    NodeNameMethod = NameImportMethod.OriginalUnique
                };

                // Load the GLTF model from the byte array
                Task<bool> loadTask = LoadGltfBinaryFromMemory(gltfData, loadedModel, downloadUrl, gltfImport);
                yield return new WaitUntil(() => loadTask.IsCompleted);

                bool success = loadTask.Result;

                if (success)
                {
                    loadedModel.transform.position = position;
                    loadedModel.SetActive(true);
                }
                else
                {
                    Debug.LogError("Loading glTF failed!");
                }
            }
            else
            {
                Debug.LogError("Error downloading GLTF file: " + www.error);
            }
        }
    }

    async Task<bool> LoadGltfBinaryFromMemory(byte[] data, GameObject gameObject, string downloadUrl, GltfImport gltfImport)
    {
        var filePath = "file://firebasestorage.googleapis.com/v0/b/vr-framework-95ccc.appspot.com/o/models%2FblueJay.glb?alt=media&token=fa864308-df4e-4da7-96ee-db6d0846b36d"; // Not sure what to put here
        //var gltf = new GltfImport();
        Debug.Log(BitConverter.ToString(data));
        bool success = await gltfImport.LoadGltfBinary(
            data,
            // The URI of the original data is important for resolving relative URIs within the glTF
            new Uri(filePath)
            );
        if (success)
        {
            await gltfImport.InstantiateMainSceneAsync(gameObject.transform);
            gameObject.SetActive(false);
        }
        return success;
    }
}
