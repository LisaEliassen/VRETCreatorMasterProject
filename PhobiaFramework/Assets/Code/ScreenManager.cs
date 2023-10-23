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

    // Start is called before the first frame update
    void Start()
    {
        uploadFileButton.onClick.AddListener(() => changeScene("upload"));
    }

    public void changeScene(string sceneName)
    {
        if (sceneName == "upload")
        {
            SceneManager.LoadScene(2);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
