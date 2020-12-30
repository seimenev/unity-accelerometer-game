using UnityEngine;
using System.Collections;
using System;

public class GameManager : MonoBehaviour {

	public enum GameState
	{
		MAINMENU,
		HOWTOPLAY,
		CLASSICMODE,
		TAGMODE,
		CHALLENGEMODE,
		GAMEOVER,
		SPLASH
	};

	public GameState currentState {get; private set;}
	GameState previousState;

	static GameManager instance;

	public static GameManager Manager
	{
		get
		{
			return instance;
		}
	}
		
	public GameObject tagEffectPrefab;
	public GameObject pickupEffectPrefab;
	public GameObject[] playerObjects;
	GameObject[] gems;	// CHANGE TO BE A POOL
	public GameObject menuRoot;
	public GameObject ingameUIRoot;
	public GameObject gameoverRoot;
	public GameObject splashScreen;
	public SpeechBubble speechBubble;
	public CCamera gameCamera;
	public GameObject[] pads;
	public int points { get; private set; }
	public Spawner spawner;
	public int lives { get; private set; }
	public GameGUI gameUI;

	float gameStartTime;
	public Gradient gradient;
	public float darkenInterval;

	float timer;
	GameState replayState;
	bool bNewHighScore;

	public void SpawnTagEffect(GameSettings.Colors color, Vector3 location)
	{
		GameObject effect = Instantiate(tagEffectPrefab, location, Quaternion.identity) as GameObject;
		effect.transform.localScale *= UIScaler.uiScale;
		ParticleSystem system = effect.GetComponent<ParticleSystem>();
		switch(color)
		{
			case GameSettings.Colors.COLOR_BLUE:
				system.startColor = Color.blue;
				break;
			case GameSettings.Colors.COLOR_RED:
				system.startColor = Color.red;
				break;
			case GameSettings.Colors.COLOR_YELLOW:
				system.startColor = Color.yellow;
				break;
		}
	}

	public void SpawnPickupEffect(GameSettings.Colors color, Vector3 location)
	{
		GameObject effect = Instantiate(pickupEffectPrefab, location, Quaternion.identity) as GameObject;
		effect.transform.localScale *= UIScaler.uiScale;
		ParticleSystem system = effect.GetComponent<ParticleSystem>();
		switch (color)
		{
			case GameSettings.Colors.COLOR_BLUE:
				system.startColor = Color.blue;
				break;
			case GameSettings.Colors.COLOR_RED:
				system.startColor = Color.red;
				break;
			case GameSettings.Colors.COLOR_YELLOW:
				system.startColor = Color.yellow;
				break;
			case GameSettings.Colors.COLOR_PURPLE:
				system.startColor = new Color(1.0f, 0.0f, 1.0f);
				break;
			case GameSettings.Colors.COLOR_ORANGE:
				system.startColor = new Color(1.0f, 0.5f, 0.0f);
				break;
			case GameSettings.Colors.COLOR_GREEN:
				system.startColor = Color.green;
				break;
		}
	}

	public void LoseLife()
	{
		lives--;
		gameUI.UpdateHealth();
		if (lives <= 0)
		{
			currentState = GameState.GAMEOVER;
		}
	}

	public void UpdateScore(int value)
	{
		points += value;
		gameUI.UpdateScore();
		if (PlayerPrefs.GetInt("HighScore", 0) < points && !bNewHighScore)
		{
			bNewHighScore = true;
			Announcer.Instance.ShowMessage("NEW RECORD!");
		}
	}

	void Awake()
	{
		if (instance == null)
			instance = this;
		else
			Destroy(gameObject);

		currentState = GameState.SPLASH;
		previousState = GameState.SPLASH;		
	}

	void Start()
	{
		menuRoot.SetActive(false);
		ingameUIRoot.SetActive(false);
		Physics.IgnoreCollision(playerObjects[0].GetComponent<Collider>(), playerObjects[1].GetComponent<Collider>());
		Physics.IgnoreCollision(playerObjects[1].GetComponent<Collider>(), playerObjects[0].GetComponent<Collider>());
		foreach (GameObject playerObject in playerObjects)
		{
			playerObject.SetActive(false);
		}		
		foreach (GameObject padObject in pads)
		{
			padObject.SetActive(false);
		}
		speechBubble.enabled = false;
		splashScreen.SetActive(true);
	}
	
	public void ChangeGameState(GameState newState)
	{
		currentState = newState;
	}

	void Update()
	{
		if (Input.GetKey(KeyCode.O))
			LoseLife();

		if (currentState != previousState)
		{
			ActivateState(currentState);
		}

		if (currentState == GameState.CLASSICMODE || currentState == GameState.TAGMODE)
		{
			gameStartTime += Time.deltaTime;

			if (gameStartTime > darkenInterval)
			{
				gameStartTime = 0.0f;
				gradient.DeepenGradient();
			}
		}
		else if (currentState == GameState.SPLASH)
		{
			if (timer > 3.0f)
			{
				ChangeGameState(GameState.MAINMENU);
			}
			else
			{
				timer += Time.deltaTime;
			}
		}
	}

	public void RestartLevel()
	{
		//if(Application.platform == RuntimePlatform.Android)
			//AdvertisementHandler.HideAds();
		gameoverRoot.SetActive(false);
		if (replayState == GameState.TAGMODE)
		{
			playerObjects[0].SetActive(true);
			playerObjects[0].SendMessage("Reset", SendMessageOptions.DontRequireReceiver);
			TagMode();
		}
		else
		{
			foreach (GameObject playerObject in playerObjects)
			{
				playerObject.SetActive(true);
				playerObject.SendMessage("Reset", SendMessageOptions.DontRequireReceiver);
			}
			ClassicMode();
		}
		previousState = replayState;
		currentState = replayState;
		gameUI.UpdateHealth();
		gameUI.UpdateScore();
	}

	void MainMenu()
	{
		//if (Application.platform == RuntimePlatform.Android)
			//AdvertisementHandler.HideAds();
		AudioManager.Manager.StopMusic();
		AudioManager.Manager.PlayMusic(true);
		ingameUIRoot.SetActive(false);
		menuRoot.SetActive(true);
		gameCamera.SwitchCamera(1);
		foreach (GameObject playerObject in playerObjects)
		{
			playerObject.SetActive(false);
		}
		foreach (GameObject padObject in pads)
		{
			padObject.SetActive(false);
		}
		spawner.Deactivate();
		splashScreen.SetActive(false);
	}

	void TagMode()
	{
		AudioManager.Manager.StopMusic();
		AudioManager.Manager.PlayMusic(false);
		lives = 3;
		points = 0;
		gameUI.UpdateHealth();
		gameUI.UpdateScore();
		ingameUIRoot.SetActive(true);
		menuRoot.SetActive(false);
		gameCamera.SwitchCamera(2);
		playerObjects[0].SetActive(true);
		foreach (GameObject padObject in pads)
		{
			padObject.SetActive(true);
		}
		spawner.Activate();
		Announcer.Instance.ShowMessage("GO!");
		gameStartTime = 0.0f;
		replayState = GameState.TAGMODE;
	}

	void ClassicMode()
	{
		AudioManager.Manager.StopMusic();
		AudioManager.Manager.PlayMusic(false);
		lives = 5;
		points = 0;
		gameUI.UpdateHealth();
		gameUI.UpdateScore();
		ingameUIRoot.SetActive(true);
		menuRoot.SetActive(false);
		gameCamera.SwitchCamera(2);
		foreach (GameObject playerObject in playerObjects)
		{
			playerObject.SetActive(true);
		}
		foreach (GameObject padObject in pads)
		{
			padObject.SetActive(false);
		}
		spawner.Activate();
		Announcer.Instance.ShowMessage("GO!");
		gameStartTime = 0.0f;
		replayState = GameState.CLASSICMODE;
	}

	void ActivateState(GameState stateToActivate)
	{
		previousState = currentState;
		switch (stateToActivate)
		{
			case GameState.MAINMENU:
				gameoverRoot.SetActive(false);
				speechBubble.enabled = true;
				gameCamera.EnableTransition();
				Invoke("MainMenu", 1.0f);
				break;
			case GameState.TAGMODE:
				gameoverRoot.SetActive(false);
				speechBubble.enabled = false;
				gameCamera.EnableTransition();
				Announcer.Instance.ShowMessage("Ready!");
				Invoke("TagMode", 1.5f);
				break;
			case GameState.CLASSICMODE:
				gameoverRoot.SetActive(false);
				speechBubble.enabled = false;
				gameCamera.EnableTransition();
				Announcer.Instance.ShowMessage("Ready!");
				Invoke("ClassicMode", 1.5f);
				break;
			case GameState.GAMEOVER:
				gradient.LightenGradient();
				bNewHighScore = false;
				if (PlayerPrefs.GetInt("HighScore", 0) < points)
					PlayerPrefs.SetInt("HighScore", points);
				foreach (GameObject playerObject in playerObjects)
				{
					playerObject.SendMessage("Reset", SendMessageOptions.DontRequireReceiver);
					playerObject.SetActive(false);
				}
				gameoverRoot.SetActive(true);
				ingameUIRoot.SetActive(true);
				menuRoot.SetActive(false);
				spawner.Deactivate();				
				//if(Application.platform == RuntimePlatform.Android && Application.internetReachability != NetworkReachability.NotReachable)
					//AdvertisementHandler.ShowAds();
				break;
			case GameState.HOWTOPLAY:
				gameoverRoot.SetActive(false);
				ingameUIRoot.SetActive(true);
				menuRoot.SetActive(false);
				spawner.Deactivate();
				break;
		}
	}
}
