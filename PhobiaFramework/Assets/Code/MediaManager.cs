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
    public Button chooseFromDeviceButton;
    public GameObject photoSphere;
    public GameObject videoSphere;
    public VideoPlayer videoPlayer;
    public RenderTexture renderTexture;

    public GameObject EditSceneUI;
    public GameObject MediaUI;

    public GameObject databaseServiceObject;
    DatabaseService dbService;

    void Start()
    {
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

        chooseFromDeviceButton.onClick.AddListener(ImportMedia);

    }

    private void ImportMedia()
    {
        StartCoroutine(ShowLoadDialogCoroutine(PickMode.Files, HandleMediaSelected));
    }

    private void HandleMediaSelected(string[] paths)
    {
        //FileBrowser.HideDialog(true);
        if (paths.Length > 0)
        {
            string path = paths[0];

            if (IsVideoFile(path))
            {
                // Handle video file
                videoPlayer.source = VideoSource.Url;
                videoPlayer.url = path;

                videoPlayer.targetTexture = renderTexture;

                // Set the Render Texture as the Skybox material
                skyboxMaterial.SetTexture("_MainTex", renderTexture);

                // Play the video
                videoPlayer.Play();

                EditSceneUI.SetActive(true);
                MediaUI.SetActive(false);
            }
            else if (IsImageFile(path))
            {
                // Handle image file
                byte[] fileData = File.ReadAllBytes(path);
                Texture2D equirectangularImage = new Texture2D(2, 2);
                equirectangularImage.LoadImage(fileData);

                // Set the loaded texture as the Skybox texture
                skyboxMaterial.SetTexture("_MainTex", equirectangularImage);

                // Deactivate the VideoPlayer if it's active
                videoPlayer.Stop();
                videoPlayer.targetTexture = null;

                EditSceneUI.SetActive(true);
                MediaUI.SetActive(false);
            }
            else
            {
                // Handle unsupported file type
                Debug.Log("Selected file is not supported.");
            }
        }
    }

    private bool IsVideoFile(string path)
    {
        string extension = Path.GetExtension(path);
        return extension.Equals(".mp4", StringComparison.OrdinalIgnoreCase) || extension.Equals(".mov", StringComparison.OrdinalIgnoreCase);
    }

    private bool IsImageFile(string path)
    {
        string extension = Path.GetExtension(path);
        return extension.Equals(".jpg", StringComparison.OrdinalIgnoreCase) || extension.Equals(".png", StringComparison.OrdinalIgnoreCase);
    }

    IEnumerator ShowLoadDialogCoroutine(PickMode pickMode, Action<string[]> callback)
    {
        FileBrowser.SetFilters(true, new FileBrowser.Filter("360 media", ".jpg", ".png", ".jpeg", ".mp4", ".mov"));
        FileBrowser.SetDefaultFilter(".jpg");

        yield return FileBrowser.WaitForLoadDialog(pickMode, true, null, null, "Load Files", "Load");

        if (FileBrowser.Success)
        {
            callback(FileBrowser.Result);
        }
    }

    public async Task<IEnumerator> HandleImageSelected(string downloadUrl)
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

    public async Task<IEnumerator> HandleVideoSelected(string downloadUrl)
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






