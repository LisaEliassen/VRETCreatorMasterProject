using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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


    // Update is called once per frame
    void Update()
    {
        
    }
}
