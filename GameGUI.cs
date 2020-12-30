using UnityEngine;
using System.Collections;

public class GameGUI : MonoBehaviour {

	public TextMesh textMesh;
	public Transform healthParent;

	GameObject[] hearts;

	void Awake()
	{
		hearts = new GameObject[5];
		hearts[0] = healthParent.Find("HP1").gameObject;
		hearts[1] = healthParent.Find("HP2").gameObject;
		hearts[2] = healthParent.Find("HP3").gameObject;
		hearts[3] = healthParent.Find("HP4").gameObject;
		hearts[4] = healthParent.Find("HP5").gameObject;
	}

	public void UpdateHealth()
	{
		int blocked = GameManager.Manager.lives;

		foreach (GameObject heart in hearts)
		{
			if (blocked > 0)
			{
				heart.SetActive(true);
				blocked--;
				continue;
			}

			heart.SetActive(false);
		}
	}

	public void UpdateScore() 
	{
		string scoreText = "";
		string displayText = "";

		int score = GameManager.Manager.points;

		while (score > 0)
		{
			scoreText += (score % 10).ToString();
			score = score / 10;
		}

		int difference = 5 - scoreText.Length;
		
		for (int i = 0; i < difference; i++)
		{
			displayText += "0";
		}

		for (int i = scoreText.Length; i > 0; i--)
		{
			displayText += scoreText[i-1];
		}

		textMesh.text = displayText;
    }
}
