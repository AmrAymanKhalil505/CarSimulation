using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        //PlayerPrefs.SetInt("TimesLaunched", 0);

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
            Debug.Log("Snapshot taken!");
            frameCounter = 0;
        }
        else
        {
            frameCounter++;
        }
    }

    string SnapshotName()
    {
        return string.Format("{0}/Snapshots/Session_{1}_{2}.png",
            Application.dataPath,
           launchCount,
           snapCounter);
        // System.DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss")
    }
}
