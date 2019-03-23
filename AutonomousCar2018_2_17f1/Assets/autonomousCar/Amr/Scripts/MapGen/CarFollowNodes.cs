using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class CarFollowNodes : MonoBehaviour {
	[Header("NavMesh")]
	
	private NavMeshAgent NMA;
	public int separations;

	public ArrayList  RightTrafficNodes;
	public ArrayList  MiddleTrafficNodes;
	public ArrayList  LeftTrafficNodes;

	public bool Rev ;
	public int CurrectNodeIndex;
	public Vector3 LastNode;

	public float nearNodeLimit=3f;

	bool isOnRoadNavMesh = false; 
	void Start () {
		//TODO Check if there is NMA 
		NMA = GetComponent<NavMeshAgent>();
		Invoke("ActivateNavMesh",3f);	
		LastNode=Vector3.zero; 
	}
	void Update () {
		if(isOnRoadNavMesh){
			CheckWaypointDistance();
		}
	}

	
	void ActivateNavMesh(){
		isOnRoadNavMesh= true;
		NMA.enabled=true;
		NMA.isStopped = false;
		Vector3 Pos_with_y = transform.position;
		Pos_with_y.y=0;
		LastNode=RightNode(Pos_with_y,CurrectNodeIndex);
		NMA.SetDestination(LastNode);
	}
	private void CheckWaypointDistance() {
		Vector3 BN =  RightNode (transform.position,CurrectNodeIndex);
		Vector3 Pos_with_noy = transform.position;
		Pos_with_noy.y=0;
		   if (Vector3.Distance(Pos_with_noy, RightNode(Pos_with_noy,CurrectNodeIndex)) < nearNodeLimit) {
            if (!Rev){
				if (CurrectNodeIndex >= MiddleTrafficNodes.Count - 1) {
                	CurrectNodeIndex = 0;
					// print(BN);
            	} else {
                	CurrectNodeIndex++;
            	}
			}else{
				if (CurrectNodeIndex <= 0) {
					// print(BN);
                	CurrectNodeIndex = MiddleTrafficNodes.Count - 1;
            	} else {
                	CurrectNodeIndex--;
            	}
			}
			
        }
		if(!BN.Equals(LastNode)){
			NMA.SetDestination(BN);
		}
		Debug.DrawLine(Pos_with_noy,BN,Color.white);
	}

	private Vector3 RightNode(Vector3 pos,int CNI){
		Vector3 rn = (Vector3) RightTrafficNodes[CNI];
		Vector3 ln = (Vector3) LeftTrafficNodes[CNI];
		
		Vector3 bestMatch = rn;  
		Vector3 Pos_with_noy = transform.position;
		Pos_with_noy.y=0;
		float spFactor = 1.0f / (separations*1.0f) ;
		//print(separations+"hah");
		for( int i = 0; i<separations-1;i++){
			Vector3 t_node = Vector3.Lerp(rn,ln,i*spFactor);
			if(Vector3.Distance(Pos_with_noy,t_node)<Vector3.Distance(Pos_with_noy,bestMatch)&& isValidNode(t_node)){
				bestMatch= t_node;
				// print(bestMatch);
			}
		}
		return bestMatch;
	}
	private bool isValidNode(Vector3 node){
		return true;
	}
	
	float onMeshThreshold = 3;
	public bool IsAgentOnNavMesh(GameObject agentObject)
	{
		Vector3 agentPosition = agentObject.transform.position;
		NavMeshHit hit;

		// Check for nearest point on navmesh to agent, within onMeshThreshold
		if (NavMesh.SamplePosition(agentPosition, out hit, onMeshThreshold, NavMesh.AllAreas))
		{
			// Check if the positions are vertically aligned
			if (Mathf.Approximately(agentPosition.x, hit.position.x)
				&& Mathf.Approximately(agentPosition.z, hit.position.z))
			{
				// Lastly, check if object is below navmesh
				return agentPosition.y >= hit.position.y;
			}
		}

		return false;
	}
	
}
