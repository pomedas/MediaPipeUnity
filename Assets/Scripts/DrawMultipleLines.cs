using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawMultipleLines : MonoBehaviour
{
    public bool draw;
    public GameObject landmark;

    enum DrawState { ON, OFF};
    DrawState drawState = DrawState.OFF;

    //LineRenderer allow to draw a line using an array of points
    //we will use the position of the hand and we will add a new point to the line
    //check the documentation for exploring the properties of LineRenderer
    //https://docs.unity3d.com/Manual/class-LineRenderer.html
    LineRenderer lineRenderer;

    //The list of points that we will update at each frame
    Queue<Vector3> points = new Queue<Vector3>();

    void Start()
    {

    }

    void Update()
    {
        // We draw in the screen only if the draw variable is true.
        // it can be changed when we want to stop drawing
        if (draw)
        {
            //Detects if we are switching the state
            if (drawState == DrawState.OFF)
            {
                //Assing the value of the new state
                drawState = DrawState.ON;

                //Creates a new game object with a line renderer that we are going to use to draw the new line
                GameObject newLineGO = new GameObject();
                newLineGO.name = "line";

                //Makes the new game object child of this Game Object
                newLineGO.transform.parent = transform;
                //Adds the line renderer component to the new object
                lineRenderer = newLineGO.AddComponent<LineRenderer>();

                //Assigns a material and modifies the properties of the line renderer - you can expose and change this properties
                lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
                lineRenderer.widthMultiplier = 0.2f;
                lineRenderer.positionCount = 0;

                //clears the points queue to start with an empty line
                points.Clear();
            }


            //Adds the curren position of the landmark assigned in the inspector to the line points list
            //We can choose any landmark to paint
            points.Enqueue(landmark.transform.position);
            lineRenderer.positionCount++;
            lineRenderer.SetPositions(points.ToArray());
        }
        else {

            // Detects if we are switch the state
            if (drawState == DrawState.ON)
            {
                drawState = DrawState.OFF;
            }
        }
    }
}
