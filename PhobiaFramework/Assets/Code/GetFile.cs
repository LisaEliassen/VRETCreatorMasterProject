using Firebase.Storage;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using UnityEngine.Assertions;
using System.Threading.Tasks;
using System.Threading;

public class GetFile : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }
    public void getFile() 
    {
        FirebaseStorage storage = FirebaseStorage.DefaultInstance;

        // Create a reference from a Google Cloud Storage URI
        StorageReference gltfReference =
            storage.GetReferenceFromUrl("gs://vr-framework-95ccc.appspot.com/models/blueJay.gltf");
        StorageReference binReference =
            storage.GetReferenceFromUrl("gs://vr-framework-95ccc.appspot.com/models/blueJay.bin");

        // Create local filesystem URL
        //string localUrl = "file:///local/images/island.jpg";

        /*
        // Download to the local filesystem
        gltfReference.GetFileAsync(localUrl).ContinueWithOnMainThread(task => {
            if (!task.IsFaulted && !task.IsCanceled)
            {
                Debug.Log("File downloaded.");
            }
        });*/
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
