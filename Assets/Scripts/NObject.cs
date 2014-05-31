using System;
using UnityEngine;

public class NObject
{
	public string name;
	public GameObject gameObject;
	public string color;
	public int[] originalPosition;
	public int x,y;
	
	public int[] finalDestination;
	public int[] currentDestination;
	
	
	public NObject (GameObject go, string name, int[] oPos, string color="")
	{
		this.gameObject = go;
		this.name = name;
		this.originalPosition = oPos;
		this.color = color;
		this.x = oPos[0];
		this.y = oPos[1];
	}
}