using System;
using UnityEngine;

public class NGui : MonoBehaviour
{
	public int nrMoves = 0;
	private Rect windowRect = new Rect (0, 0, (int)Screen.width , (int)Screen.height);
	private Rect normalCenterWindow = new Rect((int)(Screen.width *0.25f),(int)(Screen.height*0.5f - Screen.width*0.15f), (int)(Screen.width *0.5f) ,(int)(Screen.width *0.2f) ); 	
	private bool showGameMenu = false , showPauseMenu = false;
	private bool showScenarios = false;
	
	public bool showVictory = false;
	int lastCompleteLevel;
	
	Main main;
	//private MainMenu mainMenu = null;
	float w, h, oW=1920.0f, oH=1080.0f, sizeGui_normal = 1.5f, sizeGui_small = 1.1f;
	private float guiRatioX, guiRatioY, guiRatiotX, guiRatiotY, guiScrollY = 0, guiScrollX = 280;
	
	private Vector3 GUInF, GUIsF, GUItF;
	
	
	public GUISkin mainSkin, menuSkin, nextSkin, restartSkin, newSkin, undoSkin, mainMenuDisabled;
	
	public GUISkin pauseSkin, menuResumeSkin, menuMainMenuSkin;
	
	public GUISkin woodenPanel;
	
	public Texture levelTexture;
	
	public NGui(Main main)
	{
		this.main = main;
		
		this.h = Screen.height;
		this.w = Screen.width;
		guiRatioX = w/oW;
    	guiRatioY = h/oH;
			
		
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
		
		levelTexture = Resources.Load ("level_104x110") as Texture;
	}
	
	public NGui(MainMenu mainMenu)
	{
		//this.mainMenu = mainMenu;
		this.h = Screen.height;
		this.w = Screen.width;		
		
		guiRatioX = w/oW * sizeGui_normal;
    	guiRatioY = h/oH * sizeGui_normal;		
		GUInF = new Vector3(guiRatioX,guiRatioY,1); 
		
		guiRatioX = w/oW * sizeGui_small;
    	guiRatioY = h/oH * sizeGui_small;		
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
		//guiScrollY -= 3f;
		
		GUI.skin = woodenPanel;
		//GUI.matrix = Matrix4x4.TRS(new Vector3(GUInF.x,GUInF.y,0),Quaternion.identity,GUInF);
		GUI.Label(new Rect(50, 30 + guiScrollY, 704, 482),"");
		GUI.Label(new Rect(50, 550 + guiScrollY, 704, 482),"");
		GUI.Label(new Rect(50, 1070 + guiScrollY, 704, 482),"");
		
		GUI.skin = mainSkin;	
		float oY = 130 + guiScrollY;
		
		GUI.Label(new Rect( 140 , 60 + guiScrollY, 400, 120), "Chapter 1");
		Scenarios_ShowLevels(1, 8, 60 + oY);		
		
		GUI.skin = mainSkin;
		GUI.Label(new Rect( 140 , 580 + guiScrollY, 400, 120), "Chapter 2");
		Scenarios_ShowLevels(9, 16, 580 + oY);
		
		GUI.skin = mainSkin;
		GUI.Label(new Rect( 140 , 1100 + guiScrollY, 400, 120), "Chapter 3");
		
		Scenarios_ShowLevels(17, 24, 1100 + oY);
	}
	
	public void Scenarios_ShowLevels(int from, int to , float y)
	{		
		lastCompleteLevel = PlayerPrefs.GetInt ("lastCompleteLevel");
		
		if(!levelTexture) {
			levelTexture = Resources.Load ("level_104x110") as Texture;
		}
		
		
		
		int l = 1, k = 1;
		for(int i= from; i <= to; i++) {
			Scenarios_ShowLevel(i, y);		
			if(k++ == 4) {
				k = 1;
				l++;
			}
		}
	}
	
	public void Scenarios_ShowLevel(int i, float y)
	{
		
		int row = ( (int)Math.Floor((double)(i-1) % 8) < 4 ) ? 0 : 1;
		//GUI.matrix = Matrix4x4.TRS(new Vector3(GUInF.x,GUInF.y,0),Quaternion.identity,GUInF);
		GUI.DrawTexture(new Rect(( (i-1)%4 -1) * 150 + guiScrollX, row * 161 + y, 104, 110), levelTexture);
		
		if(lastCompleteLevel>=i-1) {
			GUI.skin = mainSkin;
			if(GUI.Button(new Rect(( (i-1)%4 -1) * 150 + guiScrollX, row * 161 + y, 104, 110), i.ToString()   )) {				
				PlayerPrefs.SetInt("selectedLevel", i);
				Application.LoadLevel("scene1");
			}
		} else {
				GUI.skin = mainMenuDisabled;				
				GUI.Label(new Rect(( (i-1)%4 -1) * 150 + guiScrollX, row * 161 + y, 104, 110), i.ToString());
		}	
	}

}