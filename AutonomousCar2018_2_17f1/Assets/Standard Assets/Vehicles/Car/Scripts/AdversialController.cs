﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdversialController : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		transform.Translate(0f,0f,4f*Time.deltaTime);
		
	}
}
