using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Globalization;

public class MediapipeHandsReceiver : MonoBehaviour
{
	//
	public float multiplier; 

	//Thread to receive by UDP from the Phone 
	bool on = false;
	private Thread lThread;

	//UDP Receiver definitions
	private UdpClient client;
	private IPEndPoint remoteIpEndPoint;

	//Receiver port
	int port = 21900;

	// One array for each hand with the 21 hand landmarks 
	// https://google.github.io/mediapipe/solutions/hands.html#hand-landmark-model
	public Vector3[] handLeft = new Vector3[21];
	public Vector3[] handRight = new Vector3[21];

	private void Start()
	{
		Listen();
	}

	public void Listen()
	{
		client = new UdpClient(port);
		remoteIpEndPoint = new IPEndPoint(IPAddress.Any, port);

		lThread = new Thread(new ThreadStart(ListenUDPThread));
		lThread.Name = "Receiver UDP listen thread";
		lThread.Start();
	}

	private void ListenUDPThread()
	{
		on = true;      

		Debug.Log("Receiver: listening on port " + port);

		//Forces to use "." as decimal separator
		CultureInfo info = new CultureInfo("es-ES");
		info.NumberFormat.NumberDecimalSeparator = ".";
		Thread.CurrentThread.CurrentCulture = info;
		Thread.CurrentThread.CurrentUICulture = Thread.CurrentThread.CurrentCulture;

		while (on)
		{
			try
			{
				byte[] packet = client.Receive(ref remoteIpEndPoint);

				// read message
				// Message format:
				//
				// 0: number of hands
				// 1: handeness
				// 2-22 (multiplied by 4): x,y,z,visbility of 21 hand landmarks
				// 23:
				//
				if (packet != null && packet.Length > 0)
				{
					string message = ExtractString(packet, 0, packet.Length);
					string[] messageSplit = message.Split(new Char[] { ',' });

					//
					Vector3[] handLandmarks = new Vector3[21];

					for (int i = 0; i < 21; i++)
					{
						//Shift teh first two positions in message split 
						handLandmarks[i] = new Vector3(float.Parse(messageSplit[(i * 4) + 2].ToString()), //Reverse the X axis
													   float.Parse(messageSplit[(i * 4) + 3].ToString()) * -1, //Reverse the Y axis
													   float.Parse(messageSplit[(i * 4) + 4].ToString()));
					}

					//Copy the landmarks to the corresponding hand array
					if (messageSplit[1] == "Right")
					{
						handLandmarks.CopyTo(handRight, 0);
					}
					else {
						handLandmarks.CopyTo(handLeft, 0);
					}

					
					//We check if we are receiving the two hands to parse the rest of the message
					if (int.Parse(messageSplit[0].ToString()) == 2) {

						Vector3[] handLandmarks2 = new Vector3[21];

						for (int i = 0; i < 21; i++)
						{
							//Shift the two first positions + 80 positions of the first hand + handeness of second hand
							handLandmarks2[i] = new Vector3(float.Parse(messageSplit[(21 * 4) + (i * 4) + 3].ToString()), //Reverse the X axis
															float.Parse(messageSplit[(21 * 4) + (i * 4) + 4].ToString()) * -1, //Reverse the Y axis
															float.Parse(messageSplit[(21 * 4) + (i * 4) + 5].ToString()));
						}

						//Copy the landmarks to the corresponding hand array
						if (messageSplit[(21*4)+2] == "Right")
						{
							handLandmarks2.CopyTo(handRight, 0);
						}
						else
						{
							handLandmarks2.CopyTo(handLeft, 0);
						}

					}
					
				}
				if (packet != null && packet.Length == 0)
				{
					Debug.Log("Received packet is empty");
				}
			}
			catch (Exception e)
			{
				Debug.Log(e.ToString());
			}
		}
	}

	public void Close()
	{
		on = false;
		lThread.Abort();
		if (client != null) client.Close();
	}

	private string ExtractString(byte[] packet, int start, int length)
	{
		StringBuilder sb = new StringBuilder();
		for (int i = 0; i < packet.Length; i++) sb.Append((char)packet[i]);
		return sb.ToString();
	}

	private void OnDestroy()
	{
		Close();
	}
}
