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
    public Button chooseMediaButton;
    public Button backFromUploadButton;
    public Button backFrom360Media;
    public GameObject EditSceneUI;
    public GameObject ModelUI;
    public GameObject UploadUI;
    public GameObject ChooseMediaUI;

    // Start is called before the first frame update
    void Start()
    {
        uploadFileButton.onClick.AddListener(() => ShowUI("Upload"));
        chooseMediaButton.onClick.AddListener(() => ShowUI("360Media"));
        backFrom360Media.onClick.AddListener(() => ShowUI("EditScene"));
        backFromUploadButton.onClick.AddListener(() => ShowUI("EditScene"));
    }

    public void ShowUI(string UIname)
    {
        if (UIname == "EditScene")
        {
            EditSceneUI.SetActive(true);
            ModelUI.SetActive(false);
            ChooseMediaUI.SetActive(false);
            UploadUI.SetActive(false);
        }
        else if (UIname == "Upload")
        {
            EditSceneUI.SetActive(false);
            ModelUI.SetActive(false);
            ChooseMediaUI.SetActive(false);
            UploadUI.SetActive(true);
        }
        else if (UIname == "360Media")
        {
            EditSceneUI.SetActive(false);
            ModelUI.SetActive(false);
            ChooseMediaUI.SetActive(true);
            UploadUI.SetActive(false);
        }
    }

}
