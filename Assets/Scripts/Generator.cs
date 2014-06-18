using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.IO;

public class Generator 
{
	/* Creates a new instance for your prefab. */
    public Main main;
	public string[] ramColors;
	private int nrFences = 0;
	public Vector3 normalScale = new Vector3(1f, 1f, 1f);
	public Vector3 ramScale = new Vector3(0.3f, 0.3f, 0.3f);
	public Generator(Main main)
	{
		this.main = main;
		ramColors = new string[5];		
		ramColors[0] = "ramGray";
		ramColors[1] = "ramRed";
		ramColors[2] = "ramBlue";
		ramColors[3] = "ramYellow";
		ramColors[4] = "ramBlack";
	}
	
	public GameObject instance(string strObject, Vector3 spawnLocation, Vector3 scale, float transparency = 1f)
    {	
		// Load a GameObject that exist inside the "Resources" folder.
        GameObject prefab = (GameObject)Resources.Load(strObject);
        // Create an instance of the prefab
		GameObject instance = (GameObject)GameObject.Instantiate(prefab);		
        instance.transform.position = spawnLocation;
		instance.transform.localScale = scale;		
			
		if(!(new List<float>{0f, 1f}).Contains(transparency)) {					
			if (instance.renderer.enabled) {				
					instance.renderer.material.shader = Shader.Find("Transparent/Diffuse");
	                Color color = instance.renderer.material.color;
	    			color.a = transparency;
	    			instance.renderer.material.color = color;						
			}
		}
		return instance;
    }
	
	public GameObject instance(GameObject gameObject, Vector3 spawnLocation, Vector3 scale)
	{
		GameObject instance = (GameObject)GameObject.Instantiate(gameObject);		
        instance.transform.position = spawnLocation;
		instance.transform.localScale = scale;
		return instance;
	}
	
	public void ClearFences(GameObject[] fences)
	{
		nrFences = 0;
		foreach(GameObject fence in fences) {
    		Object.Destroy(fence);
		}
    }
	
	public void ClearRams(NObject[] rams)
	{
		if(rams==null) { return; }
		foreach(NObject ram in rams) {
			if(ram!=null) {
    			Object.Destroy(ram.gameObject);
			}
		}
    }
	
	public void ClearGoals(NObject[] goals)
	{
		if(goals==null) { return; }
		foreach(NObject goal in goals) {
			if(goal!=null) {				
    			Object.Destroy(goal.gameObject);
			}
		}
    }
	
	/* Generates a random map
	 */
	
	public bool GenerateMap()
	{
		this.ClearGoals(main.goals);
		main.goals = new NObject[1];
		
		ClearFences(main.fences);
		main.fences = new GameObject[100];
		
		main.map = new Map[main.mapSize[0], main.mapSize[1]];
		for(int i=0; i<main.mapSize[0];i++) {
			for( int j=0; j<main.mapSize[1]; j++) {
				main.map[i,j] = new Map();
				main.map[i,j].objects = new SortedList();
			}
		}		
		//place 2 fences on top		
		main.map[Random.Range (2, main.mapSize[0] / 2), main.mapSize[1] - 1].right = 1;		
		main.map[Random.Range (main.mapSize[0] / 2, main.mapSize[0] - 2), main.mapSize[1] - 1].right = 1;
		//place 2 blocks on right
		main.map[main.mapSize[0] - 1, Random.Range (2, main.mapSize[1] / 2)].top = 1;
		main.map[main.mapSize[0] - 1, Random.Range (main.mapSize[1] / 2 , main.mapSize[1] - 2)].top = 1;		
		//place 2 blocks on bottom
		main.map[Random.Range (1,main.mapSize[0] / 2),0].right = 1;
		main.map[Random.Range (main.mapSize[0] / 2, main.mapSize[0] - 2),0].right = 1;
		//place 2 blocks on left
		main.map[0, Random.Range (1, main.mapSize[1] / 2)].top = 1;
		main.map[0, Random.Range (main.mapSize[1] / 2, main.mapSize[1] - 2)].top = 1;
		
		//place blocks in center
		//top
		main.map[main.mapSize[0] / 2 - 1, main.mapSize[1] / 2 ].top = 1;
		main.map[main.mapSize[0] / 2 , main.mapSize[1] / 2 ] .top = 1;
		//right
		main.map[main.mapSize[0] / 2, main.mapSize[1] / 2 -1].right = 1;
		main.map[main.mapSize[0] / 2, main.mapSize[1] / 2].right = 1;
		//bottom
		main.map[main.mapSize[0] / 2, main.mapSize[1] / 2 - 1].bottom = 1;
		main.map[main.mapSize[0] / 2 - 1, main.mapSize[1] / 2 - 1].bottom = 1;
		//left
		main.map[main.mapSize[0] / 2 - 1, main.mapSize[1] / 2].left = 1;
		main.map[main.mapSize[0] / 2 - 1, main.mapSize[1] / 2 - 1].left = 1;		
		
		//place 2 2-side blocks in each quarter, try to place target marker
		int m = Random.Range (1,9), k = 0;
		for(int i=0; i<4; i++) {
			for(int j=0; j<2; j++) {
				if(k>=m && m!=0) {
					if (PlaceDestination(i,j,true)) {
						m=0;
					}
				} else {
					PlaceDestination(i,j);
				}	
				k++;
			}
		}	
		
		if(main.goals[0]==null) {
			return false;
		}
		
		return true;
	}
	
