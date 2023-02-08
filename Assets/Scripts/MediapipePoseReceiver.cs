using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Globalization;


//Particles tutorial
//https://www.youtube.com/watch?v=agr-QEsYwD0

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
	public Vector3[] pose = new Vector3[32];

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

					for (int i = 0; i < 32; i++)
					{
						//Shift the first two positions in message split 
						pose[i] = new Vector3( float.Parse(messageSplit[i * 4].ToString()) * -1, //Reverse the X axis
											   float.Parse(messageSplit[(i * 4) + 1].ToString()) * -1, //Reverse the Y axis
											   float.Parse(messageSplit[(i * 4) + 2].ToString()));

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
