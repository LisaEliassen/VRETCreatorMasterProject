using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class VisibilityToggles : MonoBehaviour
{
    LoadGlb loadGlb;
    GameObject trigger;
    List<GameObject> triggerCopies;
    public GameObject databaseServiceObject;
    public TMP_Dropdown dropdown;
    public Slider moveSliderX;
    public Slider moveSliderY;
    public Slider sizeSlider;
    public Button addCopyButton;
    public Button removeCopyButton;
    public Toggle objectVisibility;
    public Toggle platformVisibility;
    public GameObject platform;

    // Start is called before the first frame update
    void Start()
    {
        loadGlb = databaseServiceObject.GetComponent<LoadGlb>();

        platformVisibility.onValueChanged.AddListener(PlatformVisibility);
        objectVisibility.onValueChanged.AddListener(ObjectVisibility);

        trigger = loadGlb.GetTrigger();
        if (trigger != null )
        {
            objectVisibility.interactable = true;
        }
        else
        {
            objectVisibility.interactable = false;
        }
    }

    public void ObjectVisibility(bool visible)
    {
        if (trigger == null)
        {
            trigger = loadGlb.GetTrigger();
        }
        if (trigger != null && visible)
        {
            trigger.gameObject.SetActive(true);

            triggerCopies = loadGlb.GetCopies();

            if (triggerCopies != null && triggerCopies.Count > 0 )
            {
                foreach (GameObject copy in triggerCopies)
                {
                    copy.gameObject.SetActive(true);
                }
            }

            addCopyButton.interactable = true;
            removeCopyButton.interactable = true;
            sizeSlider.interactable = true;
            moveSliderX.interactable = true;
            moveSliderY.interactable = true;
            dropdown.interactable = true;
        }
        else if (trigger != null && !visible)
        {
            trigger.gameObject.SetActive(false);

            triggerCopies = loadGlb.GetCopies();

            if (triggerCopies != null && triggerCopies.Count > 0)
            {
                foreach (GameObject copy in triggerCopies)
                {
                    copy.gameObject.SetActive(false);
                }
            }

            addCopyButton.interactable = false;
            removeCopyButton.interactable = false;
            sizeSlider.interactable = false;
            moveSliderX.interactable = false;
            moveSliderY.interactable = false;
            dropdown.interactable = false;
        }
    }

    public void PlatformVisibility(bool visible)
    {
        if (visible)
        {
            platform.GetComponent<MeshRenderer>().enabled = true;
        }
        else
        {
            platform.GetComponent<MeshRenderer>().enabled = false;
        }
    }
}
