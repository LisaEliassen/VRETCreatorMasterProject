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
    public Slider sizeSliderTrigger;
    //public Slider sizeSliderSphere;
    public Slider sizeSliderPlatform;
    public TMP_InputField sizeInputTrigger;
    public TMP_InputField sizeInputPlatform;
    public Button addCopyButton;
    public Button removeCopyButton;
    public Toggle interactableToggle;
    public Toggle objectVisibility;
    public Toggle platformVisibility;
    public Toggle wallsVisibility;
    public GameObject platform;
    public GameObject wall;
    public GameObject wall1;
    public GameObject wall2;
    public GameObject ceiling;
    public GameObject doorwall1;
    public GameObject doorwall2;
    public GameObject extraWall1;
    public GameObject extraWall2;
    public GameObject extraWall3;
    public GameObject exposureRoomSign;

    // Start is called before the first frame update
    void Start()
    {
        loadGlb = databaseServiceObject.GetComponent<LoadGlb>();

        platformVisibility.onValueChanged.AddListener(PlatformVisibility);
        objectVisibility.onValueChanged.AddListener(ObjectVisibility);
        wallsVisibility.onValueChanged.AddListener(WallsVisibility);

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
            sizeSliderTrigger.interactable = true;
            sizeInputTrigger.interactable = true;
            moveSliderX.interactable = true;
            moveSliderY.interactable = true;
            dropdown.interactable = true;
            interactableToggle.interactable = true;
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
            sizeSliderTrigger.interactable = false;
            sizeInputTrigger.interactable = false;
            moveSliderX.interactable = false;
            moveSliderY.interactable = false;
            dropdown.interactable = false;
            interactableToggle.interactable = false;
        }
    }

    public void PlatformVisibility(bool visible)
    {
        if (visible)
        {
            platform.GetComponent<MeshRenderer>().enabled = true;
            sizeSliderPlatform.interactable = true;
            sizeInputPlatform.interactable = true;
        }
        else
        {
            platform.GetComponent<MeshRenderer>().enabled = false;
            sizeSliderPlatform.interactable = false;
            sizeInputPlatform.interactable = false;
        }
    }

    public void WallsVisibility(bool visible)
    {
        if (visible)
        {
            wall.GetComponent<MeshRenderer>().enabled = true;
            wall1.GetComponent<MeshRenderer>().enabled = true;
            wall2.GetComponent<MeshRenderer>().enabled = true;
            ceiling.GetComponent<MeshRenderer>().enabled = true;
            doorwall1.GetComponent<MeshRenderer>().enabled = true;
            doorwall2.GetComponent<MeshRenderer>().enabled = true;

            extraWall1.GetComponent<MeshRenderer>().enabled = true;
            extraWall2.GetComponent<MeshRenderer>().enabled = true;
            exposureRoomSign.SetActive(true);
            extraWall3.GetComponent<MeshRenderer>().enabled = true;
        }
        else
        {
            wall.GetComponent<MeshRenderer>().enabled = false;
            wall1.GetComponent<MeshRenderer>().enabled = false;
            wall2.GetComponent<MeshRenderer>().enabled = false;
            ceiling.GetComponent<MeshRenderer>().enabled = false;
            doorwall1.GetComponent<MeshRenderer>().enabled = false;
            doorwall2.GetComponent<MeshRenderer>().enabled = false;

            extraWall1.GetComponent<MeshRenderer>().enabled = false;
            extraWall2.GetComponent<MeshRenderer>().enabled = false;
            exposureRoomSign.SetActive(false);
            extraWall3.GetComponent<MeshRenderer>().enabled = false;
        }
    }
}
