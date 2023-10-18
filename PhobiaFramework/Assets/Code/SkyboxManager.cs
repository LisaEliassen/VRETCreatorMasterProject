using UnityEngine;
using UnityEngine.UI;
using System.IO;
using UnityEngine.Video;

public class SkyboxManager : MonoBehaviour
{
    public Material skyboxMaterial;
    public Button importImageButton;
    public Button importVideoButton;
    public GameObject skyboxObject; // Reference to your Skybox GameObject
    public VideoPlayer videoPlayer;
    public RenderTexture renderTexture;

    private void Start()
    {
        importImageButton.onClick.AddListener(ImportImage);
        importVideoButton.onClick.AddListener(ImportVideo);
    }

    private void ImportImage()
    {
        string path = UnityEditor.EditorUtility.OpenFilePanel("Select 360 Image", "", "jpg, png");
        if (path.Length != 0)
        {
            byte[] fileData = File.ReadAllBytes(path);
            Texture2D equirectangularImage = new Texture2D(2, 2);
            equirectangularImage.LoadImage(fileData);

            // Set the loaded texture as the Skybox texture
            skyboxMaterial.SetTexture("_MainTex", equirectangularImage);

            // Activate the Skybox GameObject
            skyboxObject.SetActive(true);

            // Deactivate the VideoPlayer if it's active
            videoPlayer.Stop();
            videoPlayer.targetTexture = null;
        }
    }

    private void ImportVideo()
    {
        string path = UnityEditor.EditorUtility.OpenFilePanel("Select Video", "", "mp4, mov");
        if (path.Length != 0)
        {
            // Set the video source and render texture
            videoPlayer.source = VideoSource.Url;
            videoPlayer.url = path;
            videoPlayer.targetTexture = renderTexture;

            // Set the Render Texture as the Skybox material
            skyboxMaterial.SetTexture("_MainTex", renderTexture);

            // Activate the Skybox GameObject
            skyboxObject.SetActive(true);

            // Play the video
            videoPlayer.Play();
        }
    }
}




