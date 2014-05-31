using UnityEngine;
using System.Collections;

public class MainMenu : MonoBehaviour {
	public string[] ramColors;
	
	public NGui ramGUI;
	// Use this for initialization
	void Start () {
		ramColors = new string[4];		
		ramColors[0] = "ramGray";
		ramColors[1] = "ramRed";
		ramColors[2] = "ramBlue";
		ramColors[3] = "ramYellow";
		GameObject ram01 = GameObject.Find ("ram_001");
		ram01.AddComponent("MeshRenderer");
		if (ram01.renderer.enabled) {							
			Transform[] allChildren = ram01.GetComponentsInChildren<Transform>();
			foreach (Transform child in allChildren) {
				if(child.name=="ramBody" || child.name=="ramBody2") {
					child.gameObject.renderer.material = (Material)Resources.Load(ramColors[1], typeof(Material));
				}
				if(child.name=="pCone1" || child.name=="pCone2") {
					child.gameObject.renderer.material = (Material)Resources.Load("ram_horns", typeof(Material));
				}
			}							
		}
		
		ramGUI = new NGui(this);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
	void OnGUI()
	{
		ramGUI.MainMenu();
	}
}
