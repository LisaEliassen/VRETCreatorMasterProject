using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TextObject : MonoBehaviour
{
    public GameObject databaseServiceObject;
    private TextMeshProUGUI myTextMeshProUI;
    TextSizeManager textSizeManager;

    private void Awake()
    {
        // Check if the GameObject was found
        if (databaseServiceObject != null)
        {
            textSizeManager = databaseServiceObject.GetComponent<TextSizeManager>();
        }
        else
        {
            Debug.LogError("GameObject with DatabaseService not found.");
        }

        myTextMeshProUI = GetComponent<TextMeshProUGUI>();

        if (myTextMeshProUI != null)
        {
            if (textSizeManager != null)
            {
                textSizeManager.RegisterTextObject(myTextMeshProUI);
            }
            else
            {
                Debug.Log("TextSizeManager is null");
            }
        }
        else
        {
            Debug.Log("TextMeshProUGUI was not found.");
        }
        
    }
}