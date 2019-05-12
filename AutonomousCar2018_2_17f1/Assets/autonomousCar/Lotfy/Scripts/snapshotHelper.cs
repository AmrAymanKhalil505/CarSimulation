using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using UnityEngine.UI;
using System.Linq;
using System.Text;


public class snapshotHelper : MonoBehaviour {
	public snapshotCamera snapCam;
    int counter=5;
	void Update () 
    {    
        // foreach(KeyCode vKey in System.Enum.GetValues(typeof(KeyCode)))
        // {
        //     if(Input.GetKey(vKey))
        //     {
        //         string keyName=vKey+"";
        //         //if(keyName.CompareTo("UpArrow")==0 ||keyName.CompareTo("LeftArrow")==0 || keyName.CompareTo("RightArrow")==0)
        //         if(keyName.CompareTo("Q")==0)
        //         {
        //         snapCam.takeSnapshot();
        //         }
        //     }
        // }
        if(counter--<=2){
            snapCam.takeSnapshot();
            counter=5;
        }
        
    }
}
