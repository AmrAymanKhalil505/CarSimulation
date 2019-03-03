using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using System.IO;
using System;

[RequireComponent(typeof(Camera))]
public class SnapCode : MonoBehaviour
{

    Camera Snapcam;
    public int resWidth = 1080;
    public int resHight = 1080;
    int snapCounter;
    int frameCounter = 0;
    public int snapFrameCounter = 50;
    int launchCount;
    private List<string[]> rowData = new List<string[]>();
    int counting;
    private void Start()
    {
        snapCounter = 0;
        counting=0;
    }

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
		Save(snapCounter,launchCount);
        Debug.Log("Snapshot taken !");
        }
        else{
            frameCounter++;
        }

    }

    string SnapshotName()
    {
   return string.Format("{0}/Snapshots/Session_{1}_{2}.png",
            Application.dataPath,
           launchCount,
           snapCounter);
    }

//---------------------------------------------------------------------------------------------------------------------------

	 void Save(int count,int session){

        // Creating First row of titles manually..

        rowData = new List<string[]>();
        string[] rowDataTemp = new string[2];

        if(counting == 0){
        Debug.Log(counting);
        rowDataTemp[0] = "ID";
        rowDataTemp[1] = "Session";
        rowData.Add(rowDataTemp);
        counting++;
        }
        
        // You can add up the values in as many cells as you want.
            rowDataTemp = new string[2];
            rowDataTemp[0] = snapCounter+""; // ID
            rowDataTemp[1] = session+""; // ID
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
