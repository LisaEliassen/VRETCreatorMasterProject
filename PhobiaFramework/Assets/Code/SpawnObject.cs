using UnityEngine;
using UnityGLTF;
using System.Threading.Tasks;
using GLTFast;

public class SpawnObject : MonoBehaviour
{
    // file:///Assets/Assets/blueJay.gltf
    public UnityEngine.UI.Slider sizeSlider;
    public GameObject gameObject;


    void Start()
    {
        // Add an event listener to the slider's value changed event
        sizeSlider.onValueChanged.AddListener(ChangeObjectSize);
    }

    public void spawnObject(Vector3 position, string filepath, string name)
    {
        gameObject = new GameObject(name);
        gameObject.transform.position = position;

        loadGltf(gameObject, filepath, name);
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
        }
        else
        {
            Debug.LogError("Loading glTF failed!");
        }
    }

    // Callback method to adjust object size based on the slider's value
    private void ChangeObjectSize(float scaleValue)
    {
        // Assuming you want to change the scale of the loaded object
        // You can adjust this to your specific use case
        GameObject loadedObject = GameObject.Find("BlueJay"); // Replace with the actual object name
        if (loadedObject != null)
        {
            /// Map the slider value (0-100) to the desired scale range (minScale-maxScale)
            float scaledValue = scaleValue;
            Vector3 newScale = new Vector3(scaledValue, scaledValue, scaledValue);
            loadedObject.transform.localScale = newScale;
        }
    }
}
