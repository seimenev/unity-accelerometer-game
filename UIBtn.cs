using UnityEngine;
using System.Collections;

public class UIBtn : MonoBehaviour {

	string userName;
	string oldUserName;	
	TouchScreenKeyboard keyboard;
	bool bTyping;
	TextMesh text;
	bool bActivated;
	float localScaleX;
	float localScaleY;
	float localScaleZ;

	public enum ButtonType
	{
		CLASSIC,
		TAG,
		EXIT,
		SOUND,
		MUSIC,		
		SYNC,
		USERNAME,
		GAMEOVERYES,
		GAMEOVERNO
	}

	public ButtonType buttonType;

	void Awake()
	{
		if (buttonType == ButtonType.MUSIC)
		{
			if (GameObject.Find("Music") != null)
			{
				text = GameObject.Find("Music").GetComponent<TextMesh>();
			}
		}
		if (buttonType == ButtonType.SOUND)
		{
			if (GameObject.Find("Sound") != null)
			{
				text = GameObject.Find("Sound").GetComponent<TextMesh>();
			}
		}
		if (buttonType == ButtonType.USERNAME)
		{
			oldUserName = PlayerPrefs.GetString("UserName", "USERNAME");
			userName = oldUserName;
			if (GameObject.Find("Username") != null)
			{
				text = GameObject.Find("Username").GetComponent<TextMesh>();
				text.text = userName;
			}
		}
	}

	void UpdateText()
	{
		text.text = PlayerPrefs.GetString("UserName", "USERNAME");
	}

	void Start()
	{
		localScaleX = transform.localScale.x;
		localScaleY = transform.localScale.y;
		localScaleZ = transform.localScale.z;
	}

	void OnTouch()
	{
		if (!bActivated && !bTyping)
		{
			if (PlayerPrefs.GetInt("Sound", 1) == 1)
				AudioManager.Manager.PlayFX(AudioManager.AudioType.UISOUND);

			bActivated = true;

			AnimationCurve curveX = AnimationCurve.EaseInOut(0.0f, localScaleX, 0.25f, localScaleX * 1.2f);
			AnimationCurve curveY = AnimationCurve.EaseInOut(0.0f, localScaleY, 0.25f, localScaleY * 1.2f);
			AnimationCurve curveZ = AnimationCurve.EaseInOut(0.0f, localScaleZ, 0.25f, localScaleZ * 1.2f);

			AnimationClip clip = new AnimationClip();
			clip.SetCurve("", typeof(Transform), "localScale.x", curveX);
			clip.SetCurve("", typeof(Transform), "localScale.y", curveY);
			clip.SetCurve("", typeof(Transform), "localScale.z", curveZ);
			GetComponent<Animation>().AddClip(clip, "test");
			GetComponent<Animation>().Play("test");
			Invoke("Activate", 0.25f);
		}
	}
	
