using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoseJointsVisualization : MonoBehaviour
{
    public MediapipePoseReceiver mediapipePoseReceiver;
    public GameObject poseJointsParent;
    public Vector2 imgAspect; 

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < 32; i++)
        {
            poseJointsParent.transform.GetChild(i).transform.position = new Vector3(mediapipePoseReceiver.pose[i].x* imgAspect.x,
                                                                                    mediapipePoseReceiver.pose[i].y* imgAspect.y,
                                                                                    mediapipePoseReceiver.pose[i].z);

        }
    }
}
