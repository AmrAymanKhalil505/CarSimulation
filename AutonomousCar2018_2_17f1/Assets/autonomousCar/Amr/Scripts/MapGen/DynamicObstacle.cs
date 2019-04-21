using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//TODO documentation 
public class DynamicObstacle : MonoBehaviour {
	private MapGen MG;

	public GameObject dynamicObs;

      Vector3 quat;
     Vector3 spawnPlace;
     Quaternion rot;
	public void initObstacles(){   
        MG=GetComponent<MapGen>();
		ArrayList GeneratedMap = MG.GeneratedMap;
        //Debug.Log(GeneratedMap.Count);
       for(int i=0;i<GeneratedMap.Count;i++){

    	RoadSTObject tempTileset = (RoadSTObject)GeneratedMap[i];
        
        Vector3 pos = Quaternion.Euler(tempTileset.CurRotation) *  new Vector3(-20,0,-20);
         spawnPlace = tempTileset.RoadObj.transform.position + pos;
         quat = new Vector3(-90,180,90);
         rot = Quaternion.Euler(tempTileset.CurRotation + quat);
      //  GameObject obstacle = Instantiate( dynamicObs  , spawnPlace ,  Quaternion.Euler(tempTileset.CurRotation + quat) ) as GameObject;
         initiateObs();

       }
       }

public void initiateObs(){

    GameObject obstacle = Instantiate( dynamicObs  , spawnPlace ,  rot ) as GameObject;
  
}

}