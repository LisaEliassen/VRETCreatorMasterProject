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
using System.Runtime.InteropServices;
using UnityEngine;

// The script is designed to control the visibility of a waiting room and additional walls based on the position of an XR rig and the state of two doors.
// The script checks the XR rig's position and the doors' rotation in each frame to determine whether to hide or show the waiting room and walls.

public class HideWaitingRoom : MonoBehaviour
{
    public GameObject waitingRoom;
    public GameObject xrRig;
    public GameObject door1;
    public GameObject door2;
    public GameObject extraWall1;
    public GameObject extraWall2;


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(xrRig.gameObject.transform.position);
        // Check if the XR rig is past Z value 5 and the door is closed
        if (xrRig.transform.position.z > -5f && IsDoorClosed())
        {
            Debug.Log("Door is closed");
            // Hide the waiting room
            waitingRoom.SetActive(false);
            extraWall1.SetActive(false);
            extraWall2.SetActive(false);
        }
        else
        {
            // Show the waiting room
            waitingRoom.SetActive(true);
            extraWall1.SetActive(true);
            extraWall2.SetActive(true);
        }
    }

    // Method to check if the door is closed
    bool IsDoorClosed()
    {
        if (door1.transform.rotation.eulerAngles.y <= 5 && door2.transform.rotation.eulerAngles.y <= 185)
        {
            return true;
        }
        else { 
            return false; 
        }
    }
}

