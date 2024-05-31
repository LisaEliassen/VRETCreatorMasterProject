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
using System.Collections.Specialized;

// This script is designed to teleport the player to a specified position when a button (stopButton) is clicked. 

public class PanicButtonTeleport : MonoBehaviour
{
    public Button stopButton;
    public GameObject xrRig;
    public GameObject xrRigCamera;
    public GameObject position;

    // Start is called before the first frame update
    void Start()
    {
        stopButton.onClick.AddListener(TeleportPlayer);
    }

    private void TeleportPlayer()
    {
        // Vector is the position which the person will be teleported to
        xrRig.transform.position = position.transform.position;
        xrRigCamera.transform.rotation = Quaternion.Euler(0, 0, 0);
    }
}
