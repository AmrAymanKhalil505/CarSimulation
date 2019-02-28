using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MapGen : MonoBehaviour {
	[Header("Car Setting")]
	//Car which RL agent will run on 
	public GameObject Car;
	//the position will it appear in a the first of the simulation 
	public Vector3 CarInitPosition;

	[Header ("Roads")]
	//Roads that will be used to as tilesets
	public RoadSTObject RoadAhead;
	public RoadSTObject RoadRight;
	public RoadSTObject RoadLeft;
	public RoadSTObject RoadCross;
	//Array that will hold all the tilesets
	private ArrayList AvailableRoads;
	

	[Header ("Map Setting")]
	// number of tilesets that should spawn in the map at the end 
	public int GeneratedMapLen;
	//allowed size for the map
	public Vector3 MapStart;
	public Vector3 MapEnd;
	//the array that will have the generated tilesets as scriptable objects
	public ArrayList  GeneratedMap;
	private bool WorkOnMap =false;

	// counter to see the number of clones of the scriptabe objects to make sure that the ram is never full
	int NOI = 0;

	// small number near to zero that will be used to handle collisions 
	static float EPS = 1E-9f;

	// each map generation object parent should have traffic manager 
	TrafficManager TM ;
	void Start () {
		AvailableRoads=new ArrayList();
		GeneratedMap=new ArrayList();

		Car.transform.position = CarInitPosition;
		AvailableRoads.Add(RoadAhead);
		AvailableRoads.Add(RoadRight);
		AvailableRoads.Add(RoadLeft);
		AvailableRoads.Add(RoadCross);
		// TODO handle Null TM 
		TM= GetComponent<TrafficManager>();
		CreateMap();
		TM.initNodes();

	}
	

	void CreateMap(){
		int GridIndex = 0;
		GeneratedMap.Add((RoadSTObject) Object.Instantiate(RoadAhead));
		while(GeneratedMap.Count<GeneratedMapLen){
			addRoad((RoadSTObject)GeneratedMap[GeneratedMap.Count-1]);
			if(NOI>1000){
				return;
			}
		}
		foldMap();
		print(NOI);
	}

	/*
		Name: addRoad
		Description: adds a new tileset of type RoadSTObject to the map according to the given RoadSTObject  
		Inputs: RoadSTObject rtso 
					the given RoadSTObject to make new Tileset according to
	 */
	void addRoad(RoadSTObject rsto){
		/*
			this part get all the suitable Tilesets that can be added next to the given RoadSTObject
			and put it in ChoosenRoads ArrayList
		 */
		ArrayList ChoosenRoads =(ArrayList) AvailableRoads.Clone();
		for(int i=0;i<ChoosenRoads.Count;i++){
			int index =System.Array.IndexOf(rsto.SuitableNextTileSet,((RoadSTObject)ChoosenRoads[i]).RoadName);
			if(index ==-1){
				ChoosenRoads.RemoveAt(i);i--;
			}
		}
		/*
			choose one suitable road that you can added without colliding with other tileset
		 	if all the roads will collide then return error
		 */

		bool isSuitableRoad =false; 
		while(!isSuitableRoad && ChoosenRoads.Count>0){
			//choose one index randomly
			int index = Random.Range(0,ChoosenRoads.Count);
			//make a copy out of that road
			RoadSTObject newTileset =(RoadSTObject) Object.Instantiate((Object)((ChoosenRoads[index])));
			
			Vector3 tempNode = rsto.CurPosition;
			//starting a origin moving the center point to the edge
			tempNode+= Quaternion.Euler(rsto.DirectionOut+rsto.CurRotation)*(rsto.OffestOutToCenter*Vector3.forward);
			print("move to the edge"+tempNode);
			//moving from the edge to the new center and move the point away next to rsto \
			//TODO add Direction in	
			tempNode+= Quaternion.Euler(rsto.DirectionOut+rsto.CurRotation)*(newTileset.OffestInToCenter*Vector3.forward);
			print("move to the new Center"+tempNode);
			
			//check collision
			bool isCol = false;
			for(int i=0;i<GeneratedMap.Count;i++){
				RoadSTObject tempRSTO=(RoadSTObject)GeneratedMap[i];
				Vector3 l1 = tempRSTO.CurPosition  + Quaternion.Euler(tempRSTO.CurRotation)*tempRSTO.SizeMin;
				Vector3 r1 = tempRSTO.CurPosition  + Quaternion.Euler(tempRSTO.CurRotation)*tempRSTO.SizeMax;
				Vector3 l2 = tempNode  + Quaternion.Euler(rsto.CurRotation+rsto.DirectionOut)*newTileset.SizeMin;
				Vector3 r2 = tempNode  + Quaternion.Euler(rsto.CurRotation+rsto.DirectionOut)*newTileset.SizeMax;
				if(isCollisionUsingSegments(l1,r1,l2,r2)){
					isCol=true;
					break;
				}
			}
			if(isCol){
				ChoosenRoads.RemoveAt(index);
				NOI++;
				if(NOI>1000){
					break;
				}
			}else{
				isSuitableRoad = true;
				newTileset.CurPosition=tempNode;
				newTileset.CurRotation=rsto.CurRotation+ rsto.DirectionOut;
				GeneratedMap.Add(newTileset);
				break;
			}
			
		}
		if(isSuitableRoad){
				print(((RoadSTObject)GeneratedMap[GeneratedMap.Count-1]).RoadName);
				return;
			}else{
				GeneratedMap.RemoveAt(GeneratedMap.Count-1);
				print("making map failed");
			}
		
	}
	void foldMap(){
		for(int i=0;i<GeneratedMap.Count;i++){
				RoadSTObject newTileset=(RoadSTObject)GeneratedMap[i];
				GameObject RoadTemp =Instantiate(newTileset.RoadObj);
				RoadTemp.transform.position= newTileset.CurPosition;
				RoadTemp.transform.rotation= Quaternion.Euler(newTileset.initRotation+newTileset.CurRotation);
				newTileset.RoadObj= RoadTemp;
		}
	}

	Vector3 rotateAroundPivot(Vector3 Pivot,Vector3 Point,Quaternion RotationAngle){
		return RotationAngle*(Point-Pivot)+ Pivot;
	}

	bool isCollisionUsingSegments(Vector3 l1, Vector3 r1,Vector3 l2 ,Vector3 r2){
		Point R1P0 = new Point(l1.x,l1.z);
		Point R1P1 = new Point(r1.x,l1.z);
		Point R1P2 = new Point(l1.x,r1.z);
		Point R1P3= new Point(r1.x,r1.z);


		Point R2P0 = new Point(l2.x,l2.z);
		Point R2P1 = new Point(l2.x,r2.z);
		Point R2P2 = new Point(r2.x,l2.z);
		Point R2P3 = new Point(r2.x,r2.z);

		LineSegment [] rectangle1 = {new LineSegment(R1P0,R1P1),new LineSegment(R1P0,R1P2),new LineSegment(R1P2,R1P3),new LineSegment(R1P3,R1P1)};
		LineSegment [] rectangle2 = {new LineSegment(R2P0,R2P1),new LineSegment(R2P0,R2P2),new LineSegment(R2P2,R2P3),new LineSegment(R2P3,R2P1)};
		for(int i=0;i<rectangle1.Length;i++){
			for(int j=i;j<rectangle2.Length;j++){
				if(rectangle1[i].intersect(rectangle2[j])){
					return true;
				}
			}
		}
		return false;
	}
	public class Point {
		public float x, y;
		//isValid = false should be treated as null
		public bool isValid;
		public Point (float xx,float yy){
			x=xx;
			y=yy;
			isValid=true;
		}
		public bool between(Point p, Point q) {
			return x < Mathf.Max(p.x, q.x) + EPS && x + EPS > Mathf.Min(p.x, q.x) && y < Mathf.Max(p.y, q.y) + EPS
				&& y + EPS > Mathf.Min(p.y, q.y);
		}

	}

	struct Line {
		float a, b, c;
		public Line(Point p, Point q) {
			if (Mathf.Abs(p.x - q.x) < EPS) { // Vertical Line
				a = 1.0f;
				b = 0.0f;
				c = -p.x;
			} else {
				a = -(p.y - q.y) / (p.x - q.x);
				b = 1.0f;
				c = -((p.x * a) + p.y);
			}
		}
		public Line(Point p, float m) { // Point and Slope
			a = -m;
			b = 1;
			c = -(a * p.x + p.y);
		}

		// Returns the Intersection Point between this line and l.
		public Point intersect(Line l) {
			if (areParallel(l)){
				Point tempP = new Point(0,0);
				tempP.isValid= false;
				return tempP;
			}
				
			float x = (b * l.c - c * l.b) / (a * l.b - b * l.a);
			float y;
			if (Mathf.Abs(b) < EPS)
				y = -l.a * x - l.c;
			else
				y = -a * x - c;
			return new Point(x, y);
		}
		public bool areParallel(Line l) {
			return Mathf.Abs(a - l.a) < EPS && Mathf.Abs(b - l.b) < EPS;
		}
		public bool sameLine(Line l) {
			return areParallel(l) && Mathf.Abs(c - l.c) < EPS;
		}
	}
	struct LineSegment{
		public Point p, q;
		public LineSegment(Point pp,Point qq){
			p=pp;
			q=qq;
		}
		public bool intersect(LineSegment ls) {
			Line l1 = new Line(p, q), l2 = new Line(ls.p, ls.q);
			if (l1.areParallel(l2)) {
				if (l1.sameLine(l2))
					return p.between(ls.p, ls.q) || q.between(ls.p, ls.q) || ls.p.between(p, q) || ls.q.between(p, q);
				return false;
			}
			Point c = l1.intersect(l2);
			if(c.isValid){
				return c.between(p, q) && c.between(ls.p, ls.q);
			}else{
				return false;
			}
		}
	}

	
	void ClearMap(){
		while(GeneratedMap.Count>0){
			Destroy(((GameObject)GeneratedMap[0]));
		}
		GeneratedMap.Clear();
	}
	/// <summary>
	/// Callback to draw gizmos only if the object is selected.
	/// </summary>
	void OnDrawGizmosSelected(){
		Gizmos.color =Color.red;
		if(GeneratedMap!=null)
		for(int i =0 ;i<GeneratedMap.Count;i++){
			RoadSTObject tempRSTO=(RoadSTObject)GeneratedMap[i];
				Vector3 l1 = tempRSTO.CurPosition  + Quaternion.Euler(tempRSTO.CurRotation)*tempRSTO.SizeMin;
				Vector3 r1 = tempRSTO.CurPosition  + Quaternion.Euler(tempRSTO.CurRotation)*tempRSTO.SizeMax;

			Gizmos.DrawLine(l1,r1);
		}
	}
}
