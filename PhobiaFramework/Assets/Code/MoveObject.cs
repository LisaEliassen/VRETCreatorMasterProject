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

// This script allows for moving an object along the X and Y axes based on slider values. 

public class MoveObject : MonoBehaviour
{
    public Slider moveSliderX;
    public Slider moveSliderY;
    LoadGlb loadGlb;
    Vector3[] positionsX;
    Vector3[] positionsY;
    GameObject trigger;
    List<GameObject> triggerCopies;

    // Start is called before the first frame update
    void Start()
    {
        //moveSliderX = GetComponent<Slider>();

        positionsX = new[] { GameObject.Find("PositionX1").transform.position, GameObject.Find("PositionX2").transform.position, GameObject.Find("PositionX3").transform.position };
        positionsY = new[] { GameObject.Find("PositionY1").transform.position, GameObject.Find("PositionY2").transform.position, GameObject.Find("PositionY3").transform.position };

        moveSliderX.onValueChanged.AddListener(ChangePositionX);
        moveSliderY.onValueChanged.AddListener(ChangePositionY);

        moveSliderX.interactable = false;
        moveSliderY.interactable = false;

        GameObject databaseServiceObject = GameObject.Find("DatabaseService");

        loadGlb = databaseServiceObject.GetComponent<LoadGlb>();

        triggerCopies = new List<GameObject>();
    }

    void ChangePositionX(float position)
    {
        trigger = loadGlb.GetTrigger();
        triggerCopies = loadGlb.GetCopies();

        if (trigger != null)
        {
            int positionIndex = Mathf.RoundToInt(position); // Convert float to int

            if (positionIndex >= 1 && positionIndex <= positionsX.Length)
            {
                trigger.transform.position = positionsX[positionIndex-1];
            }
        }
        else 
        {
            Debug.Log("Trigger is null!");
        }
    }

    void ChangePositionY(float position)
    {
        trigger = loadGlb.GetTrigger();
        triggerCopies = loadGlb.GetCopies();

        float xValue = moveSliderX.value;

        if (trigger != null)
        {
            int positionIndex = Mathf.RoundToInt(position); // Convert float to int

            if (positionIndex >= 1 && positionIndex <= positionsY.Length)
            {
                Vector3 xPos = positionsX[Mathf.RoundToInt(xValue) - 1];
                trigger.transform.position = new Vector3(xPos.x, xPos.y, positionsY[positionIndex-1].z);
            }
        }
        else
        {
            Debug.Log("Trigger is null!");
        }
    }
}
