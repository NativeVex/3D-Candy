using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System; 

public class Grid : MonoBehaviour
{
	//user input
	public int[] dim = new int[3];
	public GameObject[] Prefabs;
	public int spacing;

	public static GameObject[] swapQue = new GameObject[2];
	public static int StaticSpacing;
	public static GameObject[,,] Sto;

	public void swapManager(){
		if (swapQue [0].tag == swapQue [1].tag) {
			swapQue[0] = null;
			swapQue [1] = null;
			return;
		}
		GameObject actualFirst = swapQue[0], actualSecond =swapQue[1];
		GameObject first = Instantiate(actualFirst), second =Instantiate(actualSecond);
		Transform firstT = first.transform;
		Transform secondT = second.transform;
		int[] firstCoords = new int[]{(int)(first.gameObject.transform.position.x / StaticSpacing),
			(int)(first.gameObject.transform.position.y / StaticSpacing),
				(int)(first.gameObject.transform.position.z / StaticSpacing)
		};
		int[] secondCoords = new int[]{(int)(second.gameObject.transform.position.x / StaticSpacing),
			(int)(second.gameObject.transform.position.y / StaticSpacing),
			(int)(second.gameObject.transform.position.z / StaticSpacing)
		};
		Sto [firstCoords [0], firstCoords [1], firstCoords [2]] = actualSecond;
		Sto [secondCoords [0], secondCoords [1], secondCoords [2]] = actualFirst;
		actualFirst.transform.position = new Vector3(secondT.position.x,secondT.position.y,secondT.position.z);
		actualSecond.transform.position = new Vector3(firstT.position.x,firstT.position.y,firstT.position.z);
		swapQue[0] = null;
		swapQue[1] =null;
		first.SetActive(false);
		second.SetActive(false);
	}

	// Use this for initialization
	void Start (){
		StaticSpacing = spacing;
		Sto = new GameObject[dim[0],dim[1],dim[2]];
		populateSto ();
	}
		
	// Update is called once per frame
	void Update ()
	{
		RunDels(); 
		SimGrav ();
		if (!ReGen ()) { //TODO enable this testing
			//print ("done");
		}
		if (swapQue[1] != null) {
			swapManager ();
			print ("called");
		}
	}

	void SimGrav(){
		for (int z = 0; z < dim [2]; z++) {
			for (int y = 1; y < dim [1]; y++) {//y starts at 1 to ignore errors //TODO testing
				for (int x = 0; x < dim [0]; x++) {
					//if (Sto [x, y, x] != null) {
					try{
						bool a = Sto[x,y,z].activeSelf; //taking the error

//						if Sto [x, y, z].activeSelf == true && (Sto [x, y - 1, z].activeSelf == false || Sto [x, y - 1, z] == null)) {
//							print ("Moved " + Sto [x, y, z].name + " to " + x * StaticSpacing + "," + (y - 1) * StaticSpacing + "," + z * StaticSpacing);
//							Vector3 newPos = new Vector3 (x * StaticSpacing, (y - 1) * StaticSpacing, z * StaticSpacing);
//							Sto [x, y, z].transform.position = newPos;
//						}
					}catch(MissingReferenceException){
						//print (e);
						continue;
						//an actual problem < not really, could be an empty space. Keep calm and continue on
					}catch(NullReferenceException){//3rd time
						continue;
					}
					try{
						if(Sto[x,y-1,z].activeSelf == false){
							throw new MissingReferenceException(); //if you won't throw it, I will
						}

					}catch(MissingReferenceException){ //that's fine.
					//	print ("Moved " + Sto [x, y, z].name + " to " + x * StaticSpacing + "," + (y - 1) * StaticSpacing + "," + z * StaticSpacing);
						Sto[x,y,z].transform.position = new Vector3(x*StaticSpacing,(y-1)*StaticSpacing,z*StaticSpacing);
						GameObject temp = Sto [x, y, z];
						Sto [x, y, z] = null;
						Sto [x, y - 1, z] = temp;

					}catch(NullReferenceException){ //because mono is inherently retarded 
					//	print ("Moved " + Sto [x, y, z].name + " to " + x * StaticSpacing + "," + (y - 1) * StaticSpacing + "," + z * StaticSpacing);
						Sto[x,y,z].transform.position = new Vector3(x*StaticSpacing,(y-1)*StaticSpacing,z*StaticSpacing);
						GameObject temp = Sto [x, y, z];
						Sto [x, y, z] = null;
						Sto [x, y - 1, z] = temp;
					}
				}
			}
		}

//		foreach (GameObject temp in Sto) {
//			int localX = (int)(temp.transform.position.x/StaticSpacing);
//			int localY = (int)(temp.transform.position.y/StaticSpacing);
//			int localZ = (int)(temp.transform.position.z/StaticSpacing);
//			try{
//				if(Sto[localX,localY-1,localZ].activeSelf == false){
//					temp.transform.position = new Vector3 (localX*StaticSpacing, (localY - 1)*StaticSpacing, localZ*StaticSpacing);
//				}
//			}catch(IndexOutOfRangeException e){
//				print (e);
//			}
//		}
	}

	void RunDels(){
		delMatches ("x",dim[0],dim[1],dim[2]);
		delMatches ("y",dim[1],dim[0],dim[2]); //TODO disabled for testing
		delMatches ("z",dim[2],dim[0],dim[1]);
	}

	bool ReGen(){//TODO do regen
		for (int z = 0; z < dim [2]; z++) {
			for (int x = 0; x < dim [0]; x++) {
				try{
					if(Sto[x,dim[1]-1,z].activeSelf == false){
						throw new MissingReferenceException();
					}
				}catch(MissingReferenceException){
					GameObject rand = RandCand ();
					rand.transform.position = new Vector3 (x * StaticSpacing, (dim[1]-1) * StaticSpacing, z * StaticSpacing); //TODO TESTING
					Sto [x, dim [1]-1, z] = Instantiate (rand);
					return true;
				}catch(NullReferenceException){
					GameObject rand = RandCand ();
					rand.transform.position = new Vector3 (x * StaticSpacing, (dim[1]-1) * StaticSpacing, z * StaticSpacing);//TODO TESTING
					Sto [x, dim [1]-1, z] = Instantiate (rand);
					return true;
				}
			}
		}
		return false;
	}

	void delMatches(string axis,int primaryDirLimit, int secondaryDirLimit, int tetriaryDirLimit){
		for (int tetriaryDir = 0; tetriaryDir < tetriaryDirLimit; tetriaryDir++) {
			for (int secondaryDir = 0; secondaryDir < secondaryDirLimit; secondaryDir++) {
				List<GameObject> inLine = new List<GameObject> (3);
				for (int primaryDir = 0; primaryDir < primaryDirLimit; primaryDir++) {
					if (inLine.Count < 3) {
						inLine.Add (AtLoc(axis,primaryDir,secondaryDir,tetriaryDir)); //was sto[x,0,0]
					} else {
						inLine.RemoveAt (0);
						inLine.Add (AtLoc(axis,primaryDir,secondaryDir,tetriaryDir)); //was sto[x,0,0]
					}
					//eval
					eval (inLine);
				}
			}
		}
	}

	GameObject AtLoc(string axis,int primaryDir, int secondaryDir, int tetriaryDir){
		if (axis.Equals ("x")) {
			return Sto [primaryDir, secondaryDir, tetriaryDir];
		} else if (axis.Equals ("y")) {
			return Sto [secondaryDir, primaryDir, tetriaryDir];
		} else if (axis.Equals ("z")) {
			return Sto [secondaryDir, tetriaryDir, primaryDir];
		} else {
			throw new AxisNotFoundException();
		}
	}

	void eval(List<GameObject> inLine){
		if (inLine.Count == 3) {
			bool allSame = true;
			foreach (GameObject temp in inLine) {
				try{
				//if (!(temp.Equals(null)||temp == null)) {
					if (!(temp.tag == inLine.ToArray () [0].tag)) {//if tags are different, report problem
						allSame = false;
					}
//				} else {
//					allSame = false;
//				}
				}catch(MissingReferenceException){
					allSame = false;
				}catch(NullReferenceException){//YAY FOR USING C# 4 AND NOT 6
					allSame = false;
				}
			}
			if (allSame) {
				for (int i = 0; i < inLine.Capacity; i++) {
					inLine [i].SetActive (false); //TODO add particle effects
					Destroy(inLine[i].gameObject);

				}
				inLine.Clear ();
			}
		}
	}

//	public static Tuple<int, int, int> CoordinatesOf<GameObject>(this GameObject[,,] Sto, GameObject value)
//	{
//		for (int x = 0; x < dim[0]; ++x)
//		{
//			for (int y = 0; y < dim[1]; ++y)
//			{
//				for (int z = 0; z < dim[2]; z++) {
//					if (Sto [x, y,z].Equals (value))
//						return Tuple.Create (x, y,z);
//				}
//			}
//		}
//
//		return Tuple.Create(-1, -1,-1);
//	}

	void populateSto(){
		for (int z = 0; z <  dim [2]; z++) {
			for (int y =0; y <  dim [1]; y++) {
				for (int x =0; x < dim[0]; x++) {
					GameObject rand = RandCand ();
					rand.transform.position = new Vector3 (x*spacing, y*spacing, z*spacing);
					Sto [x, y, z] = Instantiate(rand);
				}
			}
		}
	}

	GameObject RandCand(){
		return Prefabs [(int)((UnityEngine.Random.value * Prefabs.Length)-.1)];
	}
}