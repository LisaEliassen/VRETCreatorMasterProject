using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.IO;
using UnityEngine.Networking;
using UnityEngine;

public class AnimationController : MonoBehaviour
{
    ResourceRequest request;
    ResourceRequest request2;
    public RuntimeAnimatorController baseAnimatorController;

    public async void LoadAnimation(byte[] data, GameObject gameObjectWithAnimator, string clipName)
    {
        // Create a temporary file path
        string localFilePath = Application.persistentDataPath + "/Animations/" + clipName + ".anim";

        // Write the byte array data to a temporary file synchronously
        //await WriteBytesAsync(filePath, data);

        // Add an Animation component to the GameObject
        Animation anim = gameObjectWithAnimator.AddComponent<Animation>();

        await LoadAnimClip(anim, clipName, localFilePath);
    }

    private async Task WriteBytesAsync(string filePath, byte[] data)
    {
        using (FileStream fileStream = new FileStream(filePath, FileMode.Create))
        {
            await fileStream.WriteAsync(data, 0, data.Length);
        }
    }

    public void PlayAnimation(GameObject gameObject, string clipName)
    {
        Animation animationComponent = gameObject.GetComponent<Animation>();

        if (animationComponent != null && !string.IsNullOrEmpty(clipName))
        {
            animationComponent.Play(clipName);
        }
        else
        {
            Debug.LogError("Animation component not found or clip name is empty.");
        }
    }

    private async Task LoadAnimClip(Animation anim, string clipName, string localFilePath)
    {
        string clipPath = "Animations/" + clipName;
        request = Resources.LoadAsync(clipPath, typeof(AnimationClip));
        //request2 = Resources.LoadAsync("Animations/peck", typeof(AnimationClip));

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

        /*while (!request2.isDone)
        {
            await Task.Yield();
        }

        AnimationClip animClip2 = request2.asset as AnimationClip;
        if (animClip2 != null)
        {
            animClip2.legacy = true;

            // Add the AnimationClip to the Animation component
            anim.AddClip(animClip2, "peck");
            anim.wrapMode = WrapMode.Loop;
            // Play the animation
            //anim.Play(clipName);
        }
        else
        {
            Debug.LogError("Failed to load AnimationClip " + "peck.");
        }*/

    }
}
