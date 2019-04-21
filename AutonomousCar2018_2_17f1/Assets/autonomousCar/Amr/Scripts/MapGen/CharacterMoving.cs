using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
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

    private bool accident = false;

  
    // public AudioSource soundEffect;

	void Start () {
		//TODO Check if there is NMA 
		NMA = GetComponent<NavMeshAgent>();
		Invoke("ActivateNavMesh",3f);	
        NMA.updateRotation = false;

	}

	void Update () {
		if(isOnRoadNavMesh){
			CheckWaypointDistance();
		}

	  person.Move(NMA.desiredVelocity,false,false);
        //Debug.Log(NMA.remainingDistance);
	}
	
     void OnCollisionEnter(Collision collision)
    {
        //Debug.Log(collision.gameObject.tag);
        if(collision.gameObject.tag=="Player"){
                NMA.enabled = false;
                accident = true;
                person.logAccident(accident);
                 Destroy(gameObject, 3);

               //  GameObject obstacle = Instantiate( character  , spawn ,  rotation ) as GameObject;
  
               // soundEffect.Play();
               
                 // Destroy(gameObject);
                //m_Animator.SetBool("DeathTrigger", true);
  

        }
    }

	void ActivateNavMesh(){
        spawn = transform.position ;
        rotation = transform.rotation;
       
		isOnRoadNavMesh= true;
		NMA.enabled=true;
		NMA.isStopped = false;
		Vector3 Pos_with_y = transform.position;
        // dest = transform.position + (new Vector3 (0,0,40));
		//Debug.Log(dest);
		// if(Vector3.Distance(dest, new Vector3(40,0,0) ) == 0){

        //    dest =  new Vector3(0,0,40);


		// }else{

		//   dest =  new Vector3(40,0,0);


		// }
		NMA.SetDestination(dest);
	}

	private void CheckWaypointDistance() {
		Vector3 Pos_with_noy = transform.position;
		// Debug.Log("current" +Pos_with_noy);
		// Debug.Log("spawn" + spawn);
		// Debug.Log("dest" + dest);
		if (Vector3.Distance(dest, Pos_with_noy ) < nearNodeLimit) {
       //         Destroy(gameObject);
      //          GameObject obstacle = Instantiate( character  , spawn ,  rotation ) as GameObject;
            	//Debug.Log(NMA.isActiveAndEnabled);
			if (NMA.isActiveAndEnabled)	{

                  NMA.SetDestination(spawn);
				}
				
				
				
				}
			 else if(Vector3.Distance( Pos_with_noy , spawn ) < nearNodeLimit){
               
			  
	
	      	if (NMA.isActiveAndEnabled)	{

                 NMA.SetDestination(dest);
	
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
