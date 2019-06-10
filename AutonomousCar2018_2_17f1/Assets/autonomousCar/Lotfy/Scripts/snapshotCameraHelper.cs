// Names: Mohamed Lotfy
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using UnityEngine.UI;
using System.Linq;
using System.Text;


/* Functions : Update
Date Edited : 26/5/2019
*/


public class snapshotCameraHelper : MonoBehaviour {
    /* variables */
	public snapshotCamera snapCam; /* a reference to the snapshot camera used to capture the snapshots during recording and autonomous mode */
    int framesToSkip = 5;          /* the number of frames to skip during the autonomous mode to reduce the lag */
    /* function */
    /* executed once per frame, here we have two modes the recording mode and the self driving mode.
    During the recording mode snapshots will be taken and saved in the specified segment of the dataset whenever the key 'Q' is 
    pressed while driving the vehicle. Recording will end automatically whenever the maximum number of snapshots specified by the teacher is taken.
    */
	void Update () {    
        if(snapCam.recordingMode && (snapCam.snapshotsPerSession>0)){
            foreach(KeyCode keyPressed in System.Enum.GetValues(typeof(KeyCode))){
                if(Input.GetKey(keyPressed)){
                    string keyValue=keyPressed + "";
                    if(keyValue.CompareTo("Q") == 0){
                        snapCam.takeSnapshot();
                        Debug.Log("Snapshot taken");
                        /* a reminder for the teacher to keep track how many snapshots should be taken during this recording session */
                        Debug.Log(--(snapCam.snapshotsPerSession)+" Snapshots Remaining for this session, Keep driving");
                    }
                }
            }
        }
        else{
            if(snapCam.autonomousMode){
                if(--framesToSkip <= 5){
                snapCam.takeSnapshot();
                framesToSkip = 5;
                Debug.Log("Snapshot taken");
            }
            }
        } 
    }
}
/* END OF FILE */