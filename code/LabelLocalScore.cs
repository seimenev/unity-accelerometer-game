using UnityEngine;
using System.Collections;

public class LabelLocalScore : MonoBehaviour {
		
	void OnEnable () {
		if(GameObject.Find("LocalScore") != null)
		{
			GameObject.Find("LocalScore").GetComponent<TextMesh>().text = PlayerPrefs.GetInt("HighScore", 0).ToString();
		}
	}	
}
