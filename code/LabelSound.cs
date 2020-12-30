using UnityEngine;
using System.Collections;

public class LabelSound : MonoBehaviour {
	
	TextMesh text;
	
	void Awake()
	{
		text = gameObject.GetComponent<TextMesh>();
		UpdateText();
	}
	
	void UpdateText()
	{		
		if(PlayerPrefs.GetInt("Sound", 1) == 1)
		{
			text.text = "ENABLED";
		}
		else
		{
			text.text = "DISABLED";
		}
	}
}
