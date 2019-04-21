using System.Collections;
using UnityEngine;

public class BusScript : MonoBehaviour {
public GameObject prefab;
void Start(){
   this.gameObject.GetComponent<Collider> ().isTrigger=true;
    Physics.IgnoreCollision(this.gameObject.GetComponent<Collider> (), prefab.GetComponent<Collider> ());
 }
 
}
