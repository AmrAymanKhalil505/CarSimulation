// Names: Mohamed Lotfy
using System.Collections.Generic;
using UnityEngine;
using System.Net;
using System.Net.Sockets;
using System.Linq;
using System.Collections;


/* Functions : Start, Update, getCurrentAction
Date Edited : 26/5/2019
*/


namespace UnityStandardAssets.Vehicles.Car{ 
	[RequireComponent(typeof (CarController))]
	public class unityPythonConnection : MonoBehaviour{
		/* variables */
		/* the TCP connection parameters */
		public string IP = "127.0.0.1"; 
		public int Port ;
		public Socket client;
		int WAIT_FOR_LANDING = 20; /* counter that pause the care movement until it lands on the track floor */
		public bool HalfSpeed;     /* a flag that will allow this car to move at half the speed of the other cars in the track */
		private CarController carController; /* the controller which will be used to move the car in the autonomus mode */
		/* function */
		/* the start function is executed in the very begining of the scene. Here the car controller module is initialized. 
		The car controller module here is responsible for moving the car around the track in the autonomus mode.
		*/
		void Start ( ){
			carController = GetComponent<CarController>();
		}
		/* function */
		/* executed once per frame. the getcurrentAction() function will be executed only after the 
		landing of the sky car on the track and this is the role of the cindition in this update function.
		*/
		void Update (){
			if(--WAIT_FOR_LANDING <= 0){
				getCurrentAction ();
			}
		}
		/* function */
		/* this function is responsible for recieving the respective actions from the Python brain and move the car respectively based on these actions. 
		*/
		public void getCurrentAction(){
			/* the connection to the brain and recieving the respective action code form the server */
			client = new Socket (AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
			client.Connect (IP, Port);
			byte[] recievingBuffer = new byte[1024];
			int byteCount = client.Receive(recievingBuffer);
			string recievedData = System.Text.Encoding.ASCII.GetString(recievingBuffer, 0, byteCount);
			/* move the car based on the recieved actions from the Python brain */
			if (client.Connected){
				float reduceSpeedBy = 1;
				if(HalfSpeed){
					reduceSpeedBy = 0.5f;
				}
				string[] recievedDataArray = recievedData.Split(' ');
				switch (recievedDataArray[0]){
					case "2":
						carController.Move(0f, reduceSpeedBy*0.6f, reduceSpeedBy*0.6f, 0f);	
						Debug.Log("Foreward");
						break;
					case "1":
						carController.Move(1.0f,reduceSpeedBy*(0.4f), reduceSpeedBy*(0.4f), 0f);
						Debug.Log("Right");
						break;
					case "0":
						carController.Move(-1.0f, reduceSpeedBy*(0.2f),reduceSpeedBy*(0.2f), 0f);
						Debug.Log("Left");
						break;
					case "3":
						carController.Move(0f, 0.0f, 0.0f, 0f);
						break;
				}
			} 
			else{
				Debug.Log ("Connection Error");
			}
			client.Close();	
		}
	}
}
/* END OF FILE */