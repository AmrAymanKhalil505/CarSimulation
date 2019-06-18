using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

using System.Text;

[RequireComponent(typeof(Camera))]

public class obstacleCamera : MonoBehaviour {

    //----USER OPTIONS----------
    public Boolean recording; // User chooses whether to record or not
    public Boolean autonomousMode; // User chooses to start the simulator in autonomous mode or not
    public Boolean CSVLogging; // User enables or disables CSV Logging
    //---------------------------

    public Camera snapCam;
    StringBuilder csvContent = new StringBuilder();

    public String datasetParentPath="Users/loai/Desktop";
    public String sessionID;

    private String currentKey; //getting current pressed key

    //snapshot size
    int resWidth = 128;
    int resHeight = 128;

    public int camNumber;

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

        String root = datasetParentPath+"/sharedMemory/";

         if (Directory.Exists(root)){  
            Directory.Delete(root,true);  
            Debug.Log("shared memory is cleared");
        }

        if(recording){
            autonomousMode = false;
        }else if(!autonomousMode){

            recording = true;

        }else{

            recording = false;

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
            if(autonomousMode)
            {
                Texture2D snapShot = new Texture2D(resWidth, resHeight, TextureFormat.RGB24, false);
                snapCam.Render();
                RenderTexture.active = snapCam.targetTexture;
                snapShot.ReadPixels(new Rect(0,0,resWidth,resHeight),0,0);
                byte[] bytes = snapShot.EncodeToJPG();
                string filename = snapShotNameSelfDriving();
                System.IO.File.WriteAllBytes(filename,bytes);
           }
        }
    }

    public void setcurrentKey(String comingValue)
    {
        currentKey=comingValue;
    }

    string snapShotNameSelfDriving(){
        String sessionPath=datasetParentPath+"/sharedMemory/";
        String imageName=(++numericId).ToString();
        System.IO.Directory.CreateDirectory(sessionPath);
        return string.Format(sessionPath+imageName+".png",Application.dataPath);

    }


    string snapShotName() // give an id and a date to every snapshot and add this data to the CSV file
    {

        String sessionPath;
        String csvFilePath;
        String currentFolder="";
        
            switch(currentKey) {  
            case("UpArrow"): currentFolder = "up";      break;
            case("RightArrow"): currentFolder = "right";break;
            case("LeftArrow"): currentFolder = "left";  break;
            case("DownArrow"): currentFolder = "brake"; break;
            default: break; 
            }

           

         sessionPath=datasetParentPath+"/ObstaclesData"+camNumber+"/"+currentFolder+"/";


        System.IO.Directory.CreateDirectory(datasetParentPath);
       
        System.IO.Directory.CreateDirectory(sessionPath);

        numericId = numericId + 1;
        String idTag = numericId.ToString();
        String imageName="Session_"+sessionID+"_" + idTag ;

    if(CSVLogging){
        csvFilePath=datasetParentPath+"/CSV_Data/"+"CSVFileObstacles.csv";

         System.IO.Directory.CreateDirectory(csvFilePath);
         csvContent.AppendLine(imageName +","+ currentKey);
         File.AppendAllText(csvFilePath, csvContent.ToString());
         csvContent= new StringBuilder(); // clearing the string builder
    }

        return string.Format(sessionPath+imageName+ ".png",Application.dataPath);
         
    }
}