	/* Loads a pregenerated level
	 */
	public bool LoadLevel(int mapNr)
	{
		this.ClearGoals(main.goals);
		main.goals = new NObject[1];
		
		ClearFences(main.fences);
		main.fences = new GameObject[100];		
			
		ClearRams(main.rams);
		main.rams = new NObject[5];		
		
		
		main.map = new Map[main.mapSize[0], main.mapSize[1]];
		for(int i=0; i<main.mapSize[0];i++) {
			for( int j=0; j<main.mapSize[1]; j++) {
				main.map[i,j] = new Map();
				main.map[i,j].objects = new SortedList();
			}
		}
		
		XmlReader reader;		
		reader = XmlReader.Create(new System.IO.StringReader(main.utils.LoadXml("map_" + mapNr)));		
		int n = 0, k = 0;
		while (reader.Read()) {
			switch(reader.Name) {
			case "ram":											
				k = main.utils.FindKey(ramColors,reader.GetAttribute("color"));				
				main.rams[k] = PlaceRam(k, System.Int32.Parse(reader.GetAttribute("x")), System.Int32.Parse(reader.GetAttribute("y")) );
				main.map[main.rams[k].x, main.rams[k].y].objects.Add("ram"+k,main.rams[k]);
				n++;
				break;
			case "goal":
				k = main.utils.FindKey(ramColors,reader.GetAttribute("color")); 
				PlaceGoal(k, System.Int32.Parse(reader.GetAttribute("x")), System.Int32.Parse(reader.GetAttribute("y")));
				break;
			case "fence":
				main.map[System.Int32.Parse(reader.GetAttribute("x")), System.Int32.Parse(reader.GetAttribute("y"))].Set(reader.GetAttribute("direction"),1);
				break;
			case "scenery":				
				Vector3 pos = new Vector3(System.Int32.Parse(reader.GetAttribute("x")), System.Int32.Parse(reader.GetAttribute("y")), System.Int32.Parse(reader.GetAttribute("z")));
				float s = System.Convert.ToSingle(reader.GetAttribute("scale"));
				Vector3 scale = new Vector3(s,s,s);
				float transparency = System.Convert.ToSingle(reader.GetAttribute("transparency"));
				GameObject scenery = instance(reader.GetAttribute("name"), pos, scale, transparency);				
				break;
			case "ramDestination":
				k = main.utils.FindKey(ramColors,reader.GetAttribute("color"));
				NObject ram = main.rams[k];
				ram.finalDestination = new int[] { System.Int32.Parse(reader.GetAttribute("x")), System.Int32.Parse(reader.GetAttribute("y")) };
				break;
			}
		}
			PlaceFences();
		return true;
	}
	
