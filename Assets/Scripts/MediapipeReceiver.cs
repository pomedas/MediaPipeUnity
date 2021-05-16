using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Globalization;

public class MediapipeReceiver : MonoBehaviour
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

	// Hand landmarks positions
	// https://google.github.io/mediapipe/solutions/hands.html
	Vector3 wrist; //0
	Vector3 thumbTip; //4
	Vector3 indexFingerTip; //8
	Vector3 middleFingerTip; //12
	Vector3 ringFingerTip; //16
	Vector3 pinkyTip; //20

	//Hand landmarks representations as GameObjects
	public GameObject wristGO;
	public GameObject thumbTipGO;
	public GameObject indexFingerTipGO;
	public GameObject middleFingerTipGO;
	public GameObject ringFingerTipGO;
	public GameObject pinkyTipGO;

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
		var culture = (CultureInfo)CultureInfo.CurrentCulture.Clone();
		//Debug.Log("separator: "+culture.NumberFormat.NumberDecimalSeparator);
		on = true;

		Debug.Log("Receiver: listening on port " + port);

		while (on)
		{
			try
			{
				byte[] packet = client.Receive(ref remoteIpEndPoint);

				// read message
				if (packet != null && packet.Length > 0)
				{
					string message = ExtractString(packet, 0, packet.Length);
					string[] messageSplit = message.Split(new Char[] { ',' });

					wrist = new Vector3(float.Parse(messageSplit[0].ToString()),
															float.Parse(messageSplit[1].ToString())*-1, //Reverse the Y axis
															float.Parse(messageSplit[2].ToString()));

					thumbTip = new Vector3( float.Parse(messageSplit[4*4].ToString()),
																float.Parse(messageSplit[(4*4)+1].ToString()) * -1, //Reverse the Y axis
																float.Parse(messageSplit[(4*4)+2].ToString()));

					indexFingerTip = new Vector3(float.Parse(messageSplit[8*4].ToString()),
																	float.Parse(messageSplit[(8*4)+1].ToString()) * -1, //Reverse the Y axis
																	float.Parse(messageSplit[(8*4)+2].ToString()));

					middleFingerTip = new Vector3(float.Parse(messageSplit[12 * 4].ToString()),
												float.Parse(messageSplit[(12 * 4) + 1].ToString()) * -1, //Reverse the Y axis
												float.Parse(messageSplit[(12 * 4) + 2].ToString()));

					ringFingerTip = new Vector3(float.Parse(messageSplit[16 * 4].ToString()),
												float.Parse(messageSplit[(16 * 4) + 1].ToString()) * -1, //Reverse the Y axis
												float.Parse(messageSplit[(16 * 4) + 2].ToString()));

					pinkyTip = new Vector3(float.Parse(messageSplit[20 * 4].ToString()),
												float.Parse(messageSplit[(20 * 4) + 1].ToString()) * -1, //Reverse the Y axis
												float.Parse(messageSplit[(20 * 4) + 2].ToString()));

					//Debug.Log("Receiver: "+message);
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

	private void Update()
	{
		//Assign the positions received in the socket to the objects
		wristGO.transform.position = wrist * multiplier;
		thumbTipGO.transform.position = thumbTip * multiplier;
		indexFingerTipGO.transform.position = indexFingerTip * multiplier;
		middleFingerTipGO.transform.position = middleFingerTip * multiplier;
		ringFingerTipGO.transform.position = ringFingerTip * multiplier;
		pinkyTipGO.transform.position = pinkyTip * multiplier;
	}

	private void OnDestroy()
	{
		Close();
	}
}
