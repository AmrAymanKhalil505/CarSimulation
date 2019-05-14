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
	//public byte[] dane;
	public Socket client;
	int WaitForCarToLand=20;
	public bool HalfSpeed;

	private CarController m_Car; // the car controller we want to use

	public void Changing()
	{
		// float speed=m_Car.CurrentSpeed;
		client = new Socket (AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
		client.Connect (IP, Port);//connecting port with ip address 

		byte[] b = new byte[1024];
		int k = client.Receive(b);//recive data from port coming from python script 
		string szReceived = System.Text.Encoding.ASCII.GetString(b, 0, k);//coming data is in bytes converting into string 

		if (client.Connected)
		{
			float temp = 1;
			if(HalfSpeed)
			{
				temp=0.5f;
			}
			string[] words = szReceived.Split(' ');//split data into string data is in 2.2 3.3 4.0
			// which direction will we move the car in, 2--> foreward , 1--> right , 0--> left
			switch (words[0]) 
      		{
				
				case "2":
						m_Car.Move(0f, temp*0.6f, temp*0.6f, 0f);	
						Debug.Log("foreward");
					break;
				case "1":
						m_Car.Move(1.0f,temp*(0.4f), temp*(0.4f), 0f);
						Debug.Log("right");
					break;
				case "0":
						m_Car.Move(-1.0f, temp*(0.2f),temp*(0.2f), 0f);
						Debug.Log("left");
					break;
				case "3":
					m_Car.Move(0f, 0.0f, 0.0f, 0f);
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