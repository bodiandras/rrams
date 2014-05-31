using System;
using System.Collections;
using UnityEngine;

public class Utils
{
	public Utils()
	{
		
	}
	
	public Map[,] ArrCopy(Map[,] arr)
	{		
		Map[,] ret = new Map[arr.GetLength(0),arr.GetLength(1)];
		for(int i=0; i<arr.GetLength(0); i++) {
			for(int j=0; j<arr.GetLength(1); j++) {
				ret[i,j] = (Map) arr[i,j].Clone();
				//ret[i,j].objects = (SortedList) arr[i,j].objects.Clone();
			}
		}		
		return ret;
	}
	
	public NObject[] ArrCopy(NObject[] arr)
	{
		NObject[] ret = new NObject[arr.Length];
		for(int i=0; i<arr.Length; i++) {
			ret[i]=arr[i];
		}
		return ret;
	}
	
	
    
    public string LoadXml(string fileName) 
	{
        TextAsset textAsset = (TextAsset)Resources.Load(fileName, typeof(TextAsset));
		//GuiTextDebug.debug(textAsset.text);
			//xmldoc.LoadXml ( textAsset.text );
            //result = www.text;			
        return textAsset.text;
	}
	
	public int FindKey(string[] arr, string val)
	{
		for(int i =0; i<arr.Length; i++) {
			if(arr[i] == val) {
				return i;	
			}
		}			
		return -1;
	}
	
	public static bool Contains(Array a, object val)
    {
        return Array.IndexOf(a, val) != -1;
    }
	
}