using System.Collections.Generic;
using UnityEngine;
using System.Net;
using System.Net.Sockets;
using System.Linq;
 using System.Collections;

namespace UnityStandardAssets.Vehicles.Car
{
    [RequireComponent(typeof (CarController))]
public class PythonRecievedActions : MonoBehaviour
{
	public string IP = "127.0.0.1"; 
	public int Port = 1234;
	public byte[] dane;
	public Socket client;
	int WaitForCarToLand=30;

	private CarController m_Car; // the car controller we want to use

	public void Changing()
	{
		client = new Socket (AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
		client.Connect (IP, Port);//connecting port with ip address 
		dane = System.Text.Encoding.ASCII.GetBytes("f");//decode string  data into byte for sending 
		client.Send (dane);//send data to port 
		byte[] b = new byte[1024];
		int k = client.Receive(b);//recive data from port coming from python script 
		string szReceived = System.Text.Encoding.ASCII.GetString(b, 0, k);//coming data is in bytes converting into string 

		if (client.Connected)
		{
			string[] words = szReceived.Split(' ');//split data into string data is in 2.2 3.3 4.0
			// which direction will we move the car in, f--> foreward , b--> backward , r--> right , l--> left
			switch (words[0]) 
      		{
				case "f":
					m_Car.Move(0f, 1.0f, 1.0f, 0f);
					break;
				case "b":
					m_Car.Move(0f, -1.0f, -1.0f, 0f);
					break;
				case "r":
					m_Car.Move(1.0f, 0f, 0f, 0f);
					break;
				case "l":
					m_Car.Move(-1.0f, 0f, 0f, 0f);
					break;
      		}
		} 
		else
		{
			Debug.Log (" Not Connected");
		}
		client.Close();	
	}
	void Start ( )
	{
		m_Car = GetComponent<CarController>();
	}

	void Update ()
	{
		if(WaitForCarToLand-- <=0) // wait for the car to land on the track from the sky before moving
			Changing ();
	}
}
}