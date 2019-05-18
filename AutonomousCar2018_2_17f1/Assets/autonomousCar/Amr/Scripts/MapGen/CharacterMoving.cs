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

	int moving=0;
//    public CarController m_Car;
  
    // public AudioSource soundEffect;

	void Start () {
		//TODO Check if there is NMA 
		NMA = GetComponent<NavMeshAgent>();
		Invoke("ActivateNavMesh",3f);	
        NMA.updateRotation = false;
		cc = GameObject.Find("Car").GetComponent<CarController>();
		temp = dest;
	}

	void Update () {
		theCarPos = GameObject.Find("Car").transform.position;
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
		Vector3 Pos_with_noy = transform.position;
		//Debug.Log(moving);
     

         Vector3 target = Pos_with_noy - spawn;
		//  Debug.Log("here x"+ (Mathf.Abs(target.x)-Mathf.Abs(spawn.x)));
        //   Debug.Log("here z"+ (Mathf.Abs(Mathf.Abs(target.x)-Mathf.Abs(spawn.x))>Mathf.Abs(Mathf.Abs(target.z)-Mathf.Abs(spawn.z))));
	    //  if(moving == 10){

        //  if(Mathf.Abs(target.x-spawn.x)>Mathf.Abs(target.z-spawn.z)){
		// 			Debug.Log("hey there");
		// 			checkX = true;

		//  }else{
				
        //             Debug.Log("shishsish");
		// 			checkZ = true;
				
		//  }
		//  }

		
	    // Debug.Log("current" +Pos_with_noy);
		//  Debug.Log("spawn" + spawn);
		// Debug.Log("dest" + dest);
		bool check = ((Mathf.Abs(theCarPos.z - Pos_with_noy.z ) < 3.5 ) &&
		Mathf.Abs(theCarPos.x - Pos_with_noy.x ) < 1 ) ;
		//  ||
		// ((theCarPos.x <= Pos_with_noy.x + 3.5 && theCarPos.x >= Pos_with_noy.x - 3.5) && 
		// Mathf.Abs(theCarPos.z - Pos_with_noy.z ) < 1  && checkX );
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
		if (Vector3.Distance(dest, Pos_with_noy ) < nearNodeLimit) {

			if (NMA.isActiveAndEnabled)	{
			   	temp = spawn;
                NMA.SetDestination(spawn);
				}
				
				
				
				}
			 else if(Vector3.Distance( Pos_with_noy , spawn ) < nearNodeLimit){
               
			  
	
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
		  moving++;
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
