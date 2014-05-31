using UnityEngine;
using System.Collections;

public class RamInput {
	
	public bool mouseDown = false;
	public float rotationSpeed = 0f;
	public bool objectClicked = false;
	// Use this for initialization
	public RamInput() {
	}	
	// Update is called once per frame
	public void Update () {
		if (Input.GetMouseButtonDown(0)) {
			mouseDown = true;
		}
		if (Input.GetMouseButtonUp(0)) {
			mouseDown = false; objectClicked = false;
		}	
	}
	
	public GameObject GetClickedObject()
	{	
		string[] allowedObjects = {"ram_001(Clone)"};
		
		if( Input.GetMouseButtonDown(0) )
    	{       				
			Ray ray = Camera.main.ScreenPointToRay( Input.mousePosition );
       		RaycastHit hit;
 
       		if (Physics.Raycast( ray, out hit, 1 ) )
       		{				
				if(Utils.Contains(allowedObjects, hit.collider.name)) {
					objectClicked = true;					
					return hit.transform.gameObject;
				}
        	} 
		}
		return null;
	}
	
	public string Rotate()
	{		
		//Vector3 mousePos = Vector3.RotateTowards(new Vector3(x,0,y));
		//Vector2 mousePos = Vector2.(new Vector3(x,0,y));		
		if(mouseDown && !objectClicked) {
			float x = Input.GetAxis("Mouse X") ,y = Input.GetAxis("Mouse Y"), px = Input.mousePosition.x , py =Input.mousePosition.y;
			float ax = Mathf.Abs (x), ay = Mathf.Abs (y);
			
			if(ax<0.2f && ay<0.2f) {
				return "";
			}
			if (ax>ay && x>0 && py>Screen.height/2) {
				rotationSpeed = ax; return "right";			
			} else if (ax>ay && x<0 && py>Screen.height/2) {
				rotationSpeed = ax; return "left";
			} else if (ax>ay && x>0 && py<Screen.height/2) {
				rotationSpeed = ax; return "left";			
			} else if (ax>ay && x<0 && py<Screen.height/2) {
				rotationSpeed = ax; return "right";
			} else if (y<0 && px<Screen.width/2) {
				return "left";
			} else if (y>0 && px<Screen.width/2) {
				return "right";
			} else if (y<0 && px>Screen.width/2) {
				return "right";
			} else if (y>0 && px>Screen.width/2) {
				return "left";
			}
		}	
		return "";			
	}
	
	public static string GetMovementDirection(Vector3 origin, float cameraRotation)
	{	
		int layerMask = 1 << 8;
		float px = 0, py = 0;
		
		Ray ray = Camera.main.ScreenPointToRay( Input.mousePosition );
       	RaycastHit hit;
 
       	if (Physics.Raycast( ray, out hit, 1, layerMask ) )
       	{				
			if(hit.collider.name == "table") {
				px = hit.point.x; py = hit.point.z;				
				float px2 = origin.x, py2=origin.z;			
		
				if(Mathf.Abs(px-px2)>Mathf.Abs(py-py2)) {//horizontal movement
					if(px>px2*1.2) {
						return "right";
					} else if (px<px2*0.83) {
						return "left";
					}
				} else { // vertical movement
					if(py>py2*1.2) {
						return "up";
					} else if (py<py2*0.96) {
						return "down";
					}	
				}
			} else { // didn't collide with table
				Debug.Log (hit.collider.name);
				//return ""; 
			}				
       	}
		return ""; // movement was too little, do nothing
	}
	
	public static bool InputRelease()
	{
		if(Input.GetMouseButtonUp(0))
		{			
			return true;
		}
		return false;
	}
		
}
