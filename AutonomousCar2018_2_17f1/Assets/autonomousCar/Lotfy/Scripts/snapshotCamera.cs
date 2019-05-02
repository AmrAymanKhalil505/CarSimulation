using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

using System.Text;

[RequireComponent(typeof(Camera))]

public class snapshotCamera : MonoBehaviour {

    public Boolean rearCamera;
    public Boolean agentIsDriving;
    public string agentID;
    Camera snapCam;
    StringBuilder csvContent = new StringBuilder();
    public Boolean recording;
    public Boolean logActionsInCSVFile;
    public String datasetSector;
    public String currentTakenAction;
    public String datasetParentPath;
    private static String sessionID;

    private String currentKey;

    private static Texture2D currentImage;

    int resWidth = 1080;
    int resHeight = 1080;
    int frameCounter=0;
    long numericId = -1; // Note: This number has a maximum of "9,223,372,036,854,775,807"

    public void setSessionID(String incomingSessionID)
    {
        sessionID=incomingSessionID;
    }

    public Texture2D getCurrentImage()
    {
        // Debug.Log("get: "+currentImage.Length);
        return currentImage;
    }
    public void setRecording(bool flag){
        recording=flag;
    }
    private void Awake()
    {

        snapCam = GetComponent<Camera>();
        if (snapCam.targetTexture == null)
        {
            snapCam.targetTexture = new RenderTexture(resWidth, resHeight, 24);
        }
        else
        {
            resWidth = snapCam.targetTexture.width;
            resWidth = snapCam.targetTexture.height;
        }
        if(recording)
        {
            snapCam.gameObject.SetActive(false);
        }
        else
        {
            snapCam.gameObject.SetActive(true);
        }
        

    }

    public void takeSnapshot()
    {
        snapCam.gameObject.SetActive(true);
    }


    void LateUpdate()
    {
        if(recording)
        {
            if(snapCam.gameObject.activeInHierarchy)
            {
                Texture2D snapShot = new Texture2D(resWidth, resHeight, TextureFormat.RGB24, false);
                snapCam.Render();
                RenderTexture.active = snapCam.targetTexture;
                snapShot.ReadPixels(new Rect(0,0,resWidth,resHeight),0,0);
                byte[] bytes = snapShot.EncodeToJPG();
                currentImage=snapShot;
                string filename = snapShotName();
                System.IO.File.WriteAllBytes(filename,bytes);
                snapCam.gameObject.SetActive(false);
            }
        }
        else
        {
            if(agentIsDriving)
            {
                if(--frameCounter<=0)
                {
                Texture2D snapShot = new Texture2D(resWidth, resHeight, TextureFormat.RGB24, false);
                snapCam.Render();
                RenderTexture.active = snapCam.targetTexture;
                snapShot.ReadPixels(new Rect(0,0,resWidth,resHeight),0,0);
                byte[] bytes = snapShot.EncodeToJPG();
                string filename = snapShotNameSelfDriving();
                System.IO.File.WriteAllBytes(filename,bytes);
                frameCounter=2;
                }
            }
        }
    }

    public void setcurrentKey(String comingValue)
    {
        currentKey=comingValue;
    }

    string snapShotNameSelfDriving(){
        String sessionPath;
        if(rearCamera)
        {
            sessionPath=datasetParentPath+"/sharedMemory_agent#"+agentID+"_rearCamera/";
        }
        else
        {
            sessionPath=datasetParentPath+"/sharedMemory_agent#"+agentID+"_frontCamera/";
        }
        String imageName=(++numericId).ToString();
        System.IO.Directory.CreateDirectory(sessionPath);
        return string.Format(sessionPath+imageName+ ".png",Application.dataPath);
    }

    string snapShotName() // give an id and a date to every snapshot and add this data to the CSV file
    {

        //   /Users/MohamedAshraf/Desktop this is my cureent local path

        String sessionPath=datasetParentPath+"/Dataset/front/"+datasetSector+"/"+currentTakenAction;

        if(rearCamera)
        {
            sessionPath=datasetParentPath+"/Dataset/rear/"+datasetSector+"/"+currentTakenAction;
        }

        String csvFilePath="";
        if(logActionsInCSVFile)
        {
            if(rearCamera)
            {
                csvFilePath=datasetParentPath+"/CSV_Data_RearCamera/"+"CSVFile.csv";
            }
            else
            {
                csvFilePath=datasetParentPath+"/CSV_Data_FrontCamera/"+"CSVFile.csv";
            }
        }
        

        System.IO.Directory.CreateDirectory(datasetParentPath);
        System.IO.Directory.CreateDirectory(sessionPath);

        numericId = numericId + 1;
        String idTag = numericId.ToString();
        String imageName="Session_"+sessionID+"_" + idTag ;


        if(logActionsInCSVFile)
        {
            csvContent.AppendLine(imageName +","+ currentKey);
            File.AppendAllText(csvFilePath, csvContent.ToString());
            csvContent= new StringBuilder(); // clearng the string builder
        }
         


        return string.Format(sessionPath+"/"+imageName+ ".png",Application.dataPath);
         
    }
}
