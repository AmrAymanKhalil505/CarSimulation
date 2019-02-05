using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour {

	// Use this for initialization
	public GameObject Cube;
	void Start () {
		ArrayList x = new ArrayList();
		for(int i=0;i< 10;i++){
			GameObject test = Instantiate (Cube,new Vector3 (i*10, 0,0),Quaternion.Euler(0,0,0));
			x.Add(test);
		}
		print ( ((GameObject)x[8]).transform.position);
		Destroy(((GameObject)x[8]));
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
