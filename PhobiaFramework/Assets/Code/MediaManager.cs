using UnityEngine;
using System;
using SimpleFileBrowser;
using System.Collections;
using UnityEngine.UI;
using System.IO;
using static SimpleFileBrowser.FileBrowser;
using UnityEngine.Video;

public class MediaManager : MonoBehaviour
{
    public Material skyboxMaterial;
    public Button importImageButton;
    public Button importVideoButton;
    public GameObject photoSphere;
    public GameObject videoSphere;
    public VideoPlayer videoPlayer;
    public RenderTexture renderTexture;


    void Start()
    {
        FileBrowser.SetFilters(true, new FileBrowser.Filter("Images", ".jpg", ".png"), new FileBrowser.Filter("Videos", ".mp4", ".mov"));
        FileBrowser.SetDefaultFilter(".jpg");

        importImageButton.onClick.AddListener(ImportImage);
        importVideoButton.onClick.AddListener(ImportVideo);
    }

    private void ImportImage()
    {
        StartCoroutine(ShowLoadDialogCoroutine(PickMode.Files, HandleImageSelected));
    }

    private void ImportVideo()
    {
        StartCoroutine(ShowLoadDialogCoroutine(PickMode.Files, HandleVideoSelected));
    }

    IEnumerator ShowLoadDialogCoroutine(PickMode pickMode, Action<string[]> callback)
    {
        yield return FileBrowser.WaitForLoadDialog(pickMode, true, null, null, "Load Files", "Load");

        if (FileBrowser.Success)
        {
            callback(FileBrowser.Result);
        }
    }

    private void HandleImageSelected(string[] paths)
    {
        if (paths.Length > 0)
        {
            string path = paths[0];
            byte[] fileData = File.ReadAllBytes(path);
            Texture2D equirectangularImage = new Texture2D(2, 2);
            equirectangularImage.LoadImage(fileData);

            // Set the loaded texture as the Skybox texture
            skyboxMaterial.SetTexture("_MainTex", equirectangularImage);

            // Deactivate the VideoPlayer if it's active
            videoPlayer.Stop();
            videoPlayer.targetTexture = null;
        }
    }

    private void HandleVideoSelected(string[] paths)
    {
        if (paths.Length > 0)
        {
            string path = paths[0];
            // Set the video source and render texture
            videoPlayer.source = VideoSource.Url;
            videoPlayer.url = path;
            videoPlayer.targetTexture = renderTexture;

            // Set the Render Texture as the Skybox material
            skyboxMaterial.SetTexture("_MainTex", renderTexture);


            // Play the video
            videoPlayer.Play();
        }
    }
}