	public void PlaceRams() 
	{			
		ClearRams(main.rams);
		main.rams = new NObject[5];					
				
		for(int i=0; i<4; i++) {
			main.rams[i] = PlaceRam(i);		
			main.map[main.rams[i].x, main.rams[i].y].objects.Add("ram"+i,main.rams[i]);
		}
	}
	
	public NObject PlaceRam(int i, int x = -1, int y = -1)
	{
		// position ram randomly
		if(x == -1) {			
			do {
				x = Random.Range(0, main.mapSize[0]);			
				y = Random.Range(0, main.mapSize[1]);			 		
			} while((main.map[x,y].objects.Count>0) || ((x >= main.mapSize[0] /2  - 1) && (x <= main.mapSize[0] /2)) || ((y >= main.mapSize[1] / 2 - 1) && (y <= main.mapSize[1] / 2)) );				
		}
		GameObject ram;
			
		ram = instance("ram_001" ,new Vector3(((x - main.mapSize[0] / 2)*main.tileSize)+0.01f, 0.01f, ((y - main.mapSize[1] / 2)*main.tileSize)+0.01f), ramScale);
		ram.transform.RotateAround(Vector3.up, Random.Range(0f,360f));
			
			
		ram.AddComponent("MeshRenderer");
		if (ram.renderer.enabled) {							
			Transform[] allChildren = ram.GetComponentsInChildren<Transform>();
			foreach (Transform child in allChildren) {
				if(child.name=="ramBody" || child.name=="ramBody2") {
					child.gameObject.renderer.material = (Material)Resources.Load(ramColors[i], typeof(Material));
				}
				if(child.name=="pCone1" || child.name=="pCone2") {
					child.gameObject.renderer.material = (Material)Resources.Load("ram_horns", typeof(Material));
				}
			}							
		}
		//add collider
		SphereCollider sc;
		sc = ram.AddComponent("SphereCollider") as SphereCollider;
		sc.radius = 0.042f;
		sc.center= new Vector3(0f,0.03f,0.02f);
		
		NObject nORam = new NObject(ram, "ram"+i, new int[] {x,y}, ramColors[i]);		
		nORam.x = x;
		nORam.y = y;
		return nORam;
	}
	
	bool PlaceDestination(int quarter, int color, bool isGoal =  false)
	{			
		// 0 left+top ; 1 top-right ; 2 right-bottom; 3 bottom-right
		int orientation = 0;		
		int x, y, nrTries = 0;	
		do {
			switch (quarter) {
    			case 0:
        			x = Random.Range(1, main.mapSize[0] / 2); y = Random.Range(1, main.mapSize[1] / 2);
        		break;
    			case 1:
        			x = Random.Range(main.mapSize[0] / 2 + 1 , main.mapSize[0] - 1); y = Random.Range(1, main.mapSize[1] / 2 + 1);
        		break;
    			case 2:
					x = Random.Range(main.mapSize[0] / 2 + 1, main.mapSize[0] - 1); y = Random.Range( main.mapSize[1] / 2 +1 , main.mapSize[1] - 1);
				break;
				case 3:
					x = Random.Range(1, main.mapSize[0] / 2); y = Random.Range(main.mapSize[1] / 2 , main.mapSize[1] - 1);
				break;
				default:
					x = 1; y = 1;
				break;
			}
			// check if place not occupied and not near other object
			if (nrTries++ > 100) {
				return false;
			}
		}while(!CheckPosition(x,y));
		
		if(isGoal) {					
			PlaceGoal(Random.Range(0,4),x,y);	
		}
		orientation = Random.Range(1,4);
		switch(orientation) {
		case 0: main.map[x,y].left = 1; main.map[x,y].top = 1;
			break;				
		case 1: main.map[x,y].top = 1; main.map[x,y].right = 1;
			break;
		case 2: main.map[x,y].right = 1; main.map[x,y].bottom = 1;
			break;
		case 3: main.map[x,y].bottom = 1; main.map[x,y].left = 1;
			break;
		}		
		
		return true;
	}
	
