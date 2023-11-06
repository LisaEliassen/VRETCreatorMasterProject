using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;
using System.IO;

public class ScreenManager : MonoBehaviour
{
    public Button uploadFileButton;
    public Button backButton;
    public GameObject EditSceneUI;
    public GameObject ModelUI;
    public GameObject UploadUI;

    // Start is called before the first frame update
    void Start()
    {
        uploadFileButton.onClick.AddListener(() =>
        {
            EditSceneUI.SetActive(false);
            ModelUI.SetActive(false);
            UploadUI.SetActive(true);
        });
        backButton.onClick.AddListener(() =>
        {
            EditSceneUI.SetActive(true);
            ModelUI.SetActive(false);
            UploadUI.SetActive(false);
        });
    }

}
