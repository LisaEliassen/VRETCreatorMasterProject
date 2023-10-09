using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MoveObject : MonoBehaviour
{
    Slider moveSlider;
    Vector3[] positions;
    GameObject trigger;

    // Start is called before the first frame update
    void Start()
    {
        moveSlider = GetComponent<Slider>();
        moveSlider.onValueChanged.AddListener(ChangePosition);

        positions = new[] { GameObject.Find("Position1").transform.position, GameObject.Find("Position2").transform.position, GameObject.Find("Position3").transform.position };
    }

    void ChangePosition(float position)
    {
        trigger = GameObject.Find("Trigger");

        if (trigger != null)
        {
            int positionIndex = Mathf.RoundToInt(position); // Convert float to int

            if (positionIndex >= 1 && positionIndex <= positions.Length)
            {
                trigger.transform.position = positions[positionIndex-1];
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
