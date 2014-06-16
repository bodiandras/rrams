using UnityEngine;
using System;
using System.Collections;

public class RamAnimator
{
	float tileSize;
	public int[] mapSize;
	GameObject destinationRam;
	public Generator objGenerator;
	public bool movementFinished = false;
	
	public RamAnimator (float tileSize, int[] mapSize, Generator objGenerator)
	{
		this.tileSize = tileSize;
		this.mapSize = mapSize;
		this.objGenerator = objGenerator;
	}
	
	public IEnumerator Move(GameObject go, int[] to, float speed)
	{
		movementFinished = false;
		Vector3 originalPosition = go.transform.position;
		Vector3 moveToPosition = new Vector3(((to[0] - mapSize[0] / 2)*this.tileSize)+0.01f, 0.01f, ((to[1] - mapSize[1] / 2)*this.tileSize)+0.009f);
		float distance = Mathf.Max(Mathf.Abs(originalPosition.x - moveToPosition.x) ,Mathf.Abs(originalPosition.z - moveToPosition.z));		
		float moveTime = distance * 5f;
		float startTime = Time.time;
    	float endTime = startTime + moveTime;						
		while((Time.time < endTime) || (Mathf.Abs(moveToPosition.x - go.transform.position.x)>0.001f) || (Mathf.Abs(moveToPosition.z - go.transform.position.z)>0.001f)) {
			go.transform.position=Vector3.Lerp(originalPosition, moveToPosition, (Time.time -startTime) / moveTime);
			yield return 1;
		}
		movementFinished = true;
	}
	
	public void ShowPath(GameObject go, string dir, int[] from, int[] to)
	{
		// make  ram turn		
		go.transform.rotation = Quaternion.Euler(0, 0, 0);
		
		switch(dir) {
			case "up": while(go.transform.rotation.y > 0) {
							go.transform.RotateAround(Vector3.up, -0.1f);			
						} break;
			case "right":  while(go.transform.rotation.y < 0.7f) {							
							go.transform.RotateAround(Vector3.up, 0.1f);							
						} break;
			case "down": while(go.transform.rotation.y < 1f) {
							go.transform.RotateAround(Vector3.up, 0.1f);			
						} break;
			case "left":  while(go.transform.rotation.y > -0.7f) {							
							go.transform.RotateAround(Vector3.up, -0.1f);							
						} break;
		}	
				
		this.destinationRam = objGenerator.instance(go ,new Vector3(((to[0] - mapSize[0] / 2)*tileSize)+0.01f, 0.01f, ((to[1] - mapSize[1] / 2)*tileSize)+0.009f), objGenerator.ramScale);
		
	}
	
	public void Teleport(GameObject go, int[] to)
	{
		go.transform.position = new Vector3(((to[0]- mapSize[0] / 2)*this.tileSize)+0.01f, 0.01f, ((to[1] - mapSize[1] / 2)*this.tileSize)+0.01f);
	}
	
	public void ClearPath()
	{
		UnityEngine.Object.Destroy(this.destinationRam);
	}
}