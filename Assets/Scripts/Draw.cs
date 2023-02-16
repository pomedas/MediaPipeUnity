using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class Draw : MonoBehaviour
{
    public bool draw;
    public GameObject landmark;

    //LineRenderer allow to draw a line using an array of points
    //we will use the position of the hand and we will add a new point to the line
    //check the documentation for exploring the properties of LineRenderer
    //https://docs.unity3d.com/Manual/class-LineRenderer.html
    LineRenderer lineRenderer;

    //The list of points that we will update at each frame
    Queue<Vector3> points = new Queue<Vector3>();

    void Start()
    {
        //Gets the line renderer component that is in the same Game Object
        lineRenderer = gameObject.GetComponent<LineRenderer>();

        //Assigns a material and modifies the properties
        lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
        lineRenderer.widthMultiplier = 0.2f;
        lineRenderer.positionCount = 0;
    }

    void Update()
    {
        // We draw in the screen only if the draw variable is true.
        // it can be changed when we want to stop drawing
        if (draw)
        {
            //Adds the curren position of the landmark assigned in the inspector to the line points list
            //We can choose any landmark to paint
            points.Enqueue(landmark.transform.position);
            lineRenderer.positionCount++;
            lineRenderer.SetPositions(points.ToArray());
        }
    }
}
