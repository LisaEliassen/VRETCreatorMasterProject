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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// The Start method initializes various UI elements and sets up event listeners for buttons to control the tutorial flow.
// The PrepareButtons method assigns event listeners to buttons for navigating through different tutorial panels, allowing users to move forward and backward in the tutorial sequence.
// The StartTutorial method activates the tutorial UI and hides the initial ask panel to begin the tutorial.
// The HideAskPanel method hides the ask panel by deactivating it and makes the showTutorialButton visible to allow users to start the tutorial.

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
    public GameObject AddSceneryHelpPanel;
    public GameObject RemoveSoundHelpPanel;
    public GameObject CameraControlsPanel;
    public GameObject CameraResetPanel;
    public GameObject SaveLoadScenePanel;
    public GameObject WaitingRoomPasientViewHelpPanel;
    public GameObject StopExposureHelpPanel;

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
    public Button NextButton12;
    public Button NextButton13;
    public Button NextButton14;

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
    public Button PreviousButton11;
    public Button PreviousButton12;
    public Button PreviousButton13;

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
            StopExposureHelpPanel.SetActive(false);
            WaitingRoomPasientViewHelpPanel.SetActive(false);
            CameraResetPanel.SetActive(false);
            SaveLoadScenePanel.SetActive(false);
            RemoveSoundHelpPanel.SetActive(false);
            AddSceneryHelpPanel.SetActive(false);
            AddSoundHelpPanel.SetActive(false);


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
        AddSceneryHelpPanel.SetActive(true);

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
        AddSceneryHelpPanel.SetActive(false);

        UI_parent.SetActive(false);

        PrepareButtons();

        showTutorialButton.onClick.AddListener(StartTutorial);
    }

    public void PrepareButtons()
    {
        NextButton1.onClick.AddListener(() =>
        {
            InfoPanel.SetActive(false);
            CameraControlsPanel.SetActive(true);
            CameraResetPanel.SetActive(true);
        });
        NextButton2.onClick.AddListener(() =>
        {
            CameraControlsPanel.SetActive(false);
            CameraResetPanel.SetActive(false);
            WaitingRoomPasientViewHelpPanel.SetActive(true);
        });
        NextButton3.onClick.AddListener(() =>
        {
            WaitingRoomPasientViewHelpPanel.SetActive(false);
            StopExposureHelpPanel.SetActive(true);
        });
        NextButton4.onClick.AddListener(() =>
        {
            StopExposureHelpPanel.SetActive(false);
            ModelHelpPanel.SetActive(true);
        });
        NextButton5.onClick.AddListener(() =>
        {
            ModelHelpPanel.SetActive(false);
            SizeAndPositionHelpPanel.SetActive(true);
        });
        NextButton6.onClick.AddListener(() =>
        {
            SizeAndPositionHelpPanel.SetActive(false);
            CopiesHelpPanel.SetActive(true);
        });
        NextButton7.onClick.AddListener(() =>
        {
            CopiesHelpPanel.SetActive(false);
            AnimationHelpPanel.SetActive(true);
        });
        NextButton8.onClick.AddListener(() =>
        {
            AnimationHelpPanel.SetActive(false);
            VisibilityToggleHelpPanel.SetActive(true);
        });
        NextButton9.onClick.AddListener(() =>
        {
            RemoveTriggerHelpPanel.SetActive(true);
            VisibilityToggleHelpPanel.SetActive(false);
        });
        NextButton10.onClick.AddListener(() =>
        {
            Media360HelpPanel.SetActive(true);
            RemoveTriggerHelpPanel.SetActive(false);
        });
        NextButton11.onClick.AddListener(() =>
        {
            Media360HelpPanel.SetActive(false);
            AddSoundHelpPanel.SetActive(true);
            RemoveSoundHelpPanel.SetActive(true);
        });

        NextButton12.onClick.AddListener(() =>
        {
            AddSoundHelpPanel.SetActive(false);
            RemoveSoundHelpPanel.SetActive(false);
            AddSceneryHelpPanel.SetActive(true);
        });

        NextButton13.onClick.AddListener(() =>
        {
            AddSceneryHelpPanel.SetActive(false);
            SaveLoadScenePanel.SetActive(true);
        });

        NextButton14.onClick.AddListener(() =>
        {
            SaveLoadScenePanel.SetActive(false);
            HelpUI.SetActive(false);
            showTutorialButton.gameObject.SetActive(true);
        });

        PreviousButton1.onClick.AddListener(() =>
        {
            CameraControlsPanel.SetActive(false);
            CameraResetPanel.SetActive(false);
            InfoPanel.SetActive(true);
        });
        PreviousButton2.onClick.AddListener(() =>
        {
            CameraControlsPanel.SetActive(true);
            CameraResetPanel.SetActive(true);
            WaitingRoomPasientViewHelpPanel.SetActive(false);
        });
        PreviousButton3.onClick.AddListener(() =>
        {
            WaitingRoomPasientViewHelpPanel.SetActive(true);
            StopExposureHelpPanel.SetActive(false);

        });
        PreviousButton4.onClick.AddListener(() =>
        {
            StopExposureHelpPanel.SetActive(true);
            ModelHelpPanel.SetActive(false);

        });
        PreviousButton5.onClick.AddListener(() =>
        {
            ModelHelpPanel.SetActive(true);
            SizeAndPositionHelpPanel.SetActive(false);

        });
        PreviousButton6.onClick.AddListener(() =>
        {
            SizeAndPositionHelpPanel.SetActive(true);
            CopiesHelpPanel.SetActive(false);

        });
        PreviousButton7.onClick.AddListener(() =>
        {
            CopiesHelpPanel.SetActive(true);
            AnimationHelpPanel.SetActive(false);

        });
        PreviousButton8.onClick.AddListener(() =>
        {
            AnimationHelpPanel.SetActive(true);
            VisibilityToggleHelpPanel.SetActive(false);

        });
        PreviousButton9.onClick.AddListener(() =>
        {
            VisibilityToggleHelpPanel.SetActive(true);
            RemoveTriggerHelpPanel.SetActive(false);

        });
        PreviousButton10.onClick.AddListener(() =>
        {
            RemoveTriggerHelpPanel.SetActive(true);
            Media360HelpPanel.SetActive(false);

        });
        PreviousButton11.onClick.AddListener(() =>
        {
            Media360HelpPanel.SetActive(true);
            AddSoundHelpPanel.SetActive(false);
            RemoveSoundHelpPanel.SetActive(false);
        });
        PreviousButton12.onClick.AddListener(() =>
        {
            AddSoundHelpPanel.SetActive(true);
            RemoveSoundHelpPanel.SetActive(true);
            AddSceneryHelpPanel.SetActive(false);
        });
        PreviousButton13.onClick.AddListener(() =>
        {
            AddSceneryHelpPanel.SetActive(true);
            SaveLoadScenePanel.SetActive(false);
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
