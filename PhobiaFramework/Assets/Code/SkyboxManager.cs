using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class SkyboxManager : MonoBehaviour
{
    public Material skyboxMaterial;
    public Button importButton;

    private void Start()
    {
        importButton.onClick.AddListener(ImportSkybox);
    }

    private void ImportSkybox()
    {
        string path = UnityEditor.EditorUtility.OpenFilePanel("Select 360 Image/Video", "", "jpg, png, mp4, mov");
        if (path.Length != 0)
        {
            // Load the image or video from the selected file
            byte[] fileData = File.ReadAllBytes(path);
            Texture2D equirectangularImage = new Texture2D(2, 2);
            equirectangularImage.LoadImage(fileData);

            // Set the loaded texture as the Skybox texture
            skyboxMaterial.SetTexture("_MainTex", equirectangularImage);
        }
    }
}

