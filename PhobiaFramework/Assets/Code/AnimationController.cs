using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.IO;
using UnityEngine.Networking;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AnimationController : MonoBehaviour
{
    ResourceRequest request;
    ResourceRequest request2;
    public RuntimeAnimatorController baseAnimatorController;
    public TMP_Dropdown dropdown;
    GameObject trigger;
    List<AnimationClip> animationClipList;
    List<TMP_Dropdown.OptionData> options;
    Animation animationComponent;

    void Start()
    {
        // Ensure that the dropdown is assigned in the Inspector
        if (dropdown != null)
        {
            // Add a listener to the dropdown's onValueChanged event
            dropdown.onValueChanged.AddListener(delegate {
                DropdownValueChanged(dropdown);
            });
        }
        else
        {
            Debug.LogError("TMP Dropdown is not assigned.");
        }
        dropdown.interactable = false;
    }

    public void FindAnimations(GameObject gameObject)
    {
        trigger = gameObject;

        if (trigger != null)
        {
            animationComponent = trigger.GetComponent<Animation>();

            if (animationComponent != null)
            {
                animationClipList = new List<AnimationClip>();
                options = new List<TMP_Dropdown.OptionData>();
                int clipCount = animationComponent.GetClipCount();

                if (clipCount > 0)
                {
                    dropdown.interactable = true;
                    foreach (AnimationState clip in animationComponent)
                    {
                        animationClipList.Add(animationComponent.GetClip(clip.name));
                        options.Add(new TMP_Dropdown.OptionData(clip.name));
                    }

                    if (dropdown != null)
                    {
                        dropdown.ClearOptions(); // Clear any existing options
                        options.Add(new TMP_Dropdown.OptionData("Pause animation"));
                        dropdown.AddOptions(options); // Add the new options
                    }
                    else
                    {
                        Debug.LogError("TMP Dropdown is not assigned.");
                    }
                }
                else
                {
                    dropdown.interactable = false;
                }
            }
            else
            {
                Debug.Log("Animation component not found!");
                dropdown.ClearOptions();
                dropdown.interactable = false;
            }
        }
        else
        {
            dropdown.interactable = false;
            Debug.Log("Trigger is not found!");
        }
    }

    // This method will be called whenever the dropdown's value changes
    void DropdownValueChanged(TMP_Dropdown change)
    {
        Debug.Log("Dropdown value changed to: " + change.options[change.value].text);

        PlayAnimation(trigger, change.options[change.value].text);
    }

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
        if (animationComponent != null && !string.IsNullOrEmpty(clipName))
        {
            if (clipName == "Pause animation")
            {
                animationComponent.Stop();
            }
            animationComponent.Play(clipName);
        }
        else
        {
            Debug.LogError("Animation component not found or clip name is empty.");
        }
    }

    public void StopAnimation(GameObject trigger)
    {
        Animation animation = trigger.GetComponent<Animation>();
        animation.Stop();
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
