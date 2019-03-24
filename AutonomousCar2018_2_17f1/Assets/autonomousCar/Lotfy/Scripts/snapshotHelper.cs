using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class snapshotHelper : MonoBehaviour {

    public snapshotCamera snapCam;
	// Update is called once per frame

	void Update () 
    {    
            foreach(KeyCode vKey in System.Enum.GetValues(typeof(KeyCode))){
             if(Input.GetKey(vKey)){
                 snapCam.setcurrentKey(vKey+"");
                 snapCam.takeSnapshot();
             }
         }
    }
}
