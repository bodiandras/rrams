using System;
using System.Collections;
using UnityEngine;

public class Map : ICloneable
{
	public int top;
	public int right;
	public int bottom;
	public int left;
	public SortedList objects;

	public Map (int top=0, int right=0, int bottom=0, int left=0, SortedList objects = null)
	{
		if(objects!=null) {
			this.objects = (SortedList) objects.Clone();
		}
		this.top = top;
		this.right = right;
		this.bottom = bottom;
		this.left = left;
	}
	
	public System.Object Clone()
	{
		return new Map(this.top, this.right, this.bottom, this.left, this.objects);
	}
	
	public void Set(string variable, int value)
	{
		switch(variable) {
		case "top":
			top = value;
			break;
		case "right":
			right = value;
			break;
		case "bottom":
			bottom = value;
			break;
		case "left":
			left = value;
		break;
		}			
	}
}

