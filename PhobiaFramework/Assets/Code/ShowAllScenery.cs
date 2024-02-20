using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Collections;
using TMPro;
using System.Linq;
using System.Threading.Tasks;

public class ShowAllScenery : MonoBehaviour
{
    DatabaseService dbService;
    SceneryManager sceneryManager;
    public GameObject databaseServiceObject;
    public GameObject gridItemPrefab360;
    public Transform gridParent;
    public Button addSceneryButton;
    public GameObject EditSceneUI;
    public GameObject SceneryUI;
    List<FileMetaData> files;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
