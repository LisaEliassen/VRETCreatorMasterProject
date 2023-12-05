using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Collections;
using TMPro;
using System.Linq;
using System.Threading.Tasks;

public class ShowAllSound : MonoBehaviour
{
    DatabaseService dbService;
    SoundManager soundManager;
    public GameObject gridItemPrefab;
    public Transform gridParent;
    public Button addSoundButton;
    public Button backButton;
    public GameObject EditSceneUI;
    public GameObject SoundUI;
    List<FileMetaData> files;

    // Start is called before the first frame update
    void Start()
    {
        // Find the GameObject with the DatabaseService script
        GameObject databaseServiceObject = GameObject.Find("DatabaseService");

        // Check if the GameObject was found
        if (databaseServiceObject != null)
        {
            // Get the DatabaseService component from the found GameObject
            dbService = databaseServiceObject.GetComponent<DatabaseService>();


            soundManager = databaseServiceObject.GetComponent<SoundManager>();
        }
        else
        {
            Debug.LogError("GameObject with DatabaseService not found.");
        }

        files = new List<FileMetaData>();

        addSoundButton.onClick.AddListener(() =>
        {
            //StartCoroutine(FetchSound());
        });

        backButton.onClick.AddListener(() =>
        {
            EditSceneUI.SetActive(true);
            SoundUI.SetActive(false);
        });
    }
}
