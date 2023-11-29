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
    public Button deleteFilesButton;
    public Button backFromDeleteButton;
    public GameObject EditSceneUI;
    public GameObject ModelUI;
    public GameObject UploadUI;
    public GameObject ChooseMediaUI;
    public GameObject DeleteUI;
    public GameObject SettingsUI;
    public GameObject ControlsUI;

    // Start is called before the first frame update
    void Start()
    {
        uploadFileButton.onClick.AddListener(() => ShowUI("Upload"));
        chooseMediaButton.onClick.AddListener(() => ShowUI("360Media"));
        backFrom360Media.onClick.AddListener(() => ShowUI("EditScene"));
        backFromUploadButton.onClick.AddListener(() => ShowUI("EditScene"));
        deleteFilesButton.onClick.AddListener(() => ShowUI("Delete"));
        backFromDeleteButton.onClick.AddListener(() => ShowUI("Upload"));

        ModelUI.SetActive(true);
        ChooseMediaUI.SetActive(true);
        UploadUI.SetActive(true);
        DeleteUI.SetActive(true);
        SettingsUI.SetActive(true);
        ControlsUI.SetActive(true);

        ModelUI.SetActive(false);
        ChooseMediaUI.SetActive(false);
        UploadUI.SetActive(false);
        DeleteUI.SetActive(false);
        SettingsUI.SetActive(false);
        ControlsUI.SetActive(false);
    }

    public void ShowUI(string UIname)
    {
        if (UIname == "EditScene")
        {
            EditSceneUI.SetActive(true);
            ModelUI.SetActive(false);
            ChooseMediaUI.SetActive(false);
            UploadUI.SetActive(false);
            DeleteUI.SetActive(false);
            SettingsUI.SetActive(false);
            ControlsUI.SetActive(false);
        }
        else if (UIname == "Upload")
        {
            EditSceneUI.SetActive(false);
            ModelUI.SetActive(false);
            ChooseMediaUI.SetActive(false);
            UploadUI.SetActive(true);
            DeleteUI.SetActive(false);
            SettingsUI.SetActive(false);
            ControlsUI.SetActive(false);
        }
        else if (UIname == "360Media")
        {
            EditSceneUI.SetActive(false);
            ModelUI.SetActive(false);
            ChooseMediaUI.SetActive(true);
            UploadUI.SetActive(false);
            DeleteUI.SetActive(false);
            SettingsUI.SetActive(false);
            ControlsUI.SetActive(false);
        }
        else if(UIname == "Delete")
        {
            EditSceneUI.SetActive(false);
            ModelUI.SetActive(false);
            ChooseMediaUI.SetActive(false);
            UploadUI.SetActive(false);
            DeleteUI.SetActive(true);
            SettingsUI.SetActive(false);
            ControlsUI.SetActive(false);
        }
    }

}
