using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class snapshotHelper : MonoBehaviour {

    public snapshotCamera snapCam;
	// Update is called once per frame
	void Update () 
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            snapCam.takeSnapshot();
        }
    }
}
