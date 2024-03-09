using UnityEngine;
using System;

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
