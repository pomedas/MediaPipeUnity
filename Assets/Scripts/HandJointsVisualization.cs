using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandJointsVisualization : MonoBehaviour
{
    public MediapipeHandsReceiver mediapipeHandsReceiver;
    public GameObject landmark;
    public GameObject leftHandGO;
    public GameObject rightHandGO;
    public Vector2 imgAspect; 

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < 21; i++) {

            GameObject lmLeft = Instantiate(landmark, Vector3.zero,Quaternion.identity);
            lmLeft.transform.SetParent(leftHandGO.transform);

            GameObject lmRight = Instantiate(landmark, Vector3.zero, Quaternion.identity);
            lmRight.transform.SetParent(rightHandGO.transform);
        } 
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < 21; i++)
        {
            leftHandGO.transform.GetChild(i).transform.position = new Vector3(mediapipeHandsReceiver.handLeft[i].x* imgAspect.x,
                                                                              mediapipeHandsReceiver.handLeft[i].y* imgAspect.y,
                                                                              mediapipeHandsReceiver.handLeft[i].z);

            rightHandGO.transform.GetChild(i).transform.position = new Vector3(mediapipeHandsReceiver.handRight[i].x * imgAspect.x,
                                                                               mediapipeHandsReceiver.handRight[i].y * imgAspect.y,
                                                                               mediapipeHandsReceiver.handRight[i].z);

        }
    }
}
