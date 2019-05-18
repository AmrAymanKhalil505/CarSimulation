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
      
          for(int i=0;i<GeneratedMap.Count;i++){
    
         	RoadSTObject tempTileset = (RoadSTObject)GeneratedMap[i];

          tempTileset.spawnedIn = new Hashtable();
 
          for(int j = 0 ; j < tempTileset.obstacles.Length; j++){
          string positionOfObs="";
          float x = Random.Range( tempTileset.pointsX[j].x , tempTileset.pointsY[j].x );
          float z = Random.Range( tempTileset.pointsX[j].z ,  tempTileset.pointsY[j].z );
          if(staticObs){
          
          if(tempTileset.obstacles[j].tag == "bump"){
          
           while(tempTileset.spawnedIn.ContainsKey(positionOfObs)){

            x = Random.Range( tempTileset.pointsX[j].x , tempTileset.pointsY[j].x );
            z = Random.Range( tempTileset.pointsX[j].z ,  tempTileset.pointsY[j].z );
            positionOfObs=""+x+","+z;
            Debug.Log("hey hey hey");
           }
           positionOfObs="";
           while(z<5 && z>-5){

              z = Random.Range( tempTileset.pointsX[j].z , 
               tempTileset.pointsY[j].z );
           

           }
           Vector3 pos = new Vector3(x,5.9f, z);
           tempTileset.spawnedIn.Add(""+x+","+z,"bump"); 
           pos = Quaternion.Euler(tempTileset.CurRotation) * pos;
           Vector3 spawnPlace = tempTileset.RoadObj.transform.position + pos;
           Vector3 quat = new Vector3(-90,180,90);
           GameObject obstacle = Instantiate( tempTileset.obstacles[j] , spawnPlace , Quaternion.Euler(tempTileset.CurRotation + quat) ) as GameObject;
         
          }else if(tempTileset.obstacles[j].tag == "StreetHole"){
          
            while(tempTileset.spawnedIn.ContainsKey(positionOfObs)){

            x = Random.Range( tempTileset.pointsX[j].x , tempTileset.pointsY[j].x );
            z = Random.Range( tempTileset.pointsX[j].z ,  tempTileset.pointsY[j].z );
            positionOfObs=""+x+","+z;

           }
           positionOfObs="";
           Vector3 pos = new Vector3(x,6.18f, z);
           tempTileset.spawnedIn.Add(""+x+","+z,"StreetHole"); 
           pos = Quaternion.Euler(tempTileset.CurRotation) * pos;
           Vector3 spawnPlace = tempTileset.RoadObj.transform.position + pos;
           Vector3 quat = new Vector3(-90,180,90);
           GameObject obstacle = Instantiate( tempTileset.obstacles[j]  , spawnPlace , Quaternion.Euler(tempTileset.CurRotation + quat)) as GameObject;
          }else if(tempTileset.obstacles[j].tag == "cone" ){
            
            while(tempTileset.spawnedIn.ContainsKey(positionOfObs)){
            
            x = Random.Range( tempTileset.pointsX[j].x , tempTileset.pointsY[j].x );
            z = Random.Range( tempTileset.pointsX[j].z ,  tempTileset.pointsY[j].z );
            positionOfObs=""+x+","+z;
            Debug.Log("hey hey hey");
           }
           positionOfObs="";
           while(z<7 &&  z>-7 || ( Mathf.Abs(z - coneZ) < 15)){
            z = Random.Range( tempTileset.pointsX[j].z , 
                tempTileset.pointsY[j].z );

             }
          if(coneCount == 0){
           whichLane = Random.Range( 0 , 2 );
           coneCount++;
           coneX = x;
           coneZ = z;
           Vector3 pos = new Vector3(x,6.113f, z);
           tempTileset.spawnedIn.Add(""+x+","+z,"cone"); 
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
           tempTileset.spawnedIn.Add(""+coneX+","+coneZ,"cone"); 
           pos = Quaternion.Euler(tempTileset.CurRotation) * pos;
           Vector3 spawnPlace = tempTileset.RoadObj.transform.position + pos;
           Vector3 quat = new Vector3(-90,180,90);
           GameObject obstacle = Instantiate( tempTileset.obstacles[j]  , spawnPlace ,  Quaternion.Euler(tempTileset.CurRotation + quat) ) as GameObject;
           coneCount = (coneCount==3)?0:coneCount;
          }



          }else if(tempTileset.obstacles[j].tag == "sign"){
            
            while(tempTileset.spawnedIn.ContainsKey(positionOfObs)){

            x = Random.Range( tempTileset.pointsX[j].x , tempTileset.pointsY[j].x );
            z = Random.Range( tempTileset.pointsX[j].z ,  tempTileset.pointsY[j].z );
            positionOfObs=""+x+","+z;
            Debug.Log("hey hey hey");
           }
           positionOfObs="";
           Vector3 pos = new Vector3(x,8.19f, z);
           tempTileset.spawnedIn.Add(""+x+","+z,"sign"); 
           pos = Quaternion.Euler(tempTileset.CurRotation) * pos;
           Vector3 spawnPlace = tempTileset.RoadObj.transform.position + pos;
           Vector3 quat = new Vector3(-90,180,90);
           GameObject obstacle = Instantiate( tempTileset.obstacles[j]  , spawnPlace ,  Quaternion.Euler(tempTileset.CurRotation + quat) ) as GameObject;
  



          }else if(tempTileset.obstacles[j].tag == "man"){
  
          if( dynamicObs ){
            
            while(tempTileset.spawnedIn.ContainsKey(positionOfObs)){

            x = Random.Range( tempTileset.pointsX[j].x , tempTileset.pointsY[j].x );
            z = Random.Range( tempTileset.pointsX[j].z ,  tempTileset.pointsY[j].z );
            positionOfObs=""+x+","+z;

           }
           positionOfObs="";
            Vector3 pos = new Vector3(x,5.86f,z);
            tempTileset.spawnedIn.Add(""+x+","+z,"man"); 
            pos = Quaternion.Euler(tempTileset.CurRotation) * pos;
            Vector3 spawnPlace = tempTileset.RoadObj.transform.position + pos;
            Vector3 quat = new Vector3(0,180,0);
            GameObject obstacle = Instantiate( tempTileset.obstacles[j]  , spawnPlace , Quaternion.Euler(tempTileset.CurRotation + quat)) as GameObject;
            quat = new Vector3(x,-spawnPlace.y,z);
            quat = Quaternion.Euler(tempTileset.CurRotation) * quat;
            quat = tempTileset.RoadObj.transform.position - quat;
            obstacle.GetComponent<CharacterMoving>().dest = quat;
          }
          }else if(tempTileset.obstacles[j].tag == "bus" && tempTileset.RoadName == "Ahead"){
          
           while(tempTileset.spawnedIn.ContainsKey(positionOfObs)){

            x = Random.Range( tempTileset.pointsX[j].x , tempTileset.pointsY[j].x );
            z = Random.Range( tempTileset.pointsX[j].z ,  tempTileset.pointsY[j].z );
            positionOfObs=""+x+","+z;
            Debug.Log("hey hey hey");
           }
           positionOfObs="";
           Vector3 pos = new Vector3(x,5.86f, z);
           tempTileset.spawnedIn.Add(""+x+","+z,"bus"); 
           pos = Quaternion.Euler(tempTileset.CurRotation) * pos;
           Vector3 spawnPlace = tempTileset.RoadObj.transform.position + pos;
           Vector3 quat = new Vector3(0,-90,0);
           GameObject obstacle = Instantiate( tempTileset.obstacles[j]  , spawnPlace , Quaternion.Euler(tempTileset.CurRotation + quat)) as GameObject;
           }else{
           Vector3 pos = new Vector3(x,5.86f, z);
           pos = Quaternion.Euler(tempTileset.CurRotation) * pos;
           Vector3 spawnPlace = tempTileset.RoadObj.transform.position + pos;
           Vector3 quat = new Vector3(0,0,0);
           GameObject obstacle = Instantiate( tempTileset.obstacles[j]  , spawnPlace , Quaternion.Euler(tempTileset.CurRotation + quat)) as GameObject;

          }

         
        }else if(dynamicObs){
          if(tempTileset.obstacles[j].tag == "man"){
           
           while(tempTileset.spawnedIn.ContainsKey(positionOfObs)){

            x = Random.Range( tempTileset.pointsX[j].x , tempTileset.pointsY[j].x );
            z = Random.Range( tempTileset.pointsX[j].z ,  tempTileset.pointsY[j].z );
            positionOfObs=""+x+","+z;
            Debug.Log("hey hey hey");

           }
           positionOfObs="";
           Vector3 pos = new Vector3(x,5.86f,z);
           tempTileset.spawnedIn.Add(""+x+","+z,"man"); 
           pos = Quaternion.Euler(tempTileset.CurRotation) * pos;
           Vector3 spawnPlace = tempTileset.RoadObj.transform.position + pos;
           Vector3 quat = new Vector3(0,180,0);
           GameObject obstacle = Instantiate( tempTileset.obstacles[j]  , spawnPlace , Quaternion.Euler(tempTileset.CurRotation + quat)) as GameObject;
            quat = new Vector3(x,-spawnPlace.y,z);
            quat = Quaternion.Euler(tempTileset.CurRotation) * quat;
            quat = tempTileset.RoadObj.transform.position - quat;
            obstacle.GetComponent<CharacterMoving>().dest = quat;


           }
        } 

           }

       }
           

    }


}