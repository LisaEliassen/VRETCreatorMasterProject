using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GLTFast;
using GLTFast.Schema;
using UnityEngine.Networking;
using System;
using System.Threading.Tasks;
using static System.Net.WebRequestMethods;

public class LoadGlb : MonoBehaviour
{
    GameObject loadedModel;
    DatabaseService dbService;
    Vector3 position;
    string triggerName;

    // Start is called before the first frame update
    void Start()
    {
        // Find the GameObject with the DatabaseService script
        GameObject databaseServiceObject = GameObject.Find("DatabaseService");
        //dbService = new DatabaseService("Firebase");

        // Check if the GameObject was found
        if (databaseServiceObject != null)
        {
            // Get the DatabaseService component from the found GameObject
            dbService = databaseServiceObject.GetComponent<DatabaseService>();
        }
        else
        {
            Debug.LogError("GameObject with DatabaseService not found.");
        }

        position = GameObject.Find("Position2").transform.position;
        triggerName = "Trigger";
    }

    public void SpawnObject()
    {
        loadedModel = new GameObject(triggerName);
        LoadGlbFile(loadedModel);

        dbService.getAllModelFileData();
    }

    public async void LoadGlbFile(GameObject loadedModel)
    {
        var gltFastImport = new GLTFast.GltfImport();
        string downloadUrl = await dbService.GetDownloadURL("gs://vr-framework-95ccc.appspot.com/models/blueJay.glb");
        //dbService.addFileData("0", "blueJay", downloadUrl, "glb");

        if (!string.IsNullOrEmpty(downloadUrl))
        {
            StartCoroutine(DownloadGltfFile(downloadUrl, loadedModel));
        }
        else
        {
            Debug.Log("Failed to retrieve download URL!");
        }

    }

    IEnumerator DownloadGltfFile(string downloadUrl, GameObject loadedModel)
    {
        using (UnityWebRequest www = UnityWebRequest.Get(downloadUrl))
        {
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.Success)
            {
                byte[] gltfBinaryData = www.downloadHandler.data;

                // Create a new GLTFast loader
                var gltfImport = new GLTFast.GltfImport();

                var settings = new ImportSettings
                {
                    GenerateMipMaps = true,
                    AnisotropicFilterLevel = 3,
                    NodeNameMethod = NameImportMethod.OriginalUnique
                };


                // Load the GLTF model from the byte array
                Task<bool> loadTask = LoadGltfBinaryFromMemory(gltfBinaryData, loadedModel, downloadUrl, gltfImport);
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
        var filePath = GetFilepath(downloadUrl);
        
        //Debug.Log(BitConverter.ToString(data));
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

    public string GetFilepath(string downloadURL)
    {
        return downloadURL.Replace("https", "file");
    }

    // Update is called once per frame
    void Update()
    {

    }
}
