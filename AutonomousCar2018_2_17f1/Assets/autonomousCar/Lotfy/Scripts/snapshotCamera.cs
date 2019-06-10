// Names: Mohamed Lotfy
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using System.Text;


/* Functions : setcurrentKey, Awake, LateUpdate, takeSnapshot, snapShotNameAut, snapshotNameRec
Date Edited : 26/5/2019
*/


[RequireComponent(typeof(Camera))]
public class snapshotCamera : MonoBehaviour {
    /* variables */
    public Boolean rearCamera;       /* flag to differentiate between the front and the rear cameras */
    public Boolean autonomousMode;   /* flag to activate the autonomus driving mode */
    public Boolean recordingMode;    /* flag to activate the recording mode */
    public Boolean logInCSV;         /* flag to activate the logging of the actions taken for every snapshot into the CSV file */
    public string agentID;           /* the id of this car instance as there may be multiple cars on track */
    public String sessionID;         /* the id of the recording session*/
    public int snapshotsPerSession;  /* the number of desired snapshots the teacher would like to capture for this session*/
    public String datasetSector;     /* the sector of the dataset which the recorded snapshots should flow into (train, test, validate) */
    public String currentTakenAction;/* the current action taken by the teacher while driving the car in the recording mode*/
    public String datasetPath;       /* the path where the dataset should reside*/
    int resWidth = 128;              /* resolution width of the taken snapshot */
    int resHeight = 128;             /* resolution height of the taken snapshot */
    long numericId = -1;             /* the ID number of the taken snapshot */
    int framesToSkip = 0;            /* number of frames to drop while recording */
    String snapshotPath;             /* the path where the current taken snapshot should be saved in */
    String csvFilePath;              /* the path for the CSV file in the dataset */
    String idTag;                    /* the numeric ID of the snapshot */
    String imageName;                /* the full ID of the snapshot */
    Camera snapCam;                  /* refrence for the camera used in capturing the snapshots */
    private String currentKey;       /* the current key pressed by the teacher in the recording mode */ 
    StringBuilder csvContent = new StringBuilder(); /* the string builder used to write into the CSV file */
    /* function */
    /* used to initialize the current key variable to hold the value of the current key pressed 
       so that it can be logged into the CSV file for every snapshot taken*/
    public void setcurrentKey(String upComingValue){
        currentKey = upComingValue;
    }
    /* function */
    /* awake function runs befor the scene starts and it is used to initialize the variables of the 
       camera and the clear shared memory directory for every time the scene runs */
    private void Awake(){
        snapCam = GetComponent<Camera>(); /* initialize the camera */
        if (snapCam.targetTexture == null){
            snapCam.targetTexture = new RenderTexture(resWidth, resHeight, 24); /* initialize the render texture which will be used to cpature the snapshots */
        }
        if(recordingMode){
            snapCam.gameObject.SetActive(false);
        }
        else{
            snapCam.gameObject.SetActive(true);
        }
        /* clear the shared memory */
        string root = datasetPath+"/sharedMemory_agent#"+agentID+"/";         
        if (Directory.Exists(root)){  
            Directory.Delete(root,true);  
            Debug.Log("shared memory is cleared");
        }
    }
    /* function */
    /* executed once per frame after the update function. this function is responsible for capturing the snapshots during the recording and autonomus modes.
    */
    void LateUpdate(){
        string snapshotName;
        Texture2D snapShot = new Texture2D(resWidth, resHeight, TextureFormat.RGB24, false);
        snapCam.Render();
        RenderTexture.active = snapCam.targetTexture;
        snapShot.ReadPixels(new Rect(0,0,resWidth,resHeight),0,0);
        byte[] bytes = snapShot.EncodeToJPG();
        if(recordingMode){
            if(snapCam.gameObject.activeInHierarchy){
                snapshotName=snapshotNameRec();
                System.IO.File.WriteAllBytes(snapshotName,bytes);
            }
        }
        else{
                if(autonomousMode && (--framesToSkip <= 0)){
                    snapshotName = snapShotNameAut();
                    framesToSkip = 2;
                    System.IO.File.WriteAllBytes(snapshotName,bytes);
                }
        }
        snapCam.gameObject.SetActive(false);
    }
    /* function */
    /* activates the camera object to take a snapshot */
    public void takeSnapshot(){
        snapCam.gameObject.SetActive(true);
    }
    /* function */
    /* name the snapshot during the autonomus driving mode and save it in the shared directory. by default the camera is a front caera unless the rear
       camera flag is set then the taken snashots should flow into the sector of the rear camera in the shared memory */
    string snapShotNameAut(){
        snapshotPath = datasetPath+"/sharedMemory_agent#"+agentID+"/";
        if(rearCamera){
            snapshotPath = datasetPath+"/sharedMemory_agent#"+agentID+"/Rear/";
        }
        String imageName = (++numericId).ToString();
        System.IO.Directory.CreateDirectory(snapshotPath);
        return string.Format(snapshotPath+imageName+ ".png",Application.dataPath);
    }
    /* function */
    /* name the snapshot during the recording mode and save it in the correct segment of the dataset. by default the camera is a front caera unless the rear
       camera flag is set then the taken snashots should flow into the sector of the rear camera in the shared memory.
       this function is also responsible for logging the actions for every snapshot into the CSV file if the logInCSV flag is checked. */
    string snapshotNameRec(){
        snapshotPath = datasetPath+"/FinalDataset/"+datasetSector+"/"+currentTakenAction;
        if(rearCamera){
            snapshotPath = datasetPath+"/FinalDataset/Rear/"+datasetSector+"/"+currentTakenAction;
        }
        if(logInCSV){
            csvFilePath = datasetPath+"/CSV_Data/"+"CSVFile.csv";
        }
        System.IO.Directory.CreateDirectory(datasetPath);
        System.IO.Directory.CreateDirectory(snapshotPath);
        numericId = numericId + 1;
        idTag = numericId.ToString();
        imageName = "Session_"+sessionID+"_" + idTag ;
        if(logInCSV){
            csvContent.AppendLine(imageName +","+ currentKey);
            File.AppendAllText(csvFilePath, csvContent.ToString());
            csvContent = new StringBuilder();
        }
        return string.Format(snapshotPath+"/"+imageName+ ".png",Application.dataPath);   
    }
}
/* END OF FILE */