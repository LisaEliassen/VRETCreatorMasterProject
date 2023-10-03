using Firebase.Storage;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using UnityEngine.Assertions;
using System.Threading.Tasks;
using System.Threading;

public class AddFile : MonoBehaviour
{
    // C:\Users\Student\Documents\GitHub\Master\PhobiaFramework\Assets\living birds\models\blueJay.fbx
    // Start is called before the first frame update
    void Start()
    {

    }

    public void addFile() 
    {
        // Get a reference to the storage service, using the default Firebase App
        FirebaseStorage storage = FirebaseStorage.DefaultInstance;

        // Create a storage reference from our storage service
        StorageReference storageRef =
            storage.GetReferenceFromUrl("gs://vr-framework-95ccc.appspot.com");

        // Create a child reference
        // imagesRef now points to "images"
        StorageReference modelsRef = storageRef.Child("models");

        // Child references can also take paths delimited by '/' such as:
        // "images/space.jpg".
        StorageReference bluejayRef = modelsRef.Child("blueJay.FBX");

        // This is equivalent to creating the full referenced
        StorageReference bluejayRefFull = storage.GetReferenceFromUrl(
            "gs://vr-framework-95ccc.appspot.com/models/blueJay.FBX");

        // File located on disk
        string localFile = "Assets/living birds/models/blueJay.FBX";

        // Create a reference to the file you want to upload
        //StorageReference bluejayfbxRef = storageRef.Child("");

        // Upload the file to the path "images/rivers.jpg"
        /*bluejayRefFull.PutFileAsync(localFile)
            .ContinueWith((Task<StorageMetadata> task) => {
                if (task.IsFaulted || task.IsCanceled)
                {
                    Debug.Log(task.Exception.ToString());
                    // Uh-oh, an error occurred!
                }
                else
                {
                    // Metadata contains file metadata such as size, content-type, and download URL.
                    StorageMetadata metadata = task.Result;
                    string md5Hash = metadata.Md5Hash;
                    Debug.Log("Finished uploading...");
                    Debug.Log("md5 hash = " + md5Hash);
                }
            });*/


        //Assets/Assets/blueJay.gltf

        // Create storage reference
        StorageReference Ref = storageRef.Child("models/blueJay.FBX");

        byte[] customBytes = new byte[] {
        };
        // Create file metadata including the content type
        var newMetadata = new MetadataChange();
        newMetadata.ContentType = "application/octet-stream";

        // Upload data and metadata
        Ref.PutBytesAsync(customBytes, newMetadata, null,
            CancellationToken.None); // .ContinueWithOnMainThread(...
                                     // Upload file and metadata
        Ref.PutFileAsync(localFile, newMetadata, null,
            CancellationToken.None); // .ContinueWithOnMainThread(...

        // Upload file and metadata
        bluejayRefFull.PutFileAsync(localFile, newMetadata, null, CancellationToken.None)
            .ContinueWith((Task<StorageMetadata> task) =>
            {
                if (task.IsFaulted || task.IsCanceled)
                {
                    Debug.Log(task.Exception.ToString());
                    // Uh-oh, an error occurred!
                }
                else
                {
                    // Metadata contains file metadata such as size, content-type, and download URL.
                    StorageMetadata metadata = task.Result;
                    string md5Hash = metadata.Md5Hash;
                    Debug.Log("Finished uploading...");
                    Debug.Log("md5 hash = " + md5Hash);
                }
            });
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
