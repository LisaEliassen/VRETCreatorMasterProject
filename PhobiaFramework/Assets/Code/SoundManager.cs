#region License
// Copyright (C) 2024 Lisa Maria Eliassen & Olesya Pasichnyk
// 
// This program is free software: you can redistribute it and/or modify
// it under the terms of the Commons Clause License version 1.0 with GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or any later version.
// 
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// Commons Clause License and GNU General Public License for more details.
// 
// You should have received a copy of the Commons Clause License and GNU General Public License
// along with this program. If not, see <https://commonsclause.com/> and <https://www.gnu.org/licenses/>.
#endregion

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

//The SoundManager script provides functionality to import and play sound files, as well as remove the currently playing sound.
//It includes methods to handle importing sound files from the device, checking if the selected file is a supported sound format, and loading the audio clip asynchronously.
//Additionally, it provides a method to remove the currently playing sound and a coroutine to show a file browser dialog for selecting sound files.

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
}

