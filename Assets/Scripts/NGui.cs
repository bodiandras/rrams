using System;
using UnityEngine;

public class NGui
{
	public int nrMoves = 0;
	private Rect windowRect = new Rect (0, 0, (int)Screen.width , (int)Screen.height);
	private Rect normalCenterWindow = new Rect((int)(Screen.width *0.25f),(int)(Screen.height*0.5f - Screen.width*0.15f), (int)(Screen.width *0.5f) ,(int)(Screen.width *0.2f) ); 	
	private bool showGameMenu = false , showPauseMenu = false;
	private bool showScenarios = false;
	
	public bool showVictory = false;
	
	Main main;
	//private MainMenu mainMenu = null;
	float w, h, oW=1920, oH=1080, sizeGui = 1f, sizeGui_small = 0.6f;
	private float guiRatioX, guiRatioY, guiRatiotX, guiRatiotY;
	
	private Vector3 GUInF, GUIsF, GUItF;
	
	
	public GUISkin mainSkin, menuSkin, nextSkin, restartSkin, newSkin, undoSkin, mainMenuDisabled;
	
	public GUISkin pauseSkin, menuResumeSkin, menuMainMenuSkin;
	
	public GUISkin woodenPanel;
	
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
		
		mainSkin = Resources.Load("main_menu") as GUISkin;
		woodenPanel = Resources.Load("wooden_panel") as GUISkin;
		
		pauseSkin = Resources.Load ("pause_menu") as GUISkin;
		menuResumeSkin = Resources.Load ("menu_resume") as GUISkin;
		menuMainMenuSkin = Resources.Load ("menu_main_menu") as GUISkin;
			
		menuSkin = Resources.Load ("menu_button") as GUISkin;		
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
		GUIsF = new Vector3(guiRatioX,guiRatioY,1);	
	}
	
	public void OnGUI() 
	{		
		GUIStyle style = new GUIStyle();
		style.fontSize = 30;			
		
		// PauseMenu
		GUI.skin = menuSkin;
		if(GUI.Button(new Rect(w  - 80, 20 , 58, 75), "")) {
			showPauseMenu = true;
		}
		
		// Nr Moves
		GUI.skin = mainSkin;
		GUI.skin.button.fontSize = 20;
		GUI.Label (new Rect(w*0.5f - 360, h-50, 100, 50), "Moves: " + nrMoves, style );
		
		//Undo
		GUI.skin = undoSkin;
		if(GUI.Button(new Rect(w * 0.5f + 40, h-75 , 54, 75), "")) {
			main.Undo();
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
		
		/*
		GUI.skin = null;		
		if(GUI.Button(new Rect((int)Screen.width*0.9f, (int)Screen.height*0.1f, 80, (int)Screen.height*0.05f), "Menu")) {
			showGameMenu = true;
		}*/
		
		if (showGameMenu) {
			windowRect = GUI.Window (0, windowRect, GameMenu, "My Window");
		}
		
		if(showVictory) {
			if(!mainSkin) {
				mainSkin = Resources.Load("main_menu") as GUISkin;
			}
			GUI.skin = mainSkin;
			windowRect = GUI.Window (0, normalCenterWindow, Victory, "");
		}
		
		if(showPauseMenu) {
			GUI.skin = pauseSkin;
			windowRect = GUI.Window (0, new Rect((int)(Screen.width * 0.5f - 149),(int)(Screen.height * 0.5f - 200), 299 ,400), PauseMenu, "");
		}
		
		
	}
	public void PauseMenu(int windowID)
	{
		GUI.skin = menuResumeSkin;
		if(GUI.Button(new Rect(30, 262, 224, 71), "")) {
			showPauseMenu = false;
		}
		
		GUI.skin = menuMainMenuSkin;
		if(GUI.Button(new Rect(30, 154, 224, 71), "")) {
			Application.LoadLevel("mainmenu");
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
		if(!mainSkin) {
			mainSkin = Resources.Load("main_menu") as GUISkin;
		}
		GUI.skin = mainSkin;
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
		if(!mainMenuDisabled) {
			mainMenuDisabled = Resources.Load ("main_menu_disabled") as GUISkin;
		}
		if(!woodenPanel) {
			woodenPanel = Resources.Load ("wooden_panel") as GUISkin;
		}
		
		int lastCompleteLevel = PlayerPrefs.GetInt ("lastCompleteLevel");
		
		
		GUI.skin = woodenPanel;		
		GUI.Label(new Rect(50, 30, 704, 482),"");
		GUI.Label(new Rect(50, 550, 704, 482),"");
		
		GUI.skin = mainSkin;	
		
		GUI.Label(new Rect( 140 , 60 , 400, 120), "Chapter 1");
		Scenarios_ShowLevels(1, 6, 140);		
		
		GUI.skin = mainSkin;
		GUI.Label(new Rect( 140 , 580 , 400, 120), "Chapter 2");
		Scenarios_ShowLevels(7, 12, 440);
		
		GUI.skin = mainSkin;
		GUI.Label(new Rect( 140 , 740 , 400, 120), "Chapter 3");
		Scenarios_ShowLevels(13, 18, 740);
	}
	
	public void Scenarios_ShowLevels(int from, int to , int y)
	{		
		int lastCompleteLevel = PlayerPrefs.GetInt ("lastCompleteLevel");	
		
		int l = 1, k = 1;
		for(int i= from; i <= to; i++) {
			if(lastCompleteLevel>=i-1) {
				GUI.skin = mainSkin;
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