using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Globalization;

public class MediapipePoseReceiver : MonoBehaviour
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
	int port = 21901;

	// Pose landmarks positions
	// https://google.github.io/mediapipe/solutions/pose.html
	Vector3 nose; //0

	//Pose landmarks representations as GameObjects
	public GameObject noseGO;

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
				if (packet != null && packet.Length > 0)
				{
					string message = ExtractString(packet, 0, packet.Length);
					string[] messageSplit = message.Split(new Char[] { ',' });

					nose = new Vector3(float.Parse(messageSplit[0].ToString()),
															float.Parse(messageSplit[1].ToString())*-1, //Reverse the Y axis
															float.Parse(messageSplit[2].ToString()));

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
		noseGO.transform.position = nose * multiplier;
	}

	private void OnDestroy()
	{
		Close();
	}
}