	public void Activate()
	{
		bool bPlayExitAnim = true;

		switch (buttonType)
		{
			case ButtonType.CLASSIC:
				GameManager.Manager.ChangeGameState(GameManager.GameState.CLASSICMODE);				
				break;
			case ButtonType.EXIT:
				Application.Quit();
				bPlayExitAnim = false;
				break;
			case ButtonType.MUSIC:
				if(text.GetComponent<Animation>() != null)
				text.GetComponent<Animation>().Play();
				if(PlayerPrefs.GetInt("Music", 1) == 1)
				{
					PlayerPrefs.SetInt("Music", 0);
					AudioManager.Manager.StopMusic();
				}
				else
				{
					PlayerPrefs.SetInt("Music", 1);
					AudioManager.Manager.PlayMusic(true);
				}
				break;
			case ButtonType.SOUND:
				if (text.GetComponent<Animation>() != null)
				text.GetComponent<Animation>().Play();
				if (PlayerPrefs.GetInt("Sound", 1) == 1)
				{
					PlayerPrefs.SetInt("Sound", 0);
				}
				else
				{
					PlayerPrefs.SetInt("Sound", 1);
				}
				break;
			case ButtonType.SYNC:
				if (Application.internetReachability == NetworkReachability.NotReachable)
				{
					if (gameObject.GetComponent<TextMesh>() != null)
					{
						gameObject.GetComponent<TextMesh>().text = "No connection";
					}
					break;
				}
				if(gameObject.GetComponent<TextMesh>() != null)
				{
					gameObject.GetComponent<TextMesh>().text = "Syncing...";
				}
				StartCoroutine("UpdateScores");
				if(GameObject.Find("Highscore") != null)
				{
					GameObject.Find("Highscore").BroadcastMessage("UpdateScoreText", SendMessageOptions.DontRequireReceiver);
				}
				break;
			case ButtonType.TAG:
				GameManager.Manager.ChangeGameState(GameManager.GameState.TAGMODE);
				break;
			case ButtonType.USERNAME:
				keyboard = TouchScreenKeyboard.Open(userName, TouchScreenKeyboardType.Default);
				bTyping = true;
				break;
			case ButtonType.GAMEOVERNO:
				GameManager.Manager.ChangeGameState(GameManager.GameState.MAINMENU);
				break;
			case ButtonType.GAMEOVERYES:
				GameManager.Manager.RestartLevel();
				break;
		}

		if (bPlayExitAnim)
		{
			AnimationCurve curveX = AnimationCurve.EaseInOut(0.0f, localScaleX * 1.2f, 0.25f, localScaleX);
			AnimationCurve curveY = AnimationCurve.EaseInOut(0.0f, localScaleY * 1.2f, 0.25f, localScaleY);
			AnimationCurve curveZ = AnimationCurve.EaseInOut(0.0f, localScaleZ * 1.2f, 0.25f, localScaleZ);

			AnimationClip clip = new AnimationClip();
			clip.SetCurve("", typeof(Transform), "localScale.x", curveX);
			clip.SetCurve("", typeof(Transform), "localScale.y", curveY);
			clip.SetCurve("", typeof(Transform), "localScale.z", curveZ);
			GetComponent<Animation>().AddClip(clip, "test");
			GetComponent<Animation>().Play("test");
			Invoke("ReEnable", 0.25f);
		}
	}

	public void ReEnable()
	{
		bActivated = false;
	}

	IEnumerator UpdateScores()
	{
		string url;
		WWW www;
		float elapsedTime;

		if (PlayerPrefs.HasKey("UserName") && PlayerPrefs.HasKey("HighScore") && PlayerPrefs.GetInt("NewHighscore", 0) == 1)
		{
			PlayerPrefs.SetInt("NewHighscore", 0);
			url = "http://codemeddler.com/jtupdate.php";

			WWWForm form = new WWWForm();
			form.AddField("User", PlayerPrefs.GetString("UserName"));
			form.AddField("Score", PlayerPrefs.GetInt("HighScore"));
			www = new WWW(url, form);

			elapsedTime = 0.0f;

			while (!www.isDone)
			{
				elapsedTime += Time.deltaTime;
				if (elapsedTime >= 10.0f)
				{
					break;
				}

				yield return null;
			}

			if (!www.isDone || !string.IsNullOrEmpty(www.error))
			{
				Debug.LogError(string.Format("WWW Error!\n{0}", www.error));
				yield break;
			}
		}

		url = "http://codemeddler.com/jtscorea.php";

		www = new WWW(url);

		elapsedTime = 0.0f;

		while (!www.isDone)
		{
			elapsedTime += Time.deltaTime;
			if (elapsedTime >= 10.0f)
			{
				break;
			}

			yield return null;
		}

		if (!www.isDone || !string.IsNullOrEmpty(www.error))
		{
			Debug.LogError(string.Format("WWW Error!\n{0}", www.error));
			yield break;
		}

		string response = www.text;

		//Debug.Log(elapsedTime + " : " + response);

		string[] scoreData = response.Split('"');

		for (int i = 0; i < 10; i++)
		{
			//Debug.Log(scoreData[7 + i*12]);
			//Debug.Log(scoreData[11 + i*12]);
			PlayerPrefs.SetString("HighScoreUser" + i.ToString(), scoreData[7 + i * 12]);
			PlayerPrefs.SetString("HighScoreValue" + i.ToString(), scoreData[11 + i * 12]);
		}

		if (gameObject.GetComponent<TextMesh>() != null)
		{
			gameObject.GetComponent<TextMesh>().text = "Sync";
		}
	}

	void Update()
	{
		if (buttonType != ButtonType.USERNAME)
			return;

		if (bTyping)
		{
			if (keyboard != null)
			{
				if (keyboard.done)
				{
					userName = keyboard.text;
					if (userName.Length > 8)
					{
						userName = userName.Substring(0, 8);
					}
					PlayerPrefs.SetString("UserName", userName);
					oldUserName = userName;
					if (text.GetComponent<Animation>() != null)
						text.GetComponent<Animation>().Play();
					UpdateText();
					bTyping = false;
				}
			}
		}
	}
}
