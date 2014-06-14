using System;
using UnityEngine;

public class NGui
{
	public int nrMoves = 0;
	private Rect windowRect = new Rect (0, 0, (int)Screen.width , (int)Screen.height);
	private Rect normalCenterWindow = new Rect((int)(Screen.width *0.25f),(int)(Screen.height*0.5f - Screen.width*0.15f), (int)(Screen.width *0.5f) ,(int)(Screen.width *0.2f) ); 
	private bool showGameMenu = false;
	private bool showScenarios = false;
	
	public bool showVictory = false;
	
	Main main;
	//private MainMenu mainMenu = null;
	float w, h, oW=1920, oH=1080, sizeGui = 1f, sizeGui_small = 0.6f;
	private float guiRatioX, guiRatioY, guiRatiotX, guiRatiotY;
	
	private Vector3 GUInF, GUIsF, GUItF;
	
	
	public GUISkin mySkin, nextSkin, restartSkin, newSkin, undoSkin, mainMenu, mainMenuDisabled;
	
	public NGui(Main main)
	{
		this.main = main;
		
		this.h = Screen.height;
		this.w = Screen.width;
		guiRatioX = w/oW;
    	guiRatioY = h/oH;
		
		guiRatiotX = w / 0.5f;
		guiRatiotY = h / 0.5f;	
		GUInF = new Vector3(1f,1f,1f); // normal		
		GUIsF = new Vector3(0.75f,0.75f,0.75f); // small
		GUItF = new Vector3(0.5f,0.5f,0.5f); // tiny
		
		
		nextSkin = Resources.Load("next_button") as GUISkin;
		restartSkin = Resources.Load("restart_button") as GUISkin;
		newSkin = Resources.Load ("new_button") as GUISkin;
		undoSkin = Resources.Load ("undo_button") as GUISkin;
	}
	
	public NGui(MainMenu mainMenu)
	{
		//this.mainMenu = mainMenu;
		this.h = Screen.height;
		this.w = Screen.width;		
		
		guiRatioX = w/1920 * sizeGui;
    	guiRatioY = h/1080 * sizeGui;
		
		Debug.Log (w + " - " +guiRatioX);
		GUIsF = new Vector3(guiRatioX,guiRatioY,1);
		
	}
	
	public void OnGUI() 
	{		
		GUIStyle style = new GUIStyle();
		style.fontSize = 20;	
		
			
		
		if(!mySkin) {
			mySkin = Resources.Load("main_menu") as GUISkin;
		}
		GUI.skin = mySkin;
		GUI.skin.button.fontSize = 20;
		//GUI.matrix = Matrix4x4.TRS(new Vector3(GUIsF.x,GUIsF.y,0),Quaternion.identity,GUIsF);
		
		
		//Undo
		GUI.skin = undoSkin;
		if(GUI.Button(new Rect(w * 0.5f + 40, h-75 , 54, 75), "")) {
			Application.Quit();
		}
		
		//GUI.matrix = Matrix4x4.TRS(new Vector3(GUItF.x,GUItF.y,0),Quaternion.identity,GUItF);
		
		//Restart
		GUI.skin = restartSkin;
		if(main.selectedLevel==0) {
			if(GUI.Button(new Rect(w * 0.5f + 110, h - 75 , 84, 75), "")) {
				main.Restart();
			}
		}
		
		//New
		GUI.skin = newSkin;		
		if(main.selectedLevel==0){
			if(GUI.Button(new Rect(w * 0.5f + 215, h - 75 , 44, 75), "")) {			
				main.New();
			}
		}
		
		GUI.Label (new Rect(w*0.5f - 360, h-30, 100, 50), "Moves: " + nrMoves, style );
		
		/*
		GUI.skin = null;		
		if(GUI.Button(new Rect((int)Screen.width*0.9f, (int)Screen.height*0.1f, 80, (int)Screen.height*0.05f), "Menu")) {
			showGameMenu = true;
		}*/
		
		if (showGameMenu) {
			windowRect = GUI.Window (0, windowRect, GameMenu, "My Window");
		}
		
		if(showVictory) {
			if(!mySkin) {
				mySkin = Resources.Load("main_menu") as GUISkin;
			}
			GUI.skin = mySkin;
			windowRect = GUI.Window (0, normalCenterWindow, Victory, "");
		}
		
	
		
		
	}
	
