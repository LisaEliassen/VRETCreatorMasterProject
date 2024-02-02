using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class QuitApplication : MonoBehaviour
{
    public GameObject EditScene;
    public GameObject HelpUI;
    public GameObject QuitUI;
    public Button quitButton;
    public Button yesButton;
    public Button noButton;

    // Start is called before the first frame update
    void Start()
    {
        quitButton.onClick.AddListener(() =>
        {
            EditScene.SetActive(false);
            HelpUI.SetActive(false);
            QuitUI.SetActive(true);
        });
        yesButton.onClick.AddListener(Quit);
        noButton.onClick.AddListener(() =>
        {
            QuitUI.SetActive(false);
            HelpUI.SetActive(true);
            EditScene.SetActive(true);
        });
    }

    public void Quit()
    {
        #if UNITY_EDITOR
                    UnityEditor.EditorApplication.isPlaying = false;
        #else
                Application.Quit();
        #endif
    }
}
