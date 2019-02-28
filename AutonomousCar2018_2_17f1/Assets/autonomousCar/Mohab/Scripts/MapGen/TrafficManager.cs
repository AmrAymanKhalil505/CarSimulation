using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrafficManager : MonoBehaviour {
	private MapGen MG;
	ArrayList  RightTrafficNodes;
	ArrayList  MiddleTrafficNodes;
	ArrayList  LeftTrafficNodes;
	public void initNodes(){
		RightTrafficNodes=new ArrayList();
		MiddleTrafficNodes=new ArrayList();
		LeftTrafficNodes=new ArrayList();
		MG=GetComponent<MapGen>();
		ArrayList GeneratedMap = MG.GeneratedMap;
		for(int i=0;i<GeneratedMap.Count;i++){
			RoadSTObject tempTileset =(RoadSTObject)GeneratedMap[i];
			bool isBC = tempTileset.isBezierCurve;
			Vector3 [] TrafficNodes = tempTileset.TrafficNodes;
			if(isBC){
				TrafficNodes=tempTileset.TrafficBNodes;
				Vector3 []TrafficNodesR = tempTileset.TrafficBRNodes;
				Debug.Log(MiddleTrafficNodes.Count);
				if(TrafficNodes.Length!= 3){
					print("traffic nodes is marked as Bezier but the length isn't divisible by 3");
					return;
				}else {
					Debug.Log("b5");
					for(int j=0;j<TrafficNodes.Length;j+=3){
						Vector3 p0 =TrafficNodes[j];
						Vector3 p1 =TrafficNodes[j+1];
						Vector3 p2 =TrafficNodes[j+2];

						// Vector3 p0l =Quaternion.Euler(new Vector3(0,-90,0))*(Vector3.forward * tempTileset.LengthLeft)+ p0;
						// Vector3 p1l =Quaternion.Euler(new Vector3(0,-45,0))*(Vector3.forward * tempTileset.LengthLeft)+ p1;
						// Vector3 p2l =Quaternion.Euler(new Vector3(0,0,0))*(Vector3.forward * tempTileset.LengthLeft)+ p2;

						// Vector3 p0r =Quaternion.Euler(new Vector3(0,90,0))*(Vector3.forward * tempTileset.LengthRight)+ p0;
						// Vector3 p1r =Quaternion.Euler(new Vector3(0,135,0))*(Vector3.forward * tempTileset.LengthRight)+ p1;
						// Vector3 p2r =Quaternion.Euler(new Vector3(0,180,0))*(Vector3.forward * tempTileset.LengthRight)+ p2;
						Vector3 p0l =Quaternion.Euler(TrafficNodesR[j]+new Vector3(0,-90,0))*(Vector3.forward * tempTileset.LengthLeft)+ p0;
						Vector3 p1l =Quaternion.Euler(TrafficNodesR[j+1]+new Vector3(0,-45,0))*(Vector3.forward * tempTileset.LengthLeft)+ p1;
						Vector3 p2l =Quaternion.Euler(TrafficNodesR[j+2]+new Vector3(0,0,0))*(Vector3.forward * tempTileset.LengthLeft)+ p2;

						Vector3 p0r =Quaternion.Euler(TrafficNodesR[j]+new Vector3(0,90,0))*(Vector3.forward * tempTileset.LengthRight)+ p0;
						Vector3 p1r =Quaternion.Euler(TrafficNodesR[j+1]+new Vector3(0,135,0))*(Vector3.forward * tempTileset.LengthRight)+ p1;
						Vector3 p2r =Quaternion.Euler(TrafficNodesR[j+2]+new Vector3(0,180,0))*(Vector3.forward * tempTileset.LengthRight)+ p2;

						List <Vector3>Nodes =BezierArc(p0,p1,p2,tempTileset.NumLerpNodes);
						List <Vector3>NodesLeft =BezierArc(p0l,p1l,p2l,tempTileset.NumLerpNodes);
						List <Vector3>NodesRight =BezierArc(p0r,p1r,p2r,tempTileset.NumLerpNodes);
						for(int k=0;k<Nodes.Count-1;k++){
							// if(k==0||k==Nodes.Count-1){
							Vector3 tempNode = Nodes[k];
							Vector3 tempMiddleNode = Quaternion.Euler(tempTileset.CurRotation)*tempNode +tempTileset.CurPosition;
							Vector3	tempRightNode =Quaternion.Euler(tempTileset.CurRotation)*NodesRight[k] +tempTileset.CurPosition;
							Vector3 tempLeftNode =Quaternion.Euler(tempTileset.CurRotation)*NodesLeft[k]+tempTileset.CurPosition;
							RightTrafficNodes.Add(tempRightNode);
							MiddleTrafficNodes.Add(tempMiddleNode);
							LeftTrafficNodes.Add(tempLeftNode);
							// }else{
							// 	Vector3 tempNode = Nodes[k];
							// 	Vector3 tempMiddleNode = Quaternion.Euler(tempTileset.CurRotation)*tempNode +tempTileset.CurPosition;
							// 	Vector3 tempLastMiddleNode =(Vector3) MiddleTrafficNodes[MiddleTrafficNodes.Count-1];
							// 	tempNode= (tempMiddleNode-(tempLastMiddleNode-tempTileset.CurPosition)).normalized;
							// 	Vector3 tempRightNode=Quaternion.Euler(new Vector3(0,90,0))*(tempNode * tempTileset.LengthRight)+  (tempLastMiddleNode-tempTileset.CurPosition);
							// 	tempRightNode =Quaternion.Euler(tempTileset.CurRotation)*tempRightNode +tempTileset.CurPosition;
							// 	Vector3 tempLeftNode = Quaternion.Euler(new Vector3(0,-90,0))*(tempNode * tempTileset.LengthLeft)+ (tempLastMiddleNode-tempTileset.CurPosition);
							// 	tempLeftNode=Quaternion.Euler(tempTileset.CurRotation)*tempLeftNode +tempTileset.CurPosition;
							// 	RightTrafficNodes.Add(tempRightNode);
							// 	MiddleTrafficNodes.Add(tempMiddleNode);
							// 	LeftTrafficNodes.Add(tempLeftNode);
							// }
						}
					}
				}
			}else{
				Debug.Log(MiddleTrafficNodes.Count);

				for(int j=0;j<TrafficNodes.Length;j++){
					//RotateNode
					Debug.Log(MiddleTrafficNodes.Count);
					Vector3 tempNode = TrafficNodes[j];
					Vector3 tempMiddleNode = Quaternion.Euler(tempTileset.CurRotation)*tempNode +tempTileset.CurPosition;
					Vector3 tempRightNode = Quaternion.Euler(new Vector3(0,90,0))*(Vector3.forward * tempTileset.LengthRight)+tempNode;
					tempRightNode =Quaternion.Euler(tempTileset.CurRotation)*tempRightNode +tempTileset.CurPosition;
					Vector3 tempLeftNode = Quaternion.Euler(new Vector3(0,-90,0))*(Vector3.forward * tempTileset.LengthLeft)+ tempNode;
					tempLeftNode=Quaternion.Euler(tempTileset.CurRotation)*tempLeftNode +tempTileset.CurPosition;
					
					if(j>0 && tempTileset.NumLerpNodes >0){
						int LastNodeInd =MiddleTrafficNodes.Count-1; 			
						for(int k=1;k<tempTileset.NumLerpNodes;k++){
							RightTrafficNodes.Add(Vector3.Lerp((Vector3)RightTrafficNodes[LastNodeInd],tempRightNode,k*1.0f/tempTileset.NumLerpNodes*1.0f));
							MiddleTrafficNodes.Add(Vector3.Lerp((Vector3)MiddleTrafficNodes[LastNodeInd],tempMiddleNode,k*1.0f/tempTileset.NumLerpNodes*1.0f));
							LeftTrafficNodes.Add(Vector3.Lerp((Vector3)LeftTrafficNodes[LastNodeInd],tempLeftNode,k*1.0f/tempTileset.NumLerpNodes*1.0f));		
						}
					}else{
						RightTrafficNodes.Add(tempRightNode);
						MiddleTrafficNodes.Add(tempMiddleNode);
						LeftTrafficNodes.Add(tempLeftNode);
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
	void Update () {
		
	}
	void OnDrawGizmosSelected() {
		 
		if(RightTrafficNodes != null){
			
			for(int i=0;i<RightTrafficNodes.Count;i++){
				Gizmos.color = Color.yellow;
				Gizmos.DrawSphere((Vector3)RightTrafficNodes[i]+ new Vector3(0,15,0),3);
			}
		}
		
		if(MiddleTrafficNodes != null){
			
			for(int i=0;i<MiddleTrafficNodes.Count;i++){
				Gizmos.color = Color.red;
				Gizmos.DrawSphere((Vector3)MiddleTrafficNodes[i]+ new Vector3(0,15,0),3);
			}
		}
		 
		if(LeftTrafficNodes != null){
			
			for(int i=0;i<LeftTrafficNodes.Count;i++){
				Gizmos.color = Color.blue;
				Gizmos.DrawSphere((Vector3)LeftTrafficNodes[i]+ new Vector3(0,15,0),3);
			}
		}
		
	}
}