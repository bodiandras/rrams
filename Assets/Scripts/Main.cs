using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Main : MonoBehaviour {
	
	public Generator objGenerator;
	public NObject[] rams;
	public NObject[] originalRams;	
	public NObject[] goals;
	
	public List<History> history = new List<History>();	
		
	public float tileSize = 0.0250f;
	
	public GameObject[,] tiles;
	public GameObject[] fences;
	public GameObject[] scenery;
	
	public GameObject clickedObject;
	public GameObject selectedObject;
	public bool rotateCamera = false;
	public NObject selectedRam, lastMovedRam;
	
	public NAi ai;
	public RamAnimator ramAnimator;
	public NGui ramGUI;
	public GuiTextDebug debugGUI;
	public RamInput ramInput;
	
	string movementDirection;
	float cameraRotation=0;
	int[] destination;
	bool foundDestination = false;
	bool victory = false;
	
	Vector3 selectedOrigin;
	int[] selectedMapOrigin;
	
	public int[] mapSize = new int[2] {12,12};
	public Map[,] map;
	public Map[,] originalMap;
	public bool sceneLoaded = true;
	
	public int selectedLevel;
	
	
	public Utils utils = new Utils();
	
	// Use this for initialization
	public void Start () 
	{	
		
		Camera.main.transform.LookAt(new Vector3(0f,0f,0f));
		objGenerator = new Generator(this);
		ai = new NAi(this);
		selectedLevel = PlayerPrefs.GetInt("selectedLevel");
		if(selectedLevel == 0) {
			objGenerator.GenerateMap();
			objGenerator.PlaceRams();
		} else {
			objGenerator.LoadLevel(selectedLevel);			
		}		
		
		objGenerator.PlaceTiles();
		objGenerator.PlaceFences();
		
		originalMap = utils.ArrCopy(map);		
		originalRams = utils.ArrCopy(rams);		
		ramAnimator = new RamAnimator(tileSize, mapSize, objGenerator);
		
		sceneLoaded = true;
		ramGUI = new NGui(this);
		debugGUI = new GuiTextDebug();
		ramInput = new RamInput();
	}
	
	// Update is called once per frame
	void Update () {		
		ramInput.Update();
		clickedObject = ramInput.GetClickedObject();
		NObject blackRam = getRam("ramBlack");
		if(clickedObject) {
			selectedObject = clickedObject;			
			//selectedOrigin = Camera.main.WorldToScreenPoint(selectedObject.transform.position);	
			selectedOrigin = selectedObject.transform.position;
			int x = (int)Mathf.Floor(selectedObject.transform.position.x/tileSize)+ mapSize[0] / 2;
			int y = (int)Mathf.Floor(selectedObject.transform.position.z/tileSize)+ mapSize[1] / 2;
			selectedMapOrigin = new int[2]{x,y};
			int[] o = selectedMapOrigin;
			for(int i=0; i<map[o[0], o[1]].objects.Count; i++) {
				string key = map[o[0], o[1]].objects.GetKey(i).ToString();
				selectedRam = (key.Contains("ram"))?map[o[0], o[1]].objects[key] as NObject:selectedRam;
			}
		}
		if((selectedObject) && (selectedRam.name != "ram4")) {
			bool stopped = false;		
			string dir = RamInput.GetMovementDirection(selectedOrigin, cameraRotation);
			
			// only calculate new destination if direction changed
			if(dir!=movementDirection) {
				movementDirection = dir;
				ramAnimator.ClearPath();
				int i = selectedMapOrigin[0]+((dir=="left")?-1:1); 
				int j= selectedMapOrigin[1]+((dir=="down")?-1:1); 
				
			
				while(i!=((dir=="left")?-1:mapSize[0]) && (dir=="left" || dir=="right") && !stopped) {
					if (IsValidDestination(new int[2]{i,selectedMapOrigin[1]}, dir, selectedRam)) {
						foundDestination = true;
						destination = new int[2]{i,selectedMapOrigin[1]};						
					} else {						
						stopped = true;
					}
					i=(dir=="right")?i+1:i-1;
				}
				
				while(j!=((dir=="down")?-1:mapSize[1]) && (dir=="up" || dir=="down") && !stopped) {
					if (IsValidDestination(new int[2]{selectedMapOrigin[0],j}, dir, selectedRam)) {
						foundDestination = true;
						destination = new int[2] {selectedMapOrigin[0],j};					
					} else {
						stopped = true;
					}
					j=(dir=="up")?j+1:j-1;
				}
						
				if(foundDestination) {
					ramAnimator.ShowPath(selectedObject, dir, selectedMapOrigin, destination);					
				}
			}
			
			//check if object was released
			if(RamInput.InputRelease()) {				
				if((selectedObject) && (movementDirection!="") && (foundDestination)){									
					int[] o = selectedMapOrigin;
					moveRam(selectedRam, destination);				
				}
				movementDirection = null;
				selectedObject = null;
				foundDestination = false;
				ramAnimator.ClearPath();
			}
		}
		if(ramAnimator.movementFinished) {			
			// check if player has completed the map
			victory = ai.CheckVictory();
			if(victory) {
				Victory();				
			}
			
			//check if we should move the black ram			
			if((blackRam!=null) && (lastMovedRam.name != blackRam.name)) {								
				if((blackRam.name!=selectedRam.name) && ((blackRam.x!=blackRam.finalDestination[0]) || (blackRam.y!=blackRam.finalDestination[1]))) {
					int[] newPos = ai.getBlackRamPath(blackRam);
					moveRam(blackRam, newPos);
				}
			}
		}
		
		string rotDir = ramInput.Rotate();
		if(rotDir == "left") {
			Camera.main.transform.RotateAround(new Vector3(0f,0f,0f), new Vector3(0f,1f,0f), Time.deltaTime * 100f);
			cameraRotation += 100f;
		} else if (rotDir == "right") {
			Camera.main.transform.RotateAround(new Vector3(0f,0f,0f), new Vector3(0f,1f,0f), Time.deltaTime * -100f);	
			cameraRotation -=100f;
		}
	}
	
	public void moveRam(NObject ram, int[] destination, bool isUndo=false)
	{	StartCoroutine(ramAnimator.Move(ram.gameObject, destination, 10));
		string remove = "";					
		for(int i=0; i<map[ram.x, ram.y].objects.Count; i++) {						
			remove = (map[ram.x, ram.y].objects.GetKey(i).ToString().Contains("ram"))?map[ram.x, ram.y].objects.GetKey(i).ToString():remove;						
		}
				
		if(remove!="") {
			NObject ram2 = map[ram.x, ram.y].objects[remove] as NObject;						
			map[destination[0],destination[1]].objects.Add(remove, ram2);
			ram2 = map[destination[0],destination[1]].objects[remove] as NObject;
			map[ram.x, ram.y].objects.Remove(remove);			
		} else {
			//Debug.Log ("ORIGIN ["+o[0]+","+o[1]+"]" + " DESTINATION ["+destination[0]+","+destination[1]+"]");
		}
		
		ram.x = destination[0];
		ram.y = destination[1];
		
		ram.lastPosition = ram.currentPosition;
		ram.currentPosition = destination;
		
		if(!isUndo) {
			lastMovedRam = ram;
			var h = new History { ram = ram, lastPosition = ram.lastPosition };
			history.Add (h);
			ramGUI.nrMoves++;
		}
	}
	
	void OnGUI()
	{	
		ramGUI.OnGUI();
		debugGUI.OnGUI();
	}
	
	// start a new map with new random positions
	public void New()
	{
		int nrTries = 0;
		ramGUI.nrMoves = 0;
		while(!objGenerator.GenerateMap() && nrTries++<10){}
		objGenerator.PlaceFences();
		objGenerator.PlaceRams();
		originalMap =  utils.ArrCopy(map);
		originalRams = utils.ArrCopy(rams);
	}
	
	// reset the level to original position
	public void Restart()
	{
		victory = false;
		ramGUI.nrMoves = 0;
		map = null;
		map = utils.ArrCopy(originalMap);
		rams = utils.ArrCopy (originalRams);
		foreach(NObject ram in rams) {
		 	ramAnimator.Teleport(ram.gameObject, ram.originalPosition);
		}		
	}
	
	public void Undo()
	{		
		if((history.Count > 0) && (ramAnimator.movementFinished)) {
			History h = history[history.Count-1];		
			moveRam(h.ram, h.lastPosition, true);
			history.RemoveAt(history.Count-1);
			ramGUI.nrMoves--;
		}
	}
	
	//load the next level
	public void NextLevel()
	{
		victory = false;
		selectedLevel = PlayerPrefs.GetInt("selectedLevel");
		PlayerPrefs.SetInt("selectedLevel", ++selectedLevel);
		objGenerator.LoadLevel(selectedLevel);
		
	}
	
	public void Victory()
	{
		int lastCompleteLevel = PlayerPrefs.GetInt("lastCompleteLevel");
		selectedLevel = PlayerPrefs.GetInt("selectedLevel");
		if(selectedLevel>lastCompleteLevel) {
			PlayerPrefs.SetInt("lastCompleteLevel", selectedLevel);
		}
		ramGUI.showVictory = true;		
	}	
	
	/* Checks if destination can be accessed from certain direction
	 * 
	 */
	public bool IsValidDestination(int[] dest, string dir, NObject ram)
	{
		int x = dest[0]; int y =dest[1];
		switch(dir) {
			case "down" : if(map[x,y].top==1 || map[x,y+1].bottom==1) { return false; } break;
			case "up" : if(map[x,y].bottom==1 || map[x,y-1].top==1) { return false;} break;
			case "left" : if(map[x,y].right==1 || map[x+1,y].left==1) {return false; } break;
			case "right" : if(map[x,y].left==1 || map[x-1,y].right==1) { return false;} break;	
		}
		//an object is occupying the space
		if(map[x,y].objects.Count>0) {		
			NObject goal = null;
			for(int i=0; i<map[x,y].objects.Count; i++) {
				string key = map[x,y].objects.GetKey(i).ToString();
				goal = (key.Contains("goal"))?map[x,y].objects[key] as NObject:goal;				
			}			
			// only ram with same color as goal is allowed to access square
			if(goal != null) {				
				if(goal.color == ram.color) { // ram same color as goal, allow
					return true;
				} else { // ram is not the same color as the goal, deny
					return false;
				}
			} else { // it is not a goal, so probably a ram
				return false;
			}
		}
		return true;
	}
	
	public NObject getRam(string color) 
	{
		foreach(NObject ram in rams) {
		 	if(ram!=null) {			
				if(ram.color == color) {
					return ram;
				}
			}
		}
		return null;
	}		
}