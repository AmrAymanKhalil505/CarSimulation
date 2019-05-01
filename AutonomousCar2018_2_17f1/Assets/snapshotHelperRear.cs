using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class snapshotHelperRear : MonoBehaviour {

    public snapshotCamera snapCam;
	// Update is called once per frame

	void Update () 
    {    
            foreach(KeyCode vKey in System.Enum.GetValues(typeof(KeyCode))){
             if(Input.GetKey(vKey)){
                 string keyName=vKey+"";
                 //if(keyName.CompareTo("UpArrow")==0 ||keyName.CompareTo("LeftArrow")==0 || keyName.CompareTo("RightArrow")==0)
                 if(keyName.CompareTo("UpArrow")==0)
                 {
                    Debug.Log("snapshotTaken");
                    snapCam.setcurrentKey(vKey+"");
                    snapCam.takeSnapshot();
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

