using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "Road", menuName = "Inventory/Road", order = 1)]
public class RoadSTObject : ScriptableObject {

	public GameObject RoadObj;
	public string RoadName ;
	public float Percentage;
	public Vector3 SizeMin;
	public Vector3 SizeMax;
	
	public Vector3 CorrectPosition;
	public float OffestInToCenter;
	public float OffestOutToCenter;

	public Vector3 DirectionIn;
	public Vector3 DirectionOut;

	public Vector3 CurPosition;
	public Vector3 CurRotation;

	public Vector3 initPosition;
	public Vector3 initRotation;
	public  string [] SuitableNextTileSet;

	public Vector3 [] TrafficNodes;
	public float LengthRight;
	public float LengthLeft;
	public int NumLerpNodes;
	public bool isBezierCurve;

	public Vector3 [] TrafficBNodes;
	public Vector3 [] TrafficBRNodes;
	

}
