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
    private List<string[]> rowData = new List<string[]>();

    private void Start()
    {
        snapCounter = 0;
    }

    private void Awake()
    {
        Snapcam = GetComponent<Camera>();
        if (Snapcam.targetTexture == null)
        {
            Snapcam.targetTexture = new RenderTexture(resWidth, resHight, 24);
        }
    }
    // Update is called once per frame

    void LateUpdate()
    {
        Texture2D snapshot = new Texture2D(resWidth, resHight, TextureFormat.RGB24, false);
        Snapcam.Render();
        RenderTexture.active = Snapcam.targetTexture;
        snapshot.ReadPixels(new Rect(0, 0, resWidth, resHight), 0, 0);
        byte[] bytes = snapshot.EncodeToPNG();
        string fileName = SnapshotName();
        snapCounter++;
        System.IO.File.WriteAllBytes(fileName, bytes);
		Save(snapCounter,2);
        Debug.Log("Snapshot taken !");

    }

    string SnapshotName()
    {
        return string.Format("{0}/Snapshots/Session_{1}.png",
            Application.dataPath,
           snapCounter);
        // System.DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss")
    }

//---------------------------------------------------------------------------------------------------------------------------

	 void Save(int count,int session){

        // Creating First row of titles manually..
        string[] rowDataTemp = new string[2];
        rowDataTemp[0] = "ID";
        rowDataTemp[1] = "Session";
        rowData.Add(rowDataTemp);

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
        string     delimiter     = ",";

        StringBuilder sb = new StringBuilder();
        
        for (int index = 0; index < length; index++)
            sb.AppendLine(string.Join(delimiter, output[index]));
        
        
        string filePath = Application.dataPath +"/CSV/"+"Saved_data.csv";

        StreamWriter outStream = System.IO.File.AppendText(filePath);
        outStream.WriteLine(sb);
        outStream.Close();
    }
}
