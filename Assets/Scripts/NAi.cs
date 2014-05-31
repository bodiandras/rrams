using System;
using UnityEngine;
using System.Collections;

public class NAi
{
	public Main main;
	
	public NAi(Main main)
	{
		this.main = main;
	}
	
	public bool CheckVictory()
	{
		foreach(NObject ram in main.rams ) {		
			if(ram!=null) {				
				foreach(NObject goal in main.goals) {								
					if(ram.color == goal.color && ram.x == goal.x && ram.y ==goal.y ) {
						return true;
					}								
				}		
			}
		}
		return false;	
	}
	
	public int[] getBlackRamPath(NObject blackRam)
	{
		int[] newPos = null, retPos = null;
		int[] pos = new int[] {blackRam.x, blackRam.y};
	
		int min = 100 ,moves;
		
		newPos = tryMoveRam(blackRam, "up" , pos);
		if(newPos!=null) {
			moves = path (blackRam, newPos, 0);
			if(moves<min) {
				retPos = newPos;
				min = moves;
			}
		}
		newPos = tryMoveRam(blackRam, "right" , pos);
		if(newPos!=null) {
			moves = path (blackRam, newPos, 0);
			if(moves<min) {
				retPos = newPos;
				min = moves;
			}
		}
		newPos = tryMoveRam(blackRam, "down" , pos);
		if(newPos!=null) {
			moves = path (blackRam, newPos, 0);
			if(moves<min) {
				retPos = newPos;
				min = moves;
			}
		}
		newPos = tryMoveRam(blackRam, "left" , pos);
		if(newPos!=null) {
			moves = path (blackRam, newPos, 0);
			if(moves<min) {
				retPos = newPos;
				min = moves;
			}
		}		
		
		//Debug.Log ("Starting from :" + pos[0] + " --- " + pos[1] + ", reached destination in " + min + " moves");
		return retPos;
	}
	
	public int path(NObject ram, int[] pos, int n)
	{
		int[] newPos=null;
		
		//Debug.Log (n + " - " +ram.finalDestination[0] + ram.finalDestination[1]);
		n++;		
		if(n>7) {
			return n;
		}
		int[] moves = new int[4];
		
		newPos = tryMoveRam(ram, "up" , pos);		
		if(newPos!=null) {
			if(newPos == ram.finalDestination) {
				return n;
			} else {
				moves[0] = path(ram, newPos, n);
			}
		}
		
		newPos = tryMoveRam(ram, "right" , pos);		
		if(newPos!=null) {
			if((newPos[0] == ram.finalDestination[0]) && (newPos[1] == ram.finalDestination[1])) {			
				return n;
			} else {				
				moves[1] = path(ram, newPos, n);
			}
		}
		
		newPos = tryMoveRam(ram, "down" , pos);		
		if(newPos!=null) {
			if(newPos == ram.finalDestination) {
				return n;
			} else {
				moves[2] = path(ram, newPos, n);
			}
		}
		
		newPos = tryMoveRam(ram, "left" , pos);		
		if(newPos!=null) {
			if(newPos == ram.finalDestination) {
				return n;
			} else {
				moves[3] = path(ram, newPos, n);
			}
		}
		int min = 0;
		for(int i=0; i<=3; i++) {			
			min = ((min==0) || ((moves[i]>0) && (moves[i]<min)))?moves[i]:min;
		}
		return min;		
	}
	
	public int[] tryMoveRam(NObject ram, string dir, int[] pos)
	{
		bool stopped = false, foundDestination = false;
		int i = pos[0], j= pos[1];
		switch(dir) {
			case "up" : j++;  break;
			case "down": j--; break;
			case "right": i++; break;
			case "left": i--; break;
		}
		
		int[] destination = null;
			
		while(i!=((dir=="left")?-1:main.mapSize[0]) && (dir=="left" || dir=="right") && !stopped) {
			if (main.IsValidDestination(new int[2]{i,pos[1]}, dir, ram)) {
				foundDestination = true;
				destination = new int[2]{i,pos[1]};						
			} else {						
					stopped = true;
			}
			i=(dir=="right")?i+1:i-1;
			//Debug.Log ("checking position "+ i+":"+j);
		}
				
		while(j!=((dir=="down")?-1:main.mapSize[1]) && (dir=="up" || dir=="down") && !stopped) {
			if (main.IsValidDestination(new int[2]{pos[0],j}, dir, ram)) {
				foundDestination = true;
				destination = new int[2] {pos[0],j};					
			} else {
				stopped = true;
			}
			j=(dir=="up")?j+1:j-1;
		}
		return destination;
	}
}