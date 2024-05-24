using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

// The script manages the functionality to add or remove copies of a loaded GLB object. 

public class CopyObject : MonoBehaviour
{
    LoadGlb loadGlb;
    public Button addCopyButton;
    public Button removeCopyButton;
    public TextMeshProUGUI numCopies;
    int numberOfCopies;

    // Start is called before the first frame update
    void Start()
    {
        // Find the GameObject with the DatabaseService script
        GameObject databaseServiceObject = GameObject.Find("DatabaseService");

        loadGlb = databaseServiceObject.GetComponent<LoadGlb>();

        numberOfCopies = 0;

        addCopyButton.onClick.AddListener(AddCopy);
        removeCopyButton.onClick.AddListener(RemoveCopy);
    }

    public void AddCopy()
    {
        if (loadGlb != null)
        {
            numberOfCopies = loadGlb.GetNumCopies();
            if (numberOfCopies < 5)
            {
                numberOfCopies++;
                loadGlb.MakeCopy();
                numCopies.text = numberOfCopies.ToString();

                if (numberOfCopies == 5)
                {
                    addCopyButton.interactable = false;
                }
            }

            if (numberOfCopies > 0)
            {
                removeCopyButton.interactable = true;
            }
        }
    }

    public void RemoveCopy()
    {
        if (loadGlb != null)
        {
            numberOfCopies = loadGlb.GetNumCopies();
            if (numberOfCopies > 0)
            {
                numberOfCopies--;
                loadGlb.RemoveCopy();
                numCopies.text = numberOfCopies.ToString();

                if (numberOfCopies == 0)
                {
                    removeCopyButton.interactable = false;
                }
            }

            if (numberOfCopies < 5)
            {
                addCopyButton.interactable = true;
            }
            
        }
    }
}
