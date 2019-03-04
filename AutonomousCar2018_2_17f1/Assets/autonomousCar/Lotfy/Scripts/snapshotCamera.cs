using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

using System.Text;

[RequireComponent(typeof(Camera))]

public class snapshotCamera : MonoBehaviour {

    Camera snapCam;
    StringBuilder csvContent = new StringBuilder();


    int resWidth = 1080;
    int resHeight = 1080;
    long numericId = -1; // Note: This number has a maximum of "9,223,372,036,854,775,807" 

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
        snapCam.gameObject.SetActive(false);

    }

    public void takeSnapshot()
    {
        snapCam.gameObject.SetActive(true);
    }

    void LateUpdate()
    {
        if(snapCam.gameObject.activeInHierarchy)
        {
            Texture2D snapShot = new Texture2D(resWidth, resHeight, TextureFormat.RGB24, false);
            snapCam.Render();
            RenderTexture.active = snapCam.targetTexture;
            snapShot.ReadPixels(new Rect(0,0,resWidth,resHeight),0,0);
            byte[] bytes = snapShot.EncodeToPNG();
            string filename = snapShotName();
            System.IO.File.WriteAllBytes(filename,bytes);
            snapCam.gameObject.SetActive(false);
        }
    }

    string snapShotName() // give an id and a date to every snapshot and add this data to the CSV file
    {
        if(numericId==-1)
        {
            csvContent.AppendLine("Id,Date");
        }
        numericId = numericId + 1;
        String idTag = numericId.ToString();
        csvContent.AppendLine(idTag +","+ System.DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss"));
        File.AppendAllText("Assets/autonomousCar/Lotfy/Snapshots/CSVFile.csv", csvContent.ToString());
        return string.Format("{0}/autonomousCar/Lotfy/Snapshots/[" + idTag+"] "+ System.DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss") + ".png",Application.dataPath);
    }
}
