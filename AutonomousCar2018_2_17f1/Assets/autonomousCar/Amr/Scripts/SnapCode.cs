using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class Snapshots : MonoBehaviour
{

    Camera Snapcam;
    public int resWidth = 256;
    public int resHight = 256;

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
        System.IO.File.WriteAllBytes(fileName, bytes);
        Debug.Log("Snapshot taken !");

    }

    string SnapshotName()
    {
        return string.Format("{0}/Snapshots/snap_{1}.png",
            Application.dataPath,
            System.DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss"));
    }
}
