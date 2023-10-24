using UnityEngine;
using System;
using SimpleFileBrowser;
using System.Collections;
using UnityEngine.UI;
using System.IO;
using static SimpleFileBrowser.FileBrowser;
using UnityEngine.Video;
using UnityEngine.Networking;
using System.Threading.Tasks;


public class MediaManager : MonoBehaviour
{
    public Material skyboxMaterial;
    public Button importImageButton;
    public Button importVideoButton;
    public GameObject photoSphere;
    public GameObject videoSphere;
    public VideoPlayer videoPlayer;
    public RenderTexture renderTexture;

    DatabaseService dbService;

    void Start()
    {
        importImageButton.onClick.AddListener(ImportImage);
        importVideoButton.onClick.AddListener(ImportVideo);

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
    }

    private async void ImportImage()
    {
        string downloadUrl = await dbService.GetDownloadURL("gs://vr-framework-95ccc.appspot.com/photos/ESO_Paranal_360_Marcio_Cabral_Chile_07-CC.jpg");
        if (!string.IsNullOrEmpty(downloadUrl))
        {
            await HandleImageSelected(downloadUrl);
        }
        else
        {
            Debug.Log("Failed to retrieve download URL!");
        }
    }

    private async void ImportVideo()
    {
        string downloadUrl = await dbService.GetDownloadURL("gs://vr-framework-95ccc.appspot.com/videos/360_vr_london_on_tower_bridge.mp4");
        if (!string.IsNullOrEmpty(downloadUrl))
        {
            await HandleVideoSelected(downloadUrl);
        }
        else
        {
            Debug.Log("Failed to retrieve download URL!");
        }
    }

    async Task<IEnumerator> HandleImageSelected(string downloadUrl)
    {
        byte[] binaryData = await dbService.getFile(downloadUrl);

        if (binaryData != null)
        {
            Texture2D equirectangularImage = new Texture2D(2, 2);
            equirectangularImage.LoadImage(binaryData);

            // Set the loaded texture as the Skybox texture
            skyboxMaterial.SetTexture("_MainTex", equirectangularImage);

            // Deactivate the VideoPlayer if it's active
            videoPlayer.Stop();
            videoPlayer.targetTexture = null;
        }

        return null;
    }

    async Task<IEnumerator> HandleVideoSelected(string downloadUrl)
    {
        byte[] binaryData = await dbService.getFile(downloadUrl);

        if (binaryData != null)
        {
            // Set the video source and clip for the VideoPlayer
            videoPlayer.source = VideoSource.VideoClip;

            // Create a temporary file with the byte array data
            string tempPath = Application.persistentDataPath + "/tempVideo.mp4";
            File.WriteAllBytes(tempPath, binaryData);

            videoPlayer.url = tempPath;

            videoPlayer.renderMode = VideoRenderMode.RenderTexture;
            videoPlayer.targetTexture = renderTexture;

            // Set the Render Texture as the Skybox material
            skyboxMaterial.SetTexture("_MainTex", renderTexture);

            // Play the video
            videoPlayer.Play();
        }
        return null;
    }
}






