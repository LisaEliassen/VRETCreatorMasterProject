using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.IO;
using UnityEngine.Networking;
using UnityEngine;

public class AnimationController : MonoBehaviour
{
    ResourceRequest request;
    public RuntimeAnimatorController baseAnimatorController;

    public async void LoadAnimation(byte[] data, GameObject gameObjectWithAnimator, string clipName)
    {
        // Create a temporary file path
        string filePath = Application.dataPath + "/Resources/Animations/" + clipName + ".anim";

        // Write the byte array data to a temporary file synchronously
        //await WriteBytesAsync(filePath, data);

        // Add an Animation component to the GameObject
        Animation anim = gameObjectWithAnimator.AddComponent<Animation>();

        await LoadAnimClip(anim, clipName);
    }

    private async Task WriteBytesAsync(string filePath, byte[] data)
    {
        using (FileStream fileStream = new FileStream(filePath, FileMode.Create))
        {
            await fileStream.WriteAsync(data, 0, data.Length);
        }
    }

    private async Task LoadAnimClip(Animation anim, string clipName)
    {
        string clipPath = "Animations/" + clipName;
        request = Resources.LoadAsync(clipPath, typeof(AnimationClip));

        while (!request.isDone)
        {
            await Task.Yield();
        }

        AnimationClip animClip = request.asset as AnimationClip;
        if (animClip != null)
        {
            animClip.legacy = true;

            // Add the AnimationClip to the Animation component
            anim.AddClip(animClip, clipName);
            anim.wrapMode = WrapMode.Loop;
            // Play the animation
            anim.Play(clipName);
        }
        else
        {
            Debug.LogError("Failed to load AnimationClip.");
        }

    }
}
