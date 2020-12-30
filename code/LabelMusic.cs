using UnityEngine;
using System.Collections;

public class LabelMusic : MonoBehaviour {

	TextMesh text;
	
	void Awake()
	{
		text = gameObject.GetComponent<TextMesh>();
		UpdateText();
	}
	
	void UpdateText()
	{
		if(PlayerPrefs.GetInt("Music", 1) == 1)
		{
			text.text = "ENABLED";
		}
		else
		{
			text.text = "DISABLED";
		}
	}
}
