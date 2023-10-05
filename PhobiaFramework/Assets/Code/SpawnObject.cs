using UnityEngine;
using UnityGLTF;
using System.Threading.Tasks;
using GLTFast;

public class SpawnObject : MonoBehaviour
{
    // file:///Assets/Assets/blueJay.gltf

    void Start()
    {

    }

    public void spawnObject(Vector3 position, string filepath, string name)
    {
        var gameObject = new GameObject(name);

        loadGltf(gameObject, filepath, name);
        gameObject.transform.position = position;
        gameObject.SetActive(true);
    }

    async public void loadGltf(GameObject gameObject, string filepath, string name)
    {
        var gltf = new GLTFast.GltfImport();

        // Create a settings object and configure it accordingly
        var settings = new ImportSettings
        {
            GenerateMipMaps = true,
            AnisotropicFilterLevel = 3,
            NodeNameMethod = NameImportMethod.OriginalUnique
        };
        // Load the glTF and pass along the settings
        var success = await gltf.Load(filepath, settings);

        if (success)
        {
            await gltf.InstantiateMainSceneAsync(gameObject.transform);
            gameObject.SetActive(false);
        }
        else
        {
            Debug.LogError("Loading glTF failed!");
        }
    }
}
