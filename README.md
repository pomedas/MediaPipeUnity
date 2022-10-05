# MediaPipeUnity

A Unity template for creating and application that uses the Media Pipe Pose and Hand tracking for interaction. 
The MediaPipe tracking is executed from a Python script and sends the tracking data using UDP. 
The Unity application receives the data and decodes the information make in ready to be used in an application. 

## MediaPipe scripts
The folder MediaPipeUnity/python/ contains two scripts to execute and send the data to Unity. 
One for hands tracking and one for full body tracking. 
These are examples provided by mediape and are modified to read the detected body joints in a video frame and send the position using UDP protocol. 
Check the README in the python folder to know how to install the dependencies and run the scripts. 

## Unity template
The Unity template contains a scene that reads the data from the tracking and maps it to a game object. 

To execute it, first execute the python script and make sure that the tracking is working correctly. 
Then execute the unity scene and check that the tracking movements are mapped to the 3D objects. 

## References

Media Pipe Pose
https://google.github.io/mediapipe/solutions/pose.html

Media Pipe Hands
https://google.github.io/mediapipe/solutions/hands.html

## Sample video

[![Full Body Interaction](https://img.youtube.com/vi/7y8QfXfct7k/0.jpg)](https://www.youtube.com/watch?v=7y8QfXfct7kE "Full Body Interaction")



