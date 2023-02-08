using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandCenterVisualization : MonoBehaviour
{
    public MediapipeHandsReceiver mediapipeHandsReceiver;

    public GameObject landmark;

    public GameObject leftHandGO;
    public GameObject rightHandGO;

    public GameObject lmLeft;
    public GameObject lmRight;

    public Vector2 imgAspect;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Vector3 leftHand = Vector3.zero;
        Vector3 rightHand = Vector3.zero;

        for (int i = 0; i < 21; i++)
        {
            leftHand += new Vector3(mediapipeHandsReceiver.handLeft[i].x * imgAspect.x,
                                                                              mediapipeHandsReceiver.handLeft[i].y * imgAspect.y,
                                                                              mediapipeHandsReceiver.handLeft[i].z);

            rightHand += new Vector3(mediapipeHandsReceiver.handRight[i].x * imgAspect.x,
                                                                               mediapipeHandsReceiver.handRight[i].y * imgAspect.y,
                                                                               mediapipeHandsReceiver.handRight[i].z);
        }

        lmLeft.transform.position = leftHand / 21;
        lmRight.transform.position = rightHand / 21;
    }
}