	public void GameMenu(int windowID)
	{
		if(GUI.Button(new Rect((int)Screen.width*0.5f, (int)Screen.height*0.5f, 80,(int)Screen.height*0.05f), "New")) {
			main.New();
		}
	}
	
	public void Victory(int windowID)
	{
		
		float w = normalCenterWindow.width;
		float h = normalCenterWindow.height;
		
		GUI.Label(new Rect(55, 20, 150, 150), "Points");
		
		
		//Restart
		GUI.skin = restartSkin;
		if(GUI.Button(new Rect((int)(w*0.1f), (int)(h*0.75f), (int)(w*0.26f), (int)(w*0.26f / 5)), "")) {
			main.Restart();
			showVictory = false;
		}
		
		//Next		
		GUI.skin = nextSkin;
		if(GUI.Button(new Rect((int)(w*0.6f), (int)(h*0.7f), (int)(w*0.26f), (int)(w*0.26f / 3)), "" )) {
			main.NextLevel();
			showVictory = false;
		}
		
	}
		
	public bool MainMenu()
	{
		if(!mySkin) {
			mySkin = Resources.Load("main_menu") as GUISkin;
		}
		GUI.skin = mySkin;
		GUI.matrix = Matrix4x4.TRS(new Vector3(GUIsF.x,GUIsF.y,0),Quaternion.identity,GUIsF);
		
		if(showScenarios) {
			Scenarios();
			return true;
		}
		
		if(GUI.Button(new Rect(100, 150, 580, 90), "SCENARIOS")) {
			showScenarios = true;
		}
		
		if(GUI.Button(new Rect(100, 350, 720, 90), "RANDOM MAP")) {
			PlayerPrefs.SetInt("selectedLevel", 0);
			Application.LoadLevel("scene1");
		}
		
		if(GUI.Button(new Rect(100, 550, 580, 90), "EXIT GAME")) {
			Application.Quit();
		}
		return true;
	}
	
	public void Scenarios()
	{	
		if(!mainMenu) {
			mainMenu = Resources.Load("main_menu") as GUISkin;
		}
		if(!mainMenuDisabled) {
			mainMenuDisabled = Resources.Load ("main_menu_disabled") as GUISkin;
		}
		
		int lastCompleteLevel = PlayerPrefs.GetInt ("lastCompleteLevel");
		
		GUI.skin = mainMenu;
		GUI.Label(new Rect( 140 , 140 , 400, 120), "Chapter 1");
		Scenarios_ShowLevels(1, 6, 140);		
		
		GUI.skin = mainMenu;
		GUI.Label(new Rect( 140 , 440 , 400, 120), "Chapter 2");
		Scenarios_ShowLevels(7, 12, 440);
		
		GUI.skin = mainMenu;
		GUI.Label(new Rect( 140 , 740 , 400, 120), "Chapter 3");
		Scenarios_ShowLevels(13, 18, 740);
	}
	
	public void Scenarios_ShowLevels(int from, int to , int y)
	{		
		int lastCompleteLevel = PlayerPrefs.GetInt ("lastCompleteLevel");
		if(!mainMenu) {
			mainMenu = Resources.Load("main_menu") as GUISkin;
		}
		if(!mainMenuDisabled) {
			mainMenuDisabled = Resources.Load ("main_menu_disabled") as GUISkin;
		}
		
		int l = 1, k = 1;
		for(int i= from; i <= to; i++) {
			if(lastCompleteLevel>=i-1) {
				GUI.skin = mainMenu;
				if(GUI.Button(new Rect((k-1) * 140 + 25, l * y + 82, 125, 120), i.ToString()   )) {
				PlayerPrefs.SetInt("selectedLevel", i);
				Application.LoadLevel("scene1");
			}
			} else {
				GUI.skin = mainMenuDisabled;
				GUI.Label(new Rect((k-1) * 140 + 40, l * y + 100, 125, 120), i.ToString());
			}
			
			if(k++ == 6) {
				k = 1;
				l++;
			}
		}
	}

}