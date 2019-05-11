using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using UnityEngine.UI;
using System.Linq;
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

    public TextAsset graphModel;							// The trained TensorFlow graph
	public String firstNodeName;
	public String lastNodeName;

	private static int img_width = 128;	
	private static int img_height = 128;
	private float[,,,] inputImg = new float[1,img_width,img_height,3]; 

	private int wait=30;

    void start()
    {
        frontSnapCam.setSessionID(sessionID);
        rearSnapCam.setSessionID(sessionID);   	
    }

	void Update () 
    {    
            // foreach(KeyCode vKey in System.Enum.GetValues(typeof(KeyCode))){
            //  if(Input.GetKey(vKey)){
            //      string keyName=vKey+"";
            //      //if(keyName.CompareTo("UpArrow")==0 ||keyName.CompareTo("LeftArrow")==0 || keyName.CompareTo("RightArrow")==0)
            //      if(keyName.CompareTo("Q")==0)
            //      {
                    frontSnapCam.setcurrentKey("");
                    frontSnapCam.takeSnapshot();

					int temp=0;
					if(wait--<0){
                    int returnedActionCode=evaluateCurrentView(frontSnapCam.getCurrentImage(),temp--);
					m_Car = GetComponent<CarController>();
                    controlAgent(returnedActionCode);}

                    //rearSnapCam.setcurrentKey(vKey+"");
                    //rearSnapCam.takeSnapshot();
                 //} 
            // }
        // }
    }

	String showArray(float[,,,] inputImg,int width,int height){
		String s ="[";
		for(int i=0;i<width;i++){
			s=s+"[";
			for(int j=0;j<height;j++){
				s=s+"[";
				for(int k=0;k<3;k++){
					if(k<2){
						s=s+inputImg[0,i,j,k]+",";
					}
					else{
						s=s+inputImg[0,i,j,k];
					}
					
				}
				if(j<height-1){
					s=s+"],";
				}
				else{
					s=s+"]";
				}
				
			}
			if(i<width-1){
				s=s+"],";
			}
			else{
				s=s+"]";
			}
			
		}
		s=s+"]";
		return s;
	}

    int evaluateCurrentView (Texture2D input,int temp) {
		
		// Get raw pixel values from texture, format for inputImg array
		for (int i = 0; i < img_width; i++) {
			for (int j = 0; j < img_height; j++) {
				inputImg [0,img_width - i - 1, j, 0] = input.GetPixel(j, i).r;
				inputImg [0,img_width - i - 1, j, 1] = input.GetPixel(j, i).g;
				inputImg [0,img_width - i - 1, j, 2] = input.GetPixel(j, i).b;
			}
		}
		
		// if(temp==0){
		// String s=showArray(inputImg,128,128);
		// StreamWriter sw = new StreamWriter("/Users/MohamedAshraf/Desktop/test.txt");
		// sw.WriteLine(s);
		// sw.Close();
		// Debug.Log("************Done");
		// }
		

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

		// Find the answer the model is most confident in
		float highest_val = 0;
		int highest_ind = -1;
		float sum = 0;
		float currTime = Time.time;

		for (int j = 0; j < 3; j++) {// this would be 3
			float confidence = recurrent_tensor [0, j];
			// Debug.Log("j: "+j+" confidence: "+confidence);
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