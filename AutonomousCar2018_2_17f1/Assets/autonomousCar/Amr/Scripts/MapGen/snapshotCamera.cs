using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

using System.Text;

[RequireComponent(typeof(Camera))]

public class snapshotCamera : MonoBehaviour {

    public Boolean recording;
    Camera snapCam;
    StringBuilder csvContent = new StringBuilder();

    private String datasetParentPath="C:/Users/Loai/Desktop";
    public String sessionID;

    public int isObstacle;
    private String currentKey;

    int resWidth = 1080;
    int resHeight = 1080;
    int frameCounter=0;
    long numericId = -1; // Note: This number has a maximum of "9,223,372,036,854,775,807"

    private byte[] currentImage; 

    public byte[] getCurrentImage()
    {
        return currentImage;
    }

    public void setRecording(bool flag){
        recording=flag;
    }
    private void Awake()
    {
        // The snapshot camera part

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
                string filename = snapShotName();
                System.IO.File.WriteAllBytes(filename,bytes);
                snapCam.gameObject.SetActive(false);
            }
        }
        else
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
                // Debug.Log("SnapshotTaken");
            }
        }
    }

    public void setcurrentKey(String comingValue)
    {
        currentKey=comingValue;
    }

    string snapShotNameSelfDriving(){
        if(isObstacle==1){
        String sessionPath=datasetParentPath+"/sharedMemory/obstacles";
        String imageName=(++numericId).ToString();
        System.IO.Directory.CreateDirectory(sessionPath);
        return string.Format(sessionPath+imageName+ ".png",Application.dataPath);
        }
        else{
        String sessionPath=datasetParentPath+"/sharedMemory/scene";
        String imageName=(++numericId).ToString();
        System.IO.Directory.CreateDirectory(sessionPath);
        return string.Format(sessionPath+imageName+ ".png",Application.dataPath);


        }
    }

    string snapShotName() // give an id and a date to every snapshot and add this data to the CSV file
    {

        //      /Users/MohamedAshraf/Desktop this is my cureent local path
        String sessionPath;
        String csvFilePath;
        String currentFolder="";
        switch(currentKey){

        case("UpArrow"): currentFolder = "up";break;
        case("RightArrow"): currentFolder = "right";break;
        case("LeftArrow"): currentFolder = "left";break;
        case("DownArrow"): currentFolder = "brake";break;
        default:break;


        }
       if(isObstacle==0){

       sessionPath=datasetParentPath+"/Snapshots3/"+currentFolder+"/";
       csvFilePath=datasetParentPath+"/CSV_Data3/"+"CSVFile.csv";


       }else{

        sessionPath=datasetParentPath+"/ObstacleSnapshots3/"+currentFolder+"/";
        csvFilePath=datasetParentPath+"/CSV_Data3/"+"CSVFileObstacles.csv";


       }



        System.IO.Directory.CreateDirectory(datasetParentPath);
        System.IO.Directory.CreateDirectory(sessionPath);

        numericId = numericId + 1;
        String idTag = numericId.ToString();
        String imageName="Session_"+sessionID+"_" + idTag ;

        Debug.Log(currentKey+"");

         csvContent.AppendLine(imageName +","+ currentKey);
         File.AppendAllText(csvFilePath, csvContent.ToString());
         csvContent= new StringBuilder(); // clearng the string builder


        return string.Format(sessionPath+imageName+ ".png",Application.dataPath);
         
    }
}
