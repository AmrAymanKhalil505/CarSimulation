using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class CarFollowNodes : MonoBehaviour {
	[Header("NavMesh")]
	
	private NavMeshAgent NMA;
	public float separations;

	ArrayList  RightTrafficNodes;
	ArrayList  MiddleTrafficNodes;
	ArrayList  LeftTrafficNodes;

	public bool rev ;

	int currectNode;
	void Start () {
		//TODO Check if there is NMA 
		NMA = GetComponent<NavMeshAgent>();

	}
	
	
	void Update () {
		
	}

	
}
