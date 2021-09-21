using System.Collections;
using UnityEngine;

public class CarCollision : MonoBehaviour {

 void OnCollisionEnter(Collision col) { 

 if(col.gameObject.tag == "TrafficCar"){ 
  Debug.Log("COLLIDE!!");
  Destroy(col.gameObject); 
 }else if(col.gameObject.tag == "cone"){
    Destroy(col.gameObject,5); 

 }
 }
}
