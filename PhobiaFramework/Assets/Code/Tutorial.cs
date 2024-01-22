using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tutorial : MonoBehaviour
{
    public GameObject EditSceneUI;
    public GameObject HelpUI;
    public GameObject UI_parent;

    public GameObject AskPanel;
    public GameObject InfoPanel;
    public GameObject ModelHelpPanel;
    public GameObject SizeAndPositionHelpPanel;
    public GameObject CopiesHelpPanel;
    public GameObject AnimationHelpPanel;
    public GameObject VisibilityToggleHelpPanel;
    public GameObject RemoveTriggerHelpPanel;
    public GameObject Media360HelpPanel;
    public GameObject AddSoundHelpPanel;
    public GameObject RemoveSoundHelpPanel;
    public GameObject CameraControlsPanel;

    public GameObject RemovePanel;

    public Button yesButton;
    public Button noButton;
    public Button endTutorialButton;
    public Button showTutorialButton;

    public Button NextButton1;
    public Button NextButton2;
    public Button NextButton3;
    public Button NextButton4;
    public Button NextButton5;
    public Button NextButton6;
    public Button NextButton7;
    public Button NextButton8;
    public Button NextButton9;
    public Button NextButton10;
    public Button NextButton11;

    public Button PreviousButton1;
    public Button PreviousButton2;
    public Button PreviousButton3;
    public Button PreviousButton4;
    public Button PreviousButton5;
    public Button PreviousButton6;
    public Button PreviousButton7;
    public Button PreviousButton8;
    public Button PreviousButton9;
    public Button PreviousButton10;

    // Start is called before the first frame update
    void Start()
    {
        AskPanel.SetActive(true);
        EditSceneUI.SetActive(false);

        endTutorialButton.onClick.AddListener(() =>
        {
            ModelHelpPanel.SetActive(false);
            SizeAndPositionHelpPanel.SetActive(false);
            CopiesHelpPanel.SetActive(false);
            AnimationHelpPanel.SetActive(false);
            VisibilityToggleHelpPanel.SetActive(false);
            RemoveTriggerHelpPanel.SetActive(false);
            Media360HelpPanel.SetActive(false);
            CameraControlsPanel.SetActive(false);

            HelpUI.SetActive(false);
        });

        yesButton.onClick.AddListener(() => 
        {
            StartTutorial();
            RemovePanel.SetActive(true);
            RemovePanel.SetActive(false);
        });
        noButton.onClick.AddListener(() =>
        {
            HideAskPanel();
            EditSceneUI.SetActive(true);

            RemovePanel.SetActive(true);
            RemovePanel.SetActive(false);
        });

        UI_parent.SetActive(true);

        endTutorialButton.gameObject.SetActive(true);
        endTutorialButton.gameObject.SetActive(false);

        InfoPanel.SetActive(true);
        ModelHelpPanel.SetActive(true);
        SizeAndPositionHelpPanel.SetActive(true);
        CopiesHelpPanel.SetActive(true);
        AnimationHelpPanel.SetActive(true);
        VisibilityToggleHelpPanel.SetActive(true);
        RemoveTriggerHelpPanel.SetActive(true);
        Media360HelpPanel.SetActive(true);
        AddSoundHelpPanel.SetActive(true);
        RemoveSoundHelpPanel.SetActive(true);
        CameraControlsPanel.SetActive(true);

        InfoPanel.SetActive(false);
        ModelHelpPanel.SetActive(false);
        SizeAndPositionHelpPanel.SetActive(false);
        CopiesHelpPanel.SetActive(false);
        AnimationHelpPanel.SetActive(false);
        VisibilityToggleHelpPanel.SetActive(false);
        RemoveTriggerHelpPanel.SetActive(false);
        Media360HelpPanel.SetActive(false);
        AddSoundHelpPanel.SetActive(false);
        RemoveSoundHelpPanel.SetActive(false);
        CameraControlsPanel.SetActive(false);

        UI_parent.SetActive(false);

        PrepareButtons();

        showTutorialButton.onClick.AddListener(StartTutorial);
    }

    public void PrepareButtons()
    {
        NextButton1.onClick.AddListener(() =>
        {
            InfoPanel.SetActive(false);
            ModelHelpPanel.SetActive(true);
        });
        NextButton2.onClick.AddListener(() =>
        {
            ModelHelpPanel.SetActive(false);
            SizeAndPositionHelpPanel.SetActive(true);
        });
        NextButton3.onClick.AddListener(() =>
        {
            SizeAndPositionHelpPanel.SetActive(false);
            CopiesHelpPanel.SetActive(true);
        });
        NextButton4.onClick.AddListener(() =>
        {
            CopiesHelpPanel.SetActive(false);
            AnimationHelpPanel.SetActive(true);
        });
        NextButton5.onClick.AddListener(() =>
        {
            AnimationHelpPanel.SetActive(false);
            VisibilityToggleHelpPanel.SetActive(true);
        });
        NextButton6.onClick.AddListener(() =>
        {
            VisibilityToggleHelpPanel.SetActive(false);
            RemoveTriggerHelpPanel.SetActive(true);
        });
        NextButton7.onClick.AddListener(() =>
        {
            RemoveTriggerHelpPanel.SetActive(false);
            CameraControlsPanel.SetActive(true);
            //Media360HelpPanel.SetActive(true);
        });
        NextButton8.onClick.AddListener(() =>
        {
            CameraControlsPanel.SetActive(false);
            Media360HelpPanel.SetActive(true);

           // AddSoundHelpPanel.SetActive(true);
        });
        NextButton9.onClick.AddListener(() =>
        {
            Media360HelpPanel.SetActive(false);
            AddSoundHelpPanel.SetActive(true);
            //RemoveSoundHelpPanel.SetActive(true);
        });
        NextButton10.onClick.AddListener(() =>
        {
            AddSoundHelpPanel.SetActive(false);
            RemoveSoundHelpPanel.SetActive(true);
            //CameraControlsPanel.SetActive(true);
        });
        NextButton11.onClick.AddListener(() =>
        {
            //CameraControlsPanel.SetActive(false);
            RemoveSoundHelpPanel.SetActive(false);
            showTutorialButton.gameObject.SetActive(true);
            HelpUI.SetActive(false);
        });


        PreviousButton1.onClick.AddListener(() =>
        {
            ModelHelpPanel.SetActive(false);
            InfoPanel.SetActive(true);
        });
        PreviousButton2.onClick.AddListener(() =>
        {
            ModelHelpPanel.SetActive(true);
            SizeAndPositionHelpPanel.SetActive(false);
        });
        PreviousButton3.onClick.AddListener(() =>
        {
            SizeAndPositionHelpPanel.SetActive(true);
            CopiesHelpPanel.SetActive(false);
        });
        PreviousButton4.onClick.AddListener(() =>
        {
            CopiesHelpPanel.SetActive(true);
            AnimationHelpPanel.SetActive(false);
        });
        PreviousButton5.onClick.AddListener(() =>
        {
            AnimationHelpPanel.SetActive(true);
            VisibilityToggleHelpPanel.SetActive(false);
        });
        PreviousButton6.onClick.AddListener(() =>
        {
            VisibilityToggleHelpPanel.SetActive(true);
            RemoveTriggerHelpPanel.SetActive(false);
        });
        PreviousButton7.onClick.AddListener(() =>
        {
            RemoveTriggerHelpPanel.SetActive(true);
            CameraControlsPanel.SetActive(false);
            //Media360HelpPanel.SetActive(false);
        });
        PreviousButton8.onClick.AddListener(() =>
        {
            Media360HelpPanel.SetActive(false);
            CameraControlsPanel.SetActive(true);
            //AddSoundHelpPanel.SetActive(false);
        });
        PreviousButton9.onClick.AddListener(() =>
        {
            Media360HelpPanel.SetActive(true);
            AddSoundHelpPanel.SetActive(false);
            //RemoveSoundHelpPanel.SetActive(false);
        });
        PreviousButton10.onClick.AddListener(() =>
        {
            RemoveSoundHelpPanel.SetActive(false);
            AddSoundHelpPanel.SetActive(true);
            //CameraControlsPanel.SetActive(false);
        });
    }

    public void StartTutorial()
    {
        HelpUI.SetActive(true);
        HideAskPanel();
        EditSceneUI.SetActive(true);
        InfoPanel.SetActive(true);
        endTutorialButton.gameObject.SetActive(true);
    }

    public void HideAskPanel()
    {
        AskPanel.SetActive(false);
        showTutorialButton.gameObject.SetActive(true);
    }
}
