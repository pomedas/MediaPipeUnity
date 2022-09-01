using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Select : MonoBehaviour
{
    [SerializeField] GameObject drawButtom;
    [SerializeField] GameObject clearButtom;
    [SerializeField] Draw draw;

    bool drawButtonOn = false;
    bool clearButtonOn = false;

    GameObject leftHand;

    void Start()
    {

    }

    void Update()
    {
        if (leftHand == null && FindObjectOfType<HandCenterVisualization>() != null)
        {
            leftHand = FindObjectOfType<HandCenterVisualization>().lmLeft;
        }
    }

    void FixedUpdate()
    {
        if (leftHand != null)
        {
            Debug.DrawRay(leftHand.transform.position, transform.TransformDirection(Vector3.forward), Color.yellow);

            Vector3 fwd = leftHand.transform.TransformDirection(Vector3.forward) * -10;

            RaycastHit hit;
            if (Physics.Raycast(leftHand.transform.position, fwd, out hit, 10))
            {
                if (hit.transform.gameObject.name == "DrawButtom" && !drawButtonOn)
                {
                    drawButtonOn = true;
                    OnDrawButtomEnter();
                }

                if (hit.transform.gameObject.name == "ClearButtom" && !clearButtonOn)
                {
                    clearButtonOn = true;
                    OnClearButtomEnter();
                }
            }
            else {
                if (drawButtonOn) {
                    drawButtonOn = false;
                    OnDrawButtomExit();
                }

                if (clearButtonOn)
                {
                    clearButtonOn = false;
                    OnClearButtomExit();
                }
            }

        }
    }

    void OnDrawButtomEnter() {
        draw.draw = !draw.draw;
        if (draw.draw)
        {
            drawButtom.GetComponent<Renderer>().material.color = Color.blue;
        }
        else {
            drawButtom.GetComponent<Renderer>().material.color = Color.white;
        }
    }

    void OnDrawButtomExit()
    {
    }

    void OnClearButtomEnter()
    {
        clearButtom.GetComponent<Renderer>().material.color = Color.blue;
        draw.ClearLine();
    }

    void OnClearButtomExit()
    {
        clearButtom.GetComponent<Renderer>().material.color = Color.white;
    }

}
