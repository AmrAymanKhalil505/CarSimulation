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
	private int snapshotCounter=0;
	void Update () 
    {    
            foreach(KeyCode vKey in System.Enum.GetValues(typeof(KeyCode)))
			{
             if(Input.GetKey(vKey))
			 {
                 string keyName=vKey+"";
                 //if(keyName.CompareTo("UpArrow")==0 ||keyName.CompareTo("LeftArrow")==0 || keyName.CompareTo("RightArrow")==0)
                 if(keyName.CompareTo("Q")==0 && snapshotCounter++<1000)
                 {
					snapCam.takeSnapshot();
					Debug.Log("snapshot taken");
                 } 
				 if(snapshotCounter>=500){
					 Debug.Log("Done recording for this session");
				 }
            }
        }
    }

}
