using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestingTileSet : MonoBehaviour {
	public RoadSTObject RSTO;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	void OnDrawGizmosSelected() {
		Vector3 p0 = RSTO.SizeMin+transform.position+Vector3.up*15;
		Vector3 p3 = RSTO.SizeMax+transform.position+Vector3.up*15;
		Vector3 p2 = new Vector3(p0.x,0,p3.z)+transform.position+Vector3.up*15;
		Vector3 p1 = new Vector3(p3.x,0,p0.z)+transform.position+Vector3.up*15;
		
		Gizmos.color =Color.green;
		for(int i=0;i<=9;i++){
			Gizmos.DrawSphere(Vector3.Lerp(p0,p1,i*1.0f/9)+Vector3.up*15,3);
			Gizmos.DrawSphere(Vector3.Lerp(p0,p2,i*1.0f/9)+Vector3.up*15,3);
			Gizmos.DrawSphere(Vector3.Lerp(p3,p2,i*1.0f/9)+Vector3.up*15,3);
			Gizmos.DrawSphere(Vector3.Lerp(p3,p1,i*1.0f/9)+Vector3.up*15,3);
		}
		Gizmos.DrawLine(p0,p1);
		Gizmos.DrawLine(p1,p3);
		Gizmos.DrawLine(p2,p3);
		Gizmos.DrawLine(p2,p0);
		



		bool isBC = RSTO.isBezierCurve;

		if(isBC){
				ArrayList MiddleTrafficNodes=new ArrayList();
				Vector3 [] TrafficNodes = RSTO.TrafficNodes;
				if(TrafficNodes.Length%3!= 0){
					print("traffic nodes is marked as Bezier but the length isn't divisible by 3");
					return;
				}else {
					Debug.Log("b5");
					for(int j=0;j<TrafficNodes.Length;j+=3){
						p0 =TrafficNodes[j];
						p1 =TrafficNodes[j+1];
						p2 =TrafficNodes[j+2];
						List <Vector3>Nodes =BezierArc(p0,p1,p2,RSTO.NumLerpNodes);
						for(int k=0;k<Nodes.Count-1;k++){
							if(k==0||k==Nodes.Count-1){
							Vector3 tempNode = Nodes[k];
							Vector3 tempMiddleNode = Quaternion.Euler(RSTO.CurRotation)*tempNode +RSTO.CurPosition;
							Vector3 tempRightNode = Quaternion.Euler(new Vector3(0,90,0))*(Vector3.forward * RSTO.LengthRight)+ tempNode;
							tempRightNode =Quaternion.Euler(RSTO.CurRotation)*tempRightNode +RSTO.CurPosition;
							Vector3 tempLeftNode = Quaternion.Euler(new Vector3(0,-90,0))*(Vector3.forward * RSTO.LengthLeft)+ tempNode;
							tempLeftNode=Quaternion.Euler(RSTO.CurRotation)*tempLeftNode +RSTO.CurPosition;
							Gizmos.color =Color.red;
							Gizmos.DrawSphere(tempRightNode+Vector3.up*15,3);
							Gizmos.color =Color.magenta;
							Gizmos.DrawSphere(tempMiddleNode+Vector3.up*15,3);
							Gizmos.color =Color.cyan;
							Gizmos.DrawSphere(tempLeftNode+Vector3.up*15,3);
							MiddleTrafficNodes.Add(tempMiddleNode);
							}else{
								Vector3 tempNode = Nodes[k];
								Vector3 tempMiddleNode = Quaternion.Euler(RSTO.CurRotation)*tempNode +RSTO.CurPosition;
								Vector3 tempLastMiddleNode =(Vector3) MiddleTrafficNodes[MiddleTrafficNodes.Count-1];
								tempNode= (tempMiddleNode-(tempLastMiddleNode-RSTO.CurPosition)).normalized;
								Vector3 tempRightNode=Quaternion.Euler(new Vector3(0,90,0))*(tempNode * RSTO.LengthRight)+  (tempLastMiddleNode-RSTO.CurPosition);
								tempRightNode =Quaternion.Euler(RSTO.CurRotation)*tempRightNode +RSTO.CurPosition;
								Vector3 tempLeftNode = Quaternion.Euler(new Vector3(0,-90,0))*(tempNode * RSTO.LengthLeft)+ (tempLastMiddleNode-RSTO.CurPosition);
								tempLeftNode=Quaternion.Euler(RSTO.CurRotation)*tempLeftNode +RSTO.CurPosition;
								Gizmos.color =Color.red;
								Gizmos.DrawSphere(tempRightNode+Vector3.up*15,3);
								Gizmos.color =Color.magenta;
								Gizmos.DrawSphere(tempMiddleNode+Vector3.up*15,3);
								Gizmos.color =Color.cyan;
								Gizmos.DrawSphere(tempLeftNode+Vector3.up*15,3);
								MiddleTrafficNodes.Add(tempMiddleNode);
							}
						}
					}
				}
			}
	}
	List <Vector3> BezierArc(Vector3 p0,Vector3 p1,Vector3 p2,int NumLerpNodes){
		List <Vector3> output = new List<Vector3> ();
		float i=0.0f;
		float turnSeparation = 1.0f/(NumLerpNodes*1.0f);
		while (i<=1){
			output.Add (BezierPoint (p0, p1, p2, i));
			i += turnSeparation;
		}
		return output;
	}
	Vector3 BezierPoint(Vector3 p0,Vector3 p1,Vector3 p2, float t){
		Vector3 output = Vector3.zero;
		output.x = (1 - t) * (1 - t) * p0.x + 2 * (1 - t) * t * p1.x + t * t * p2.x;
		output.z = (1 - t) * (1 - t) * p0.z + 2 * (1 - t) * t * p1.z + t * t * p2.z;
		return output;
	}
}
