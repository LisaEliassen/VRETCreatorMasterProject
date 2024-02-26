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
using System.Net;

public class SoundManager : MonoBehaviour
{
    public Button chooseFromDeviceButton;
    public Button removeSoundButton;
    public AudioSource audioSource;
    private AudioClip audioClip;

    public GameObject EditSceneUI;
    public GameObject SoundUI;

    public GameObject databaseServiceObject;
    DatabaseService dbService;
    SceneSaver sceneSaver;

    void Start()
    {
        // Check if the GameObject was found
        if (databaseServiceObject != null)
        {
            // Get the DatabaseService component from the found GameObject
            dbService = databaseServiceObject.GetComponent<DatabaseService>();
            sceneSaver = databaseServiceObject.GetComponent<SceneSaver>();
        }
        else
        {
            Debug.LogError("GameObject with DatabaseService not found.");
        }

        chooseFromDeviceButton.onClick.AddListener(ImportMedia);

        removeSoundButton.onClick.AddListener(RemoveSound);
    }

    public void RemoveSound()
    {
        audioSource.Stop();
        audioClip = null;
        audioSource.clip = audioClip;

        removeSoundButton.interactable = false;

        sceneSaver.SetPathToAudio("");
    }

    private void ImportMedia()
    {
        StartCoroutine(ShowLoadDialogCoroutine(PickMode.Files, HandleMediaSelected));
    }

    private void HandleMediaSelected(string[] paths)
    {
        // FileBrowser.HideDialog(true);
        if (paths.Length > 0)
        {
            string path = paths[0];

            if (IsSoundFile(path))
            {
                StartCoroutine((IEnumerator)HandleSoundSelected(path));
                sceneSaver.SetPathToAudio(path);
            }
            else
            {
                Debug.Log("Selected file is not supported.");
            }
        }
    }

    public async Task<IEnumerator> HandleSoundSelected(string downloadUrl)
    {
        audioClip = await dbService.getAudioClip(downloadUrl);

        if (audioClip != null)
        {
            // Set the loaded audio clip as the AudioSource clip
            audioSource.clip = audioClip;

            // Play the audio
            audioSource.Play();

            removeSoundButton.interactable = true;
        }
        return null;
    }

    private bool IsSoundFile(string path)
    {
        // Adjust the condition to check for sound file extensions
        string extension = Path.GetExtension(path);
        return extension.Equals(".mp3", StringComparison.OrdinalIgnoreCase) || extension.Equals(".wav", StringComparison.OrdinalIgnoreCase);
    }

    IEnumerator ShowLoadDialogCoroutine(PickMode pickMode, Action<string[]> callback)
    {
        FileBrowser.SetFilters(true, new FileBrowser.Filter("Sound Files", ".mp3", ".wav"));
        FileBrowser.SetDefaultFilter(".mp3");

        yield return FileBrowser.WaitForLoadDialog(pickMode, true, null, null, "Load Files", "Load");

        if (FileBrowser.Success)
        {
            callback(FileBrowser.Result);
        }
    }

    /*
    private async void ImportSound()
    {
        string downloadUrl = await dbService.GetDownloadURL("gs://vr-framework-95ccc.appspot.com/sounds/sound.mp3");
        if (!string.IsNullOrEmpty(downloadUrl))
        {
            await HandleSoundSelected(downloadUrl);
        }
        else
        {
            Debug.Log("Failed to retrieve download URL!");
        }
    }

    public async Task<IEnumerator> HandleSoundSelected(string downloadUrl)
    {
        byte[] binaryData = await dbService.getFile(downloadUrl);

        if (binaryData != null)
        {
            // Apply sound data to your sound-related script or AudioSource
            // Example using AudioSource:
            audioSource.clip = WavUtility.ToAudioClip(binaryData, 0, binaryData.Length, 0, 0);
            audioSource.Play();
        }

        return null;
    }*/
}

