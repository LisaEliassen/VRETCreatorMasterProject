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

using UnityEngine;
using System;

// The script provides a method to parse a string representing position, rotation, and scale components of a transform into Vector3 and Quaternion values.

public class TransformParser : MonoBehaviour
{
    public static bool TryParseTransformString(string transformString, out Vector3 position, out Quaternion rotation, out Vector3 scale)
    {
        position = Vector3.zero;
        rotation = Quaternion.identity;
        scale = Vector3.one;

        // Split the transformString into position, rotation, and scale components
        string[] components = transformString.Split(',');
        if (components.Length != 10)
        {
            Debug.Log(components.Length.ToString());

            Debug.LogError("Invalid transform string format: " + transformString);
            return false;
        }

        try
        {
            // Parse position
            position = ParseVector3(components[0], components[1], components[2]);

            // Parse rotation
            rotation = ParseQuaternion(components[3], components[4], components[5], components[6]);

            // Parse scale
            scale = ParseVector3(components[7], components[8], components[9]);
        }
        catch (FormatException e)
        {
            Debug.LogError("Error parsing transform components: " + e.Message);
            return false;
        }

        return true;
    }

    private static Vector3 ParseVector3(string xStr, string yStr, string zStr)
    {
        float x = float.Parse(xStr.Trim('('));
        float y = float.Parse(yStr.Trim());
        float z = float.Parse(zStr.Trim(')'));

        return new Vector3(x, y, z);
    }

    private static Quaternion ParseQuaternion(string xStr, string yStr, string zStr, string wStr)
    {
        float x = float.Parse(xStr.Trim('('));
        float y = float.Parse(yStr.Trim());
        float z = float.Parse(zStr.Trim());
        float w = float.Parse(wStr.Trim(')'));

        return new Quaternion(x, y, z, w);
    }
}
