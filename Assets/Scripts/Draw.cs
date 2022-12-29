using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Draw : MonoBehaviour
{
    public bool draw = true;

    LineRenderer lineRenderer;
    GameObject rightHand;
    Queue<Vector3> points = new Queue<Vector3>();

    void Start()
    {
        lineRenderer = gameObject.GetComponent<LineRenderer>();
        lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
        lineRenderer.widthMultiplier = 0.2f;
        lineRenderer.positionCount = 0;
    }

    void Update()
    {
        // Checks and assigns the righHand object
        if (rightHand == null && FindObjectOfType<HandCenterVisualization>() != null)
        {
            rightHand = FindObjectOfType<HandCenterVisualization>().lmRight;
        }
        else
        {
            if (draw)
            {
                //adds the curren position of the hand to the line points list
                points.Enqueue(rightHand.transform.position);
                lineRenderer.positionCount++;
                lineRenderer.SetPositions(points.ToArray());
            }
        }
    }
}
