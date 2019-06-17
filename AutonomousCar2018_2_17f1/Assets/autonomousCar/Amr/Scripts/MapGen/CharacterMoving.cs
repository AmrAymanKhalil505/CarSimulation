#Created by: Loay Naser

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityStandardAssets.Vehicles.Car;
using UnityStandardAssets.Characters.ThirdPerson;
[RequireComponent(typeof(Animator))]
public class CharacterMoving : MonoBehaviour {

    Animator m_Animator;

	[Header("NavMesh")]
	
	private NavMeshAgent NMA;
    
	public Vector3 dest;
    public Vector3 spawn;

    public Quaternion rotation;

    public GameObject character;
	public float nearNodeLimit=9f;

    public ThirdPersonCharacter person;
	bool isOnRoadNavMesh = false; 

    private Vector3 theCarPos;
    private bool accident = false;

	private bool nearbyCar = false;
	private bool checkX = false;
	private bool checkZ = false;
	Vector3 temp;

	private CarController cc;


	void Start () {
		NMA = GetComponent<NavMeshAgent>();
		Invoke("ActivateNavMesh",3f);	
        NMA.updateRotation = false;
		cc = GameObject.Find("Car").GetComponent<CarController>();
		temp = dest;
	}

	void Update () {
		theCarPos = GameObject.Find("Car").transform.position; //getting the current position of the Car agent
        if(isOnRoadNavMesh){
			CheckWaypointDistance();
		}
      person.Move(NMA.desiredVelocity,false,false);
	}
	
     void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag=="Player" && cc.CurrentSpeed > 5 ){
			    
                NMA.enabled = false;
                accident = true;
                person.logAccident(accident);
                 Destroy(gameObject, 3);
      }
    }

	void ActivateNavMesh(){
        spawn = transform.position ;
        rotation = transform.rotation;
		isOnRoadNavMesh= true;
		NMA.enabled=true;
		NMA.isStopped = false;
		Vector3 Pos_with_y = transform.position;
		NMA.SetDestination(dest);
	}

	private void CheckWaypointDistance() {
		 /* This function does two things :
		  1-  checking the distance between the character and the destination 
		  2-  checking the distance between the character and the car agent to prevent unreal collision  */ 

		Vector3 Pos_with_noy = transform.position; // getting the current position of the character

		bool check = ((Mathf.Abs(theCarPos.z - Pos_with_noy.z ) < 3.5 ) &&
					  Mathf.Abs(theCarPos.x - Pos_with_noy.x ) < 1 ) ; // checking the distance between the character and car agent

         if (NMA.isActiveAndEnabled)	{

			NMA.SetDestination(temp);
			nearbyCar = false;
			person.logStop(nearbyCar);

		 }

		if(check){

           	if (NMA.isActiveAndEnabled)	{

				nearbyCar = true;
				person.logStop(nearbyCar);
				NMA.SetDestination(Pos_with_noy);

          }
		}else{
		if (Vector3.Distance(dest, Pos_with_noy ) < nearNodeLimit) { // checking if the character reached it's destination

			if (NMA.isActiveAndEnabled)	{
			   	temp = spawn;
                NMA.SetDestination(spawn);
				}	
				}
			 else if(Vector3.Distance( Pos_with_noy , spawn ) < nearNodeLimit){ // checking if the character reached it's destination

				if (NMA.isActiveAndEnabled)	{
					temp = dest;
					NMA.SetDestination(dest);
		
					}
			 }else if ((theCarPos.z <= Pos_with_noy.z+5 && theCarPos.z >= Pos_with_noy.z - 5 )&& checkZ ){
		    	if (NMA.isActiveAndEnabled)	{
				
					nearbyCar = true;
					person.logStop(nearbyCar);
					NMA.SetDestination(Pos_with_noy);

				}
			 }
          }
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
