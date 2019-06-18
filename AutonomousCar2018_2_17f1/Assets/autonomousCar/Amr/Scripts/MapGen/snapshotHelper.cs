using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class snapshotHelper : MonoBehaviour {

    public obstacleCamera obsCam1;
    public obstacleCamera obsCam2;

	void Update () 
    {    
            foreach(KeyCode vKey in System.Enum.GetValues(typeof(KeyCode))){
             if(Input.GetKey(vKey)){
                 string keyName=vKey+"";
                 if(keyName.CompareTo("UpArrow")==0 || keyName.CompareTo("DownArrow")==0 || keyName.CompareTo("LeftArrow")==0 || keyName.CompareTo("RightArrow")==0)
                 {
                    obsCam1.setcurrentKey(vKey+"");
                    obsCam1.takeSnapshot();

                    obsCam2.setcurrentKey(vKey+"");
                    obsCam2.takeSnapshot();
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