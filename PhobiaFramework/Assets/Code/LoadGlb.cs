using System;
using System.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using GLTFast;
using GLTFast.Schema;
using UnityEngine.Networking;

public class LoadGlb : MonoBehaviour
{
    GameObject loadedModel;
    DatabaseService dbService;
    AnimationController animController;
    Vector3 position;
    string triggerName;

    // Start is called before the first frame update
    void Start()
    {
        // Find the GameObject with the DatabaseService script
        GameObject databaseServiceObject = GameObject.Find("DatabaseService");

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

        // Find the GameObject with the DatabaseService script
        GameObject animationControllerObject = GameObject.Find("AnimationController");

        // Check if the GameObject was found
        if (animationControllerObject != null)
        {
            // Get the DatabaseService component from the found GameObject
            animController = animationControllerObject.GetComponent<AnimationController>();
        }
        else
        {
            Debug.LogError("GameObject with AnimationController not found.");
        }

        position = GameObject.Find("Position2").transform.position;
        triggerName = "Trigger";
    }

    public void SpawnObject()
    {
        loadedModel = new GameObject(triggerName);
        LoadGlbFile(loadedModel);
    }

    //public async void LoadGlbFile(GameObject loadedModel, string downloadUrl, string modelName)
    public async void LoadGlbFile(GameObject loadedModel)
    {
        var gltFastImport = new GLTFast.GltfImport();
        string downloadUrl = await dbService.GetDownloadURL("gs://vr-framework-95ccc.appspot.com/models/blueJay.glb");
        string modelName = "blueJay";
        string avatarUrl = await dbService.GetDownloadURL("gs://vr-framework-95ccc.appspot.com/models/avatars/blueJayAvatar.asset");
        string animUrl = await dbService.GetDownloadURL("gs://vr-framework-95ccc.appspot.com/animations/watch01.anim");

        // Get byte data from database
        byte[] glbData = await dbService.getFile(downloadUrl);
        byte[] avatarData = await dbService.getFile(avatarUrl);
        byte[] animData = await dbService.getFile(animUrl);

        if (glbData != null)
        {
            // Create a new GLTFast loader
            var gltfImport = new GLTFast.GltfImport();

            var settings = new ImportSettings
            {
                GenerateMipMaps = true,
                AnisotropicFilterLevel = 3,
                NodeNameMethod = NameImportMethod.OriginalUnique
            };


            // Load the GLTF model from the byte array
            bool success = await LoadGltfBinaryFromMemory(glbData, loadedModel, downloadUrl, gltfImport);

            if (success)
            {
                loadedModel.transform.position = position;
                loadedModel.SetActive(true);


                // Find the child GameObject named "Trigger"
                Transform triggerTransform = loadedModel.transform.Find(modelName);

                // Check if the child GameObject was found
                if (triggerTransform != null)
                {
                    // Add the Animator component to the child GameObject
                    //Animator animator = triggerTransform.gameObject.AddComponent<Animator>();
                    //LoadAvatar(avatarData, triggerTransform.gameObject, modelName);*/

                    // animator.runtimeAnimatorController = yourAnimatorController;

                    animController.LoadAnimation(animData, triggerTransform.gameObject, "watch01");
                }
                else
                {
                    Debug.LogError("Child GameObject with name " + modelName + " not found.");
                }
            }
            else
            {
                Debug.LogError("Loading glTF failed!");
            }
        }
    }

    public void LoadAvatar(byte[] data, GameObject gameObjectWithAnimator, string modelName)
    {
        string tempFilePath = Path.GetTempFileName();

        //string tempFilePath = Application.persistentDataPath + "/tempAvatar/" + modelName + ".asset";
        File.WriteAllBytes(tempFilePath, data);

        // Load the Avatar from the temporary file
        Avatar loadedAvatar = AvatarBuilder.BuildGenericAvatar(gameObjectWithAnimator, tempFilePath);

        // Check if the Avatar was loaded successfully
        if (loadedAvatar != null)
        {
            // Get the Animator component from the provided GameObject
            Animator animator = gameObjectWithAnimator.GetComponent<Animator>();

            // Assign the loaded Avatar to the Animator component
            animator.avatar = loadedAvatar;

            // Delete the temporary file
            File.Delete(tempFilePath);
        }
        else
        {
            Debug.LogError("Failed to load Avatar.");
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
