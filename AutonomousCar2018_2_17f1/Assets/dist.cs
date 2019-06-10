using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dist : MonoBehaviour {
 public GameObject go1;
 public GameObject go2;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		bool isKeyDown=Input.GetKey(KeyCode.Q);

	if(isKeyDown){
   if (go1 != null && go2 != null)
    {
       float dist = Vector3.Distance(go1.transform.position, go2.transform.position);
       Debug.Log(string.Format("Distance between {0} and {1} is: {2}", go1, go2, dist));
    }
		}
	}
}

