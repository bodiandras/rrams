using UnityEngine;

public class RamPrefs
{
	public RamPrefs()
	{
		
	}
	
	public void SaveLevelRating(int lvl, int moveDiff)
	{
		string key = "levelrating_"	+ lvl;
		int oldRating = this.GetLevelRating(lvl);
		int newRating;
		switch(moveDiff) {
			case 0: newRating = 3; break;
			case 1: newRating = 2; break;;
			default: newRating = 1; break;
		}
		if(newRating > oldRating) {
			PlayerPrefs.SetInt(key, newRating);
		}
	}
	
	public int GetLevelRating(int lvl)
	{
		string key = "levelrating_"	+ lvl;
		return PlayerPrefs.GetInt(key);
	}
}	

