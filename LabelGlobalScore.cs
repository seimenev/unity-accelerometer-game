using UnityEngine;
using System.Collections;

public class LabelGlobalScore : MonoBehaviour {

	void Start()
	{
		UpdateScoreText();
	}
	
	void UpdateScoreText()
	{
		TextMesh target;
		for(int i=0; i<10; i++)
		{
			target = transform.Find("HighScore" + (i+1).ToString()).GetComponent<TextMesh>();
			
			target.text = ParsePositionText(i) + ParsePlayerName(i) + PlayerPrefs.GetString("HighScoreValue"+i.ToString(), "0");			
		}		
	}
	
	string ParsePositionText(int position)
	{
		string positionText = (position+1).ToString();
		if((position+1) < 10)
		{
			positionText += "  ";
		}
		return positionText;
	}
	
	string ParsePlayerName(int position)
	{
		string playerName = PlayerPrefs.GetString("HighScoreUser"+position.ToString(), "");
		int letterCount = playerName.Length;
		for(int i=letterCount; i<=8; i++)
		{
			playerName += "  ";
		}
		return playerName;
	}
}
