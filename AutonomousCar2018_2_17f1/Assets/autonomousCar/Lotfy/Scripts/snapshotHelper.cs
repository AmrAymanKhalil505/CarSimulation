using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using UnityEngine.UI;

using System.Text;

using TensorFlow;

namespace UnityStandardAssets.Vehicles.Car
{
[RequireComponent(typeof (CarController))]
public class snapshotHelper : MonoBehaviour {

    public snapshotCamera frontSnapCam;
    public snapshotCamera rearSnapCam;

    public String sessionID;
    private CarController m_Car; // the car controller we want to use
	// Update is called once per frame

    public TextAsset graphModel;							// The trained TensorFlow graph

	public String firstNodeName;
	public String lastNodeName;
									// Label of classification

	private static int img_width = 128;						// Image width
	private static int img_height = 128;						// Image height
	private float[,,,] inputImg = new float[1,img_width,img_height,3]; 

	private int wait=30;

    void start()
    {
		Debug.Log("*************");
		Debug.Log("control: "+m_Car);
        frontSnapCam.setSessionID(sessionID);
        rearSnapCam.setSessionID(sessionID);
        m_Car = GetComponent<CarController>();	
    }

	void Update () 
    {    
            // foreach(KeyCode vKey in System.Enum.GetValues(typeof(KeyCode))){
            //  if(Input.GetKey(vKey)){
            //      string keyName=vKey+"";
            //      //if(keyName.CompareTo("UpArrow")==0 ||keyName.CompareTo("LeftArrow")==0 || keyName.CompareTo("RightArrow")==0)
            //      if(keyName.CompareTo("Q")==0)
            //      {
                    Debug.Log("snapshotTaken");
                    frontSnapCam.setcurrentKey("");
                    frontSnapCam.takeSnapshot();

					if(wait--<0){
                    int returnedActionCode=evaluateCurrentView(frontSnapCam.getCurrentImage());
                    controlAgent(returnedActionCode);}

                    //rearSnapCam.setcurrentKey(vKey+"");
                    //rearSnapCam.takeSnapshot();
                 //} 
            // }
        // }
    }

    int evaluateCurrentView (Texture2D input) {
		
		// Get raw pixel values from texture, format for inputImg array
		for (int i = 0; i < img_width; i++) {
			for (int j = 0; j < img_height; j++) {
				inputImg [0, img_width - i - 1, j, 0] = input.GetPixel(j, i).r;
			}
		}
		Debug.Log("ck1");

		// Debug.Log("width: "+input.height);
		// Debug.Log("height: "+input.width);
		// byte[] bytes = input.EncodeToJPG();
		// System.IO.File.WriteAllBytes("/Users/MohamedAshraf/Desktop/test.jpg",bytes);


		// Apply texture to displayMaterial
		// displayMaterial.mainTexture = input;

		// Create the TensorFlow model

		var graph = new TFGraph();
		graph.Import (graphModel.bytes);
		var session = new TFSession (graph);
		var runner = session.GetRunner ();
		
	
		// Set up the input tensor and input
		runner.AddInput (graph [firstNodeName] [0], inputImg);
		
		
		// Set up the output tensor
		runner.Fetch (graph [lastNodeName] [0]);

		// Run the model
		float[,] recurrent_tensor = runner.Run () [0].GetValue () as float[,];
		Debug.Log("ck");

		// Find the answer the model is most confident in
		float highest_val = 0;
		int highest_ind = -1;
		float sum = 0;
		float currTime = Time.time;

		for (int j = 0; j < 3; j++) {// this would be 3
			float confidence = recurrent_tensor [0, j];
			if (highest_ind > -1) {
				if (recurrent_tensor [0, j] > highest_val) {
					highest_val = confidence;
					highest_ind = j;
				}
			} else {
				highest_val = confidence;
				highest_ind = j;
			}

			// sum should total 1 in the end
			sum += confidence;
		}
		// String[] arrayOfActionNames = {"left","right","straight"};

		// Display the answer to the screen
		// label.text = "Answer: " + highest_ind + "\n Confidence: " + highest_val +
		// 	"\nLatency: " + (Time.time - currTime) * 1000000 + " us";

		// label.text = "Answer: " + arrayOfActions[highest_ind] + "\n Confidence: " + highest_val +
		
		// 	"\nLatency: " + (Time.time - currTime) * 1000000 + " us";
        Debug.Log("done");
		return highest_ind;
		
	}

    void controlAgent(int actionCode){
        switch (actionCode) {	
				case 2:
						m_Car.Move(0f, 0.8f, 0.8f, 0f);	
						Debug.Log("foreward");
					break;
				case 1:
						m_Car.Move(0.8f,0.4f, 0.4f, 0f);
						Debug.Log("right");
					break;
				case 0:
						m_Car.Move(-0.8f, 0.4f,0.4f, 0f);
						Debug.Log("left");
					break;
				case 3:
					m_Car.Move(0f, 0.0f, 0.0f, 0f);
					break;
      		}
    }
}
}


// the only 4 keys to listen to 
// up -->  UpArrow
// Down -->  DownArrow
// Right -->  RightArrow
// Left -->  LeftArrow