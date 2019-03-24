using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using UnityEngine;
using System.IO;
using System;

	
[RequireComponent(typeof(Camera))]
public class SnapCode : MonoBehaviour
{
    //public String path;
    Camera Snapcam;
    public int resWidth = 1080;
    public int resHight = 1080;
    int snapCounter;
    int frameCounter = 0;
    public int snapFrameCounter = 50;
    int launchCount;
    private List<string[]> rowData = new List<string[]>();
    int counting;
		#region private members 	
	private TcpClient socketConnection; 	
	private Thread clientReceiveThread; 	
    private String currentKey;

	#endregion  	
    private void Start()
    {   ConnectToTcpServer();
        snapCounter = 0;
        counting=0;
    }
//---------------------------------------------------------------------------------------------------------------
	// void Update () {         
	// 	if (Input.GetKeyDown(KeyCode.Z)) {             
	// 		SendMessage(snapCounter-1,launchCount);         
	// 	}     
	// }  	
        public void setcurrentKey(String comingValue)
            {
                currentKey=comingValue;
            }

	    private void ConnectToTcpServer () { 		
		try {  			
			clientReceiveThread = new Thread (new ThreadStart(ListenForData)); 			
			clientReceiveThread.IsBackground = true; 			
			clientReceiveThread.Start();  		
		} 		
		catch (Exception e) { 			
			Debug.Log("On client connect exception " + e); 		
		} 	
	}
        private void ListenForData() { 		
		try { 			
			socketConnection = new TcpClient("127.0.0.1", 1234);  			
			Byte[] bytes = new Byte[1024];             
			while (true) { 				
				// Get a stream object for reading 				
				using (NetworkStream stream = socketConnection.GetStream()) { 					
					int length; 					
					// Read incomming stream into byte arrary. 					
					while ((length = stream.Read(bytes, 0, bytes.Length)) != 0) { 						
						var incommingData = new byte[length]; 						
						Array.Copy(bytes, 0, incommingData, 0, length); 						
						// Convert byte array to string message. 						
						string serverMessage = Encoding.ASCII.GetString(incommingData); 						
						Debug.Log("server message received as: " + serverMessage); 					
					} 				
				} 			
			}         
		}         
		catch (SocketException socketException) {             
			Debug.Log("Socket exception: " + socketException);         
		}     
	}  	


    private void SendMessage(int count,int launch) {         
		if (socketConnection == null) {             
			return;         
		}  		
		try { 			
			// Get a stream object for writing. 			
			NetworkStream stream = socketConnection.GetStream(); 			
            Debug.Log(count);
			if (stream.CanWrite) {                 
	    
			Stream imageFileStream = File.OpenRead("C:/Users/Loai/Desktop/Snapshots/"+"Session_"+launch+"_"+count+".png");
            byte[] clientMessageAsByteArray=new byte[imageFileStream.Length];	// Convert string message to byte array.                 
		    imageFileStream.Read(clientMessageAsByteArray, 0, (int)imageFileStream.Length);
                //byte[] clientMessageAsByteArray = Encoding.ASCII.GetBytes(clientMessage); 				
				// Write byte array to socketConnection stream.                 
				stream.Write(clientMessageAsByteArray, 0, clientMessageAsByteArray.GetLength(0));                 
				Debug.Log("Client sent his message - should be received by server");             
			}         
		} 		
		catch (SocketException socketException) {             
			Debug.Log("Socket exception: " + socketException);         
		}     
	} 
	//------------------------------------------------------------------------------------------------
    private void Awake()
    {
        Snapcam = GetComponent<Camera>();
        if (Snapcam.targetTexture == null)
        {
            Snapcam.targetTexture = new RenderTexture(resWidth, resHight, 24);
        }
        launchCount = PlayerPrefs.GetInt("TimesLaunched", 0);

        // After Grabbing 'TimesLaunched' we increment the value by 1
        launchCount = launchCount + 1;

        // Set 'TimesLaunched' To The Incremented Value
        PlayerPrefs.SetInt("TimesLaunched", launchCount);

        // Now I Would Destroy The Script Or Whatever You
        // Want To Do To Prevent It From Running Multiple
        // Times In One Launch Session
    }

    // Update is called once per frame

    void LateUpdate()
    {
            if (frameCounter >= snapFrameCounter)
        {
        Texture2D snapshot = new Texture2D(resWidth, resHight, TextureFormat.RGB24, false);
        Snapcam.Render();
        RenderTexture.active = Snapcam.targetTexture;
        snapshot.ReadPixels(new Rect(0, 0, resWidth, resHight), 0, 0);
        byte[] bytes = snapshot.EncodeToPNG();
        string fileName = SnapshotName();
        snapCounter++;
         System.IO.File.WriteAllBytes(fileName, bytes);
		 if(snapCounter>2 ){//&& snapCounter%2 ==0){
		    SendMessage(snapCounter-1,launchCount);}
		 Save(snapCounter,launchCount,currentKey);
        Debug.Log("Snapshot taken !");
        }
        else{
            frameCounter++;
        }

    }

    string SnapshotName()
    {
   return string.Format("{0}/Snapshots/Session_{1}_{2}.png",
           "C:/Users/Loai/Desktop/",
           launchCount,
           snapCounter);
    }
 void Update(){


            foreach(KeyCode vKey in System.Enum.GetValues(typeof(KeyCode))){
             if(Input.GetKey(vKey)){
                 setcurrentKey(vKey+"");

             }
         }
 }

//---------------------------------------------------------------------------------------------------------------------------

	 void Save(int count,int session,String key){

        // Creating First row of titles manually..

        rowData = new List<string[]>();
        string[] rowDataTemp = new string[2];

        if( launchCount == 0 && counting == 0){
                    Debug.Log(counting);
                    rowDataTemp[0] = "ID";
                    rowDataTemp[1] = "Session";
                    rowData.Add(rowDataTemp);
                    counting++;
        }
        
        // You can add up the values in as many cells as you want.
            rowDataTemp = new string[3];
            rowDataTemp[0] = snapCounter+""; // ID
            rowDataTemp[1] = session+""; // ID
            rowDataTemp[2] = key;
            rowData.Add(rowDataTemp);

        string[][] output = new string[rowData.Count][];

        for(int i = 0; i < output.Length; i++){
            output[i] = rowData[i];
        }
        Debug.Log(output);
        int     length         = output.GetLength(0);
        string  delimiter     = ",";

        StringBuilder sb = new StringBuilder();
        
        for (int index = 0; index < length; index++)
            sb.AppendLine(string.Join(delimiter, output[index]));
        
        
        string filePath = Application.dataPath +"/CSV/"+"Saved_data.csv";

        StreamWriter outStream = System.IO.File.AppendText(filePath);
        outStream.WriteLine(sb);
        outStream.Close();
    }
}