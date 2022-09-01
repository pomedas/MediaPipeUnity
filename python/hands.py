import socket
import cv2
import mediapipe as mp
mp_drawing = mp.solutions.drawing_utils
mp_drawing_styles = mp.solutions.drawing_styles
mp_hands = mp.solutions.hands

UDP_IP = "127.0.0.1"
UDP_PORT = 21900

sock = socket.socket(socket.AF_INET, #Internet
                     socket.SOCK_DGRAM) #UDP

# For webcam input:
hands = mp_hands.Hands(
    model_complexity=0,min_detection_confidence=0.5, min_tracking_confidence=0.5)
cap = cv2.VideoCapture(0)

if cap.isOpened():
  width  = cap.get(cv2.CAP_PROP_FRAME_WIDTH)   # float `width`
  height = cap.get(cv2.CAP_PROP_FRAME_HEIGHT)  # float `height`
  print(str(width) + ' ' + str(height))

while cap.isOpened():
  success, image = cap.read()
  if not success:
    print("Ignoring empty camera frame.")
    # If loading a video, use 'break' instead of 'continue'.
    continue

  # Flip the image horizontally for a later selfie-view display, and convert
  # the BGR image to RGB.
  image = cv2.cvtColor(cv2.flip(image, 1), cv2.COLOR_BGR2RGB)

  # To improve performance, optionally mark the image as not writeable to
  # pass by reference.
  image.flags.writeable = False
  results = hands.process(image)

  #Send landmarks using UDP
  detections = results.multi_hand_landmarks

  index=0
  i=0
  if detections is not None:
      msg = ''
      i=i+1
      for detection in detections:
          handedness = results.multi_handedness[index].classification[0].label
          msg += handedness + ','
          i=i+1
          for landmark in detection.landmark:
              msg+=str(round(landmark.x,3))+','+str(round(landmark.y,3))+','+str(round(landmark.z,3))+','+str(landmark.visibility)+','
              i=i+1
          index=index+1

      sockmsg=bytearray(str(index)+','+msg, 'utf-8')
      sock.sendto(sockmsg, (UDP_IP,UDP_PORT))

  # Draw the hand annotations on the image.
  image.flags.writeable = True
  image = cv2.cvtColor(image, cv2.COLOR_RGB2BGR)
  if results.multi_hand_landmarks:
    for hand_landmarks in results.multi_hand_landmarks:
        mp_drawing.draw_landmarks(
            image,
            hand_landmarks,
            mp_hands.HAND_CONNECTIONS,
            mp_drawing_styles.get_default_hand_landmarks_style(),
            mp_drawing_styles.get_default_hand_connections_style())

  # Flip the image horizontally for a selfie-view display.
  cv2.imshow('MediaPipe Hands', image)

  if cv2.waitKey(5) & 0xFF == 27:
    break

hands.close()
cap.release()
