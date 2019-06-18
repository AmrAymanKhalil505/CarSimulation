using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class snapshotHelper : MonoBehaviour {

    public snapshotCamera snapCam;
    public obstacleCamera obsCam;

	void Update () 
    {    
            foreach(KeyCode vKey in System.Enum.GetValues(typeof(KeyCode))){
             if(Input.GetKey(vKey)){
                 string keyName=vKey+"";
                 if(keyName.CompareTo("UpArrow")==0 || keyName.CompareTo("DownArrow")==0 || keyName.CompareTo("LeftArrow")==0 || keyName.CompareTo("RightArrow")==0)
                 {
                    obsCam.setcurrentKey(vKey+"");
                    obsCam.takeSnapshot();
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