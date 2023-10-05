using Firebase.Storage;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityGLTF;
using System.Threading.Tasks;
using GLTFast;
using GLTFast.Schema;
using Firebase.Extensions;
using UnityEngine.Networking;
using System;

public class LoadGltfFromDatabase : MonoBehaviour
{
    FirebaseStorage storage;
    StorageReference gltfReference;
    GameObject loadedModel;
    public Vector3 position;
    public string name;

    // Start is called before the first frame update
    void Start()
    {
        
    }
    public void spawnObject()
    {
        loadedModel = new GameObject(name);
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

        /*
        // Fetch the download URL
        gltfReference.GetDownloadUrlAsync().ContinueWithOnMainThread(task => {
            if (!task.IsFaulted && !task.IsCanceled)
            {
                Debug.Log("Download URL: " + task.Result);
                // ... now download the file via WWW or UnityWebRequest.
            }
        });*/

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

    // Update is called once per frame
    void Update()
    {
        
    }
}
