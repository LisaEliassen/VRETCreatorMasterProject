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
