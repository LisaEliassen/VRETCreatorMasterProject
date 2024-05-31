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
using UnityEngine.SceneManagement;
using UnityEngine.Networking;
using static System.Net.WebRequestMethods;
using TMPro;
using Firebase;
using Firebase.Auth;
using System.Threading.Tasks;

// The script handles user authentication using Firebase Authentication.
// It initializes the Firebase Authentication instance and sets up an onClick listener for the sign-in button.
// When the sign-in button is clicked, it attempts to sign in the user with the provided email and password.
// Upon successful sign-in, it sets a flag indicating the sign-in process is complete.
// In the Update method, it checks if the sign-in process is complete and ensures that the next scene is loaded only once.
// If the sign-in process is complete and the next scene has not been loaded yet, it starts a coroutine to load the next scene after a short delay.

public class SignInManager : MonoBehaviour
{
    public TMP_InputField emailInputField;
    public TMP_InputField passwordInputField;
    public UnityEngine.UI.Button signInButton;

    bool hasLoadedNextScene = false; // Flag to ensure the scene is loaded only once
    bool isSignInComplete = false;
    FirebaseAuth auth;

    void Start()
    {
        // Initialize Firebase Authentication instance
        auth = FirebaseAuth.DefaultInstance;

        // Add an onClick listener to the sign-in button
        signInButton.onClick.AddListener(SignIn);
    }

    void SignIn()
    {
        string email = emailInputField.text;
        string password = passwordInputField.text;

        // Sign in the user with the email and password
        auth.SignInWithEmailAndPasswordAsync(email, password).ContinueWith(task =>
        {
            if (task.IsCanceled)
            {
                Debug.LogError("SignInWithEmailAndPasswordAsync was canceled.");
                return;
            }
            if (task.IsFaulted)
            {
                Debug.LogError("SignInWithEmailAndPasswordAsync encountered an error: " + task.Exception);
                return;
            }

            // Get the result of the authentication task
            Firebase.Auth.AuthResult authResult = task.Result;
            Firebase.Auth.FirebaseUser user = authResult.User;
            Debug.LogFormat("User signed in successfully: {0} ({1})", user.DisplayName, user.UserId);

            isSignInComplete = true;
        });
    }

    void Update()
    {
        if (isSignInComplete && !hasLoadedNextScene)
        {
            isSignInComplete = false; // Reset the flag
            hasLoadedNextScene = true; // Set the flag to true to ensure it's not called multiple times
            StartCoroutine(LoadNextScene());
        }
    }

    IEnumerator LoadNextScene()
    {        
        yield return new WaitForSeconds(0.1f); // Adjust the delay time if necessary
        SceneManager.LoadScene(1);
    }
}
