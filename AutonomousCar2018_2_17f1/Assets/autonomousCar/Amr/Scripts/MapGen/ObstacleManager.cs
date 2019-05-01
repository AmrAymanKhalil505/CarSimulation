using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//TODO documentation 
public class ObstacleManager : MonoBehaviour {
	private MapGen MG;
  private int coneCount = 0;
  private float coneX=0.0f;
  private float coneZ=0.0f;  
  private int whichLane=0;
	public void initObstacles( bool staticObs , bool dynamicObs){   
        MG=GetComponent<MapGen>();
		ArrayList GeneratedMap = MG.GeneratedMap;
        //Debug.Log(GeneratedMap.Count);
       for(int i=0;i<GeneratedMap.Count;i++){
      
        //Debug.Log(i);
    	RoadSTObject tempTileset = (RoadSTObject)GeneratedMap[i];
      //Debug.Log(tempTileset.CurRotation);
     // Vector3 spawnPlace = Vector3.Lerp ( , , Random.Range (0, 1)) +offestToNodesSpwan;
       //  Debug.Log(tempTileset.RoadObj.transform.position);
	    //Debug.Log( tempTileset.pointsX[0].x);
      //  if(i<tempTileset.obstacles.Length){
	    
        //Debug.Log(tempTileset.obstacles[0].tag);
        for(int j = 0 ; j < tempTileset.obstacles.Length; j++){
          // tempTileset.RoadObj.transform.position + 
          float x = Random.Range( tempTileset.pointsX[j].x , tempTileset.pointsY[j].x );
          float z = Random.Range( tempTileset.pointsX[j].z ,  tempTileset.pointsY[j].z );
          // Debug.Log(x);
          // Debug.Log(z);
          // Debug.Log(tempTileset.obstacles[j].tag );
          if(staticObs){

       
          if(tempTileset.obstacles[j].tag == "bump"){
           while(z<5 && z>-5){

           z = Random.Range( tempTileset.pointsX[j].z , 
               tempTileset.pointsY[j].z );
           

           }
           Vector3 pos = new Vector3(x,5.9f, z);
           pos = Quaternion.Euler(tempTileset.CurRotation) * pos;
           Vector3 spawnPlace = tempTileset.RoadObj.transform.position + pos;
           Vector3 quat = new Vector3(-90,180,90);
           GameObject obstacle = Instantiate( tempTileset.obstacles[j] , spawnPlace , Quaternion.Euler(tempTileset.CurRotation + quat) ) as GameObject;
         
          }else if(tempTileset.obstacles[j].tag == "StreetHole"){
           Vector3 pos = new Vector3(x,6.18f, z);
           pos = Quaternion.Euler(tempTileset.CurRotation) * pos;
           Vector3 spawnPlace = tempTileset.RoadObj.transform.position + pos;
           Vector3 quat = new Vector3(-90,180,90);
           GameObject obstacle = Instantiate( tempTileset.obstacles[j]  , spawnPlace , Quaternion.Euler(tempTileset.CurRotation + quat)) as GameObject;
          }else if(tempTileset.obstacles[j].tag == "cone" ){
           
           while(z<5 &&  z>-5 || (z == coneZ)){
            z = Random.Range( tempTileset.pointsX[j].z , 
                tempTileset.pointsY[j].z );
             }
          if(coneCount == 0){
           whichLane = Random.Range( 0 , 2 );
           Debug.Log("coneZ:"+coneZ);
           Debug.Log("z:"+z);
           coneCount++;
           coneX = x;
           coneZ = z;
           Vector3 pos = new Vector3(x,6.113f, z);
           pos = Quaternion.Euler(tempTileset.CurRotation) * pos;
           Vector3 spawnPlace = tempTileset.RoadObj.transform.position + pos;
           Vector3 quat = new Vector3(-90,180,90);
           GameObject obstacle = Instantiate( tempTileset.obstacles[j]  , spawnPlace ,  Quaternion.Euler(tempTileset.CurRotation + quat) ) as GameObject;
           }
           else{
           coneCount++;
           if(whichLane==0){
               coneX+=2;
           }else{
               coneX-=2;
           }
           Vector3 pos = new Vector3(coneX,6.113f, coneZ);
           pos = Quaternion.Euler(tempTileset.CurRotation) * pos;
           Vector3 spawnPlace = tempTileset.RoadObj.transform.position + pos;
           Vector3 quat = new Vector3(-90,180,90);
           GameObject obstacle = Instantiate( tempTileset.obstacles[j]  , spawnPlace ,  Quaternion.Euler(tempTileset.CurRotation + quat) ) as GameObject;
           coneCount = (coneCount==4)?0:coneCount;
          }



          }else if(tempTileset.obstacles[j].tag == "sign"){

           Vector3 pos = new Vector3(x,8.19f, z);
           pos = Quaternion.Euler(tempTileset.CurRotation) * pos;
           Vector3 spawnPlace = tempTileset.RoadObj.transform.position + pos;
           Vector3 quat = new Vector3(-90,180,90);
           GameObject obstacle = Instantiate( tempTileset.obstacles[j]  , spawnPlace ,  Quaternion.Euler(tempTileset.CurRotation + quat) ) as GameObject;
  



          }else if(tempTileset.obstacles[j].tag == "man"){
            // 1- offest by road pos 
            // 2- debug sh
            // 3- Quaternion( (x,y,z)+ (x,y,z)) * point + pos (in case we need to rotate 90 or smth)
          //
          if( dynamicObs ){
          Vector3 pos = new Vector3(x,5.86f,z);
          pos = Quaternion.Euler(tempTileset.CurRotation) * pos;
           Vector3 spawnPlace = tempTileset.RoadObj.transform.position + pos;
           Vector3 quat = new Vector3(0,180,0);
           GameObject obstacle = Instantiate( tempTileset.obstacles[j]  , spawnPlace , Quaternion.Euler(tempTileset.CurRotation + quat)) as GameObject;
            quat = new Vector3(0,-spawnPlace.y,30);
            quat = Quaternion.Euler(tempTileset.CurRotation) * quat;
            quat = tempTileset.RoadObj.transform.position - quat;
          //  quat =  Quaternion.Euler(tempTileset.CurRotation) * quat;
          // // Vector3 quat2 = new Vector3(90,0,90);
             obstacle.GetComponent<CharacterMoving>().dest = quat;
            //  Debug.Log("destination" + tempTileset.RoadObj.transform.position  );
            //  Debug.Log("destination2" +     obstacle.GetComponent<CharacterMoving>().dest );
          }
          }else {
           Vector3 pos = new Vector3(x,5.86f, z);
           pos = Quaternion.Euler(tempTileset.CurRotation) * pos;
           Vector3 spawnPlace = tempTileset.RoadObj.transform.position + pos;
           Vector3 quat = new Vector3(0,0,0);
           GameObject obstacle = Instantiate( tempTileset.obstacles[j]  , spawnPlace , Quaternion.Euler(tempTileset.CurRotation + quat)) as GameObject;

          }

         
        }else if(dynamicObs){
          if(tempTileset.obstacles[j].tag == "man"){
          Vector3 pos = new Vector3(x,5.86f,z);
          pos = Quaternion.Euler(tempTileset.CurRotation) * pos;
           Vector3 spawnPlace = tempTileset.RoadObj.transform.position + pos;
           Vector3 quat = new Vector3(0,180,0);
           GameObject obstacle = Instantiate( tempTileset.obstacles[j]  , spawnPlace , Quaternion.Euler(tempTileset.CurRotation + quat)) as GameObject;
            quat = new Vector3(x,-spawnPlace.y,z);
            quat = Quaternion.Euler(tempTileset.CurRotation) * quat;
            quat = tempTileset.RoadObj.transform.position - quat;
          //  quat =  Quaternion.Euler(tempTileset.CurRotation) * quat;
          // // Vector3 quat2 = new Vector3(90,0,90);
             obstacle.GetComponent<CharacterMoving>().dest = quat;
            //  Debug.Log("destination" + tempTileset.RoadObj.transform.position  );
            //  Debug.Log("destination2" +     obstacle.GetComponent<CharacterMoving>().dest );

          }
        }

           }
        //if(tempTileset.obstacles[j].tag == "Pedestrian")
	      //GameObject obstacle = Instantiate( tempTileset.obstacles[0]  , spawnPlace , Quaternion.Euler(0,180,90)) as GameObject;
        // }
        //tempTileset.RoadObj.transform.position

       }
           
      //Debug.Log("hey");
    }


}