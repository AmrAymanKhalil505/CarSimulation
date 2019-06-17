//Created by: Loay Naser

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//TODO documentation 
public class ObstacleManager : MonoBehaviour {
	private MapGen MG;

  // the amount of cones intiallized in one row 
  private int coneCount = 0;

  // Two variables that help to make the intialization of horizontal cones successful
  private float coneX=0.0f;
  private float coneZ=0.0f;  

  // A variable that gets assigned a random value to determine in which lane the cone row spawns
  private int whichLane=0;

  private int conesLimit = 3;
	public void initObstacles( bool staticObs , bool dynamicObs){   // intiate obstacles static and dynamic ones
         
          MG=GetComponent<MapGen>();
	  
        	ArrayList GeneratedMap = MG.GeneratedMap;
      
          for(int i=0;i<GeneratedMap.Count;i++){
    
         	RoadSTObject tempTileset = (RoadSTObject)GeneratedMap[i];

          tempTileset.spawnedIn = new Hashtable();
 
          for(int j = 0 ; j < tempTileset.obstacles.Length; j++){
          string positionOfObs="";
          float x = Random.Range( tempTileset.pointsX[j].x , tempTileset.pointsY[j].x );
          float z = Random.Range( tempTileset.pointsX[j].z ,  tempTileset.pointsY[j].z );
         
          if(staticObs){ // checking if user enabled the static obstacles boolean
          
          if(tempTileset.obstacles[j].tag == "bump"){ // checking if the obstacle is a bump obstacle
          
           while(tempTileset.spawnedIn.ContainsKey(positionOfObs)){ // prevent obstacles overlapping on top of each other

            x = Random.Range( tempTileset.pointsX[j].x , tempTileset.pointsY[j].x );
            z = Random.Range( tempTileset.pointsX[j].z ,  tempTileset.pointsY[j].z );
            positionOfObs=""+x+","+z;
           }
           positionOfObs="";
           while(z<5 && z>-5){ // prevent intiallzing of obstacles in the center of the roadtile, to prevent collision of the obstacle and the car agent when starting the episode

              z = Random.Range( tempTileset.pointsX[j].z , 
               tempTileset.pointsY[j].z );
           

           }
           Vector3 pos = new Vector3(x,5.9f, z);
           tempTileset.spawnedIn.Add(""+x+","+z,"bump"); 
           pos = Quaternion.Euler(tempTileset.CurRotation) * pos;
           Vector3 spawnPlace = tempTileset.RoadObj.transform.position + pos;
           Vector3 quat = new Vector3(-90,180,90);
           GameObject obstacle = Instantiate( tempTileset.obstacles[j] , spawnPlace , Quaternion.Euler(tempTileset.CurRotation + quat) ) as GameObject;
         
          }else if(tempTileset.obstacles[j].tag == "StreetHole"){ // checking if the obstacle is a street hole obstacle
          
            while(tempTileset.spawnedIn.ContainsKey(positionOfObs)){ // prevent obstacles overlapping on top of each other

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
          }else if(tempTileset.obstacles[j].tag == "cone" ){ // checking if the obstacle is a cone obstacle
            
            while(tempTileset.spawnedIn.ContainsKey(positionOfObs)){
            
            x = Random.Range( tempTileset.pointsX[j].x , tempTileset.pointsY[j].x );
            z = Random.Range( tempTileset.pointsX[j].z ,  tempTileset.pointsY[j].z );
            positionOfObs=""+x+","+z;
           }
           positionOfObs="";
           while(z<7 &&  z>-7 || ( Mathf.Abs(z - coneZ) < 15)){   // creates vertical distance between multiple row of cones
            z = Random.Range( tempTileset.pointsX[j].z , 
                tempTileset.pointsY[j].z );

             }
          if(coneCount == 0){ // if this is the first cone in the row
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
           Vector3 pos = new Vector3(coneX,6.113f, coneZ); // continue building the row of cones
           tempTileset.spawnedIn.Add(""+coneX+","+coneZ,"cone"); 
           pos = Quaternion.Euler(tempTileset.CurRotation) * pos;
           Vector3 spawnPlace = tempTileset.RoadObj.transform.position + pos;
           Vector3 quat = new Vector3(-90,180,90);
           GameObject obstacle = Instantiate( tempTileset.obstacles[j]  , spawnPlace ,  Quaternion.Euler(tempTileset.CurRotation + quat) ) as GameObject;
           coneCount = (coneCount==conesLimitß)?0:coneCount;
          }



          }else if(tempTileset.obstacles[j].tag == "sign"){ // checking if the obstacle is a sign obstacle
            
            while(tempTileset.spawnedIn.ContainsKey(positionOfObs)){

            x = Random.Range( tempTileset.pointsX[j].x , tempTileset.pointsY[j].x );
            z = Random.Range( tempTileset.pointsX[j].z ,  tempTileset.pointsY[j].z );
            positionOfObs=""+x+","+z;
           }
           positionOfObs="";
           Vector3 pos = new Vector3(x,8.19f, z);
           tempTileset.spawnedIn.Add(""+x+","+z,"sign"); 
           pos = Quaternion.Euler(tempTileset.CurRotation) * pos;
           Vector3 spawnPlace = tempTileset.RoadObj.transform.position + pos;
           Vector3 quat = new Vector3(-90,180,90);
           GameObject obstacle = Instantiate( tempTileset.obstacles[j]  , spawnPlace ,  Quaternion.Euler(tempTileset.CurRotation + quat) ) as GameObject;
  



          }else if(dynamicObs && tempTileset.obstacles[j].tag == "man"){ // checking if user enabled dynamic obstacles and the obstacle is a man obstacle
            
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
            obstacle.GetComponent<CharacterMoving>().dest = quat; // setting the destination of the character
          
          }else if(tempTileset.obstacles[j].tag == "bus" && tempTileset.RoadName == "Ahead"){ //checking if the obstacle is a bus obstacle and in the Straight-Up roadtile
          
           while(tempTileset.spawnedIn.ContainsKey(positionOfObs)){

            x = Random.Range( tempTileset.pointsX[j].x , tempTileset.pointsY[j].x );
            z = Random.Range( tempTileset.pointsX[j].z ,  tempTileset.pointsY[j].z );
            positionOfObs=""+x+","+z;
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
           GameObject obstacle = Instantiate(tempTileset.obstacles[j], spawnPlace , Quaternion.Euler(tempTileset.CurRotation + quat)) as GameObject;

          }

         
        }else if(dynamicObs && tempTileset.obstacles[j].tag == "man")){ //checking user enabled dynamic obstacles only 
           
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

           }

       }
           

    }


}