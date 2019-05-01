using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class snapshotHelper : MonoBehaviour {

    public snapshotCamera frontSnapCam;
    public snapshotCamera rearSnapCam;
	// Update is called once per frame

	void Update () 
    {    
            foreach(KeyCode vKey in System.Enum.GetValues(typeof(KeyCode))){
             if(Input.GetKey(vKey)){
                 string keyName=vKey+"";
                 //if(keyName.CompareTo("UpArrow")==0 ||keyName.CompareTo("LeftArrow")==0 || keyName.CompareTo("RightArrow")==0)
                 if(keyName.CompareTo("Q")==0)
                 {
                    Debug.Log("snapshotTaken");
                    frontSnapCam.setcurrentKey(vKey+"");
                    frontSnapCam.takeSnapshot();

                    rearSnapCam.setcurrentKey(vKey+"");
                    rearSnapCam.takeSnapshot();
                 } 
             }
         }
    }
}


// the only 4 keys to listen to 
// up -->  UpArrow
// Down -->  DownArrow
// Right -->  RightArrow
// Left -->  LeftArrow