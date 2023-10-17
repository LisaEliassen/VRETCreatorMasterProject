using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PostFileData : MonoBehaviour
{
    DatabaseService dbService;

    // Start is called before the first frame update
    void Start()
    {
        GameObject databaseServiceObject = GameObject.Find("DatabaseService");

        // Check if the GameObject was found
        if (databaseServiceObject != null)
        {
            // Get the DatabaseService component from the found GameObject
            dbService = databaseServiceObject.GetComponent<DatabaseService>();
        }
        else
        {
            Debug.LogError("GameObject with DatabaseService not found.");
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
