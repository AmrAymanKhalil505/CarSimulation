using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[RequireComponent(typeof(Camera))]

public class snapshotCamera : MonoBehaviour {

    Camera snapCam;

    int resWidth = 1080;
    int resHeight = 1080;
    long numericId = -1;

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
            Debug.Log("snapshot taken");
            snapCam.gameObject.SetActive(false);
        }
    }

    string snapShotName()
    {
        numericId = numericId + 1;
        String idTag = numericId.ToString();
        return string.Format
        (
            "{0}/Snapshots/["+ idTag+"] "+ System.DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss") + ".png",
            Application.dataPath,
            resWidth,
            resHeight
            );


    }
}
