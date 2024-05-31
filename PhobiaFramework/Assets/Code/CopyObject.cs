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
// along with this program.  If not, see <https://commonsclause.com/> and <https://www.gnu.org/licenses/>.
#endregion

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
