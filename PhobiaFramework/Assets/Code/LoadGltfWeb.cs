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

public class LoadGltfWeb : MonoBehaviour
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
        gltf.Url = "https://firebasestorage.googleapis.com/v0/b/vr-framework-95ccc.appspot.com/o/models%2FblueJay.gltf?alt=media&token=86d0ca42-dabd-4bfa-ba76-847e80573dfa";

        loadGltf(loadedModel);
    }

    public void loadGltf(GameObject loadedModel)
    {
        var gltFastImport = new GLTFast.GltfImport();

        storage = FirebaseStorage.DefaultInstance;

        gltfReference =
            storage.GetReferenceFromUrl("gs://vr-framework-95ccc.appspot.com/models/blueJay.gltf");

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
                var gltfLoader = new GLTFast.GltfImport();

                var settings = new ImportSettings
                {
                    GenerateMipMaps = true,
                    AnisotropicFilterLevel = 3,
                    NodeNameMethod = NameImportMethod.OriginalUnique
                };

                // Load the GLTF model from the byte array
                Task<bool> loadTask = LoadGltfBinaryFromMemory(gltfData, loadedModel, downloadUrl);
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

    async Task<bool> LoadGltfBinaryFromMemory(byte[] data, GameObject gameObject, string downloadUrl)
    {
        var filePath = "https://firebasestorage.googleapis.com/v0/b/vr-framework-95ccc.appspot.com/o/models%2FblueJay.gltf?alt=media&token=86d0ca42-dabd-4bfa-ba76-847e80573dfa"; // Not sure what to put here
        var gltf = new GltfImport();
        bool success = await gltf.LoadGltfBinary(
            data,
            // The URI of the original data is important for resolving relative URIs within the glTF
            new Uri(filePath)
            );
        if (success)
        {
            await gltf.InstantiateMainSceneAsync(gameObject.transform);
            gameObject.SetActive(false);
        }
        return success;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