	/** i : the color of destination
	 * 
	}*/
	public void PlaceGoal(int i,int x, int y) {
		GameObject go = this.instance("sphere_001_prefab" ,new Vector3(((x - main.mapSize[0] / 2) * main.tileSize)+0.01f, 0.015f, ((y - main.mapSize[1] / 2)* main.tileSize )+0.01f), normalScale);
		go.renderer.material = (Material)Resources.Load(ramColors[i], typeof(Material));
		main.goals[0] = new NObject(go, "goal0", new int[] {x,y}, ramColors[i]);
		main.map[x,y].objects.Add("goal0",main.goals[0]);
	}
	/** Checks on position and in perimeter of x for obstacles
	 * 
	 */
	bool CheckPosition(int i, int j, int x = 0)
	{
		x = (x!=0)?x:1;
				
		for(int k = Mathf.Max(0,i-x); k<=Mathf.Min(i+x, main.mapSize[0] - 1); k++) {
			for(int l = Mathf.Max(0,j-x); l<=Mathf.Min(j+x, main.mapSize[1] - 1); l++) {
				if (main.map[k,l].right==1 || main.map[k,l].top==1 || main.map[k,l].left==1 || main.map[k,l].bottom==1) {
					return false;
				}
			}
		}
		
		return true;		
	}
	
	public void PlaceTiles()
	{		
		main.tiles = new GameObject[main.mapSize[0],main.mapSize[1]];
		for(int i=0; i<main.mapSize[0]; i++) {
			for(int j=0; j<main.mapSize[1]; j++) {
				int k = Random.Range(1,5);
				main.tiles[i,j] = instance("tile_001" ,new Vector3(((i- main.mapSize[0]/2)*main.tileSize)+0.01f, 0.0096f, ((j- main.mapSize[1]/2)*main.tileSize)+0.01f), normalScale);
				main.tiles[i,j].renderer.material = (Material)Resources.Load("grass_"+k, typeof(Material));
		
				main.tiles[i,j].renderer.material.mainTextureScale = new Vector2(3.9f, 3.9f);
				main.tiles[i,j].renderer.material.mainTextureOffset = new Vector2(0.55f, 0.02f);
			}
		}
	}
	
	
	//will visually place fences as they are in the generated map objectge
	public void PlaceFences()
	{		
		for(int i=0; i<main.mapSize[0]; i++) {
			for( int j=0; j<main.mapSize[1]; j++) {
				if(main.map[i,j].top==1) {
					PlaceFence("top", i, j);									
				}
				if(main.map[i,j].right==1) {
					PlaceFence("right", i, j);				
				}
				if(main.map[i,j].bottom==1) {
					PlaceFence("bottom", i, j);
				}
				if(main.map[i,j].left==1) {
					PlaceFence("left", i, j);					
				}			
			}
		}
	}

	
	public void PlaceFence(string dir, int x, int y)
	{
		int k = nrFences++;
		switch(dir) {
		case "top":			
			main.fences[k] = instance("fence_001" ,new Vector3(((x - main.mapSize[0] / 2 )*main.tileSize)+0.01f, 0.011f, ((y - main.mapSize[1] / 2 + 0.5f)*main.tileSize)+0.01f), normalScale);				
			break;
		case "right" :			
			main.fences[k] = instance("fence_001" ,new Vector3(((x - main.mapSize[0] / 2 + 0.5f)*main.tileSize)+0.01f, 0.011f, ((y - main.mapSize[1] / 2)*main.tileSize)+0.01f), normalScale);
			main.fences[k].transform.Rotate(Vector3.up, 90f);
			break;
		case "bottom":			
			main.fences[k] = instance("fence_001" ,new Vector3(((x - main.mapSize[0] / 2)*main.tileSize)+0.01f, 0.011f, ((y - main.mapSize[1] / 2 - 0.5f)*main.tileSize)+0.01f), normalScale);					
			main.fences[k].transform.Rotate(Vector3.up, 180f);
			break;
		case "left":			
			main.fences[k] = instance("fence_001" ,new Vector3(((x - main.mapSize[0] / 2 - 0.5f)*main.tileSize)+0.01f, 0.011f, ((y - main.mapSize[1] / 2 )*main.tileSize)+0.01f), normalScale);
			main.fences[k].transform.Rotate(Vector3.up, -90f);
			break;
		}
	}

}
