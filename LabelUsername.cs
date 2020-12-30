using UnityEngine;
using System.Collections;

public class LabelUsername : MonoBehaviour {

	TextMesh text;
	string oldUserName;
	
	void Awake()
	{
		oldUserName = PlayerPrefs.GetString("UserName", "USERNAME");
		text = gameObject.GetComponent<TextMesh>();
		UpdateText();
	}
	
	void UpdateText()
	{
		text.text = oldUserName;
	}
}
