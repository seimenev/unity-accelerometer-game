using UnityEngine;
using System.Collections;

public class PickUpGem : MonoBehaviour {
		
	public static int GENERALGEMVALUE = 55;
	public static int DOUBLECOLORGEMVALUE = 150;
	public static int SINGLECOLORGEMVALUE = 250;
	public static float DECAYTIME = 0.5f;
	public static int DECAYVALUE = 1;
	
	public GameSettings.Colors gemColor;
		
	bool bSoundEnabled;
	bool bDecayRunning;
	public bool bTimedValue;
	int scoreValue;
	
	const string PARTICLEFFECT = "CollectEffect";
	
	void Start () 
	{		
		if(PlayerPrefs.GetInt("Sound", 1) == 1)
		{
			bSoundEnabled = true;
		}
		
		if(gemColor == GameSettings.Colors.COLOR_WHITE)
		{
			scoreValue = GENERALGEMVALUE;
		}
		else if(gemColor == GameSettings.Colors.COLOR_BLUE
			|| gemColor == GameSettings.Colors.COLOR_RED
			|| gemColor == GameSettings.Colors.COLOR_YELLOW)
		{
			scoreValue = SINGLECOLORGEMVALUE;
		}
		else
			scoreValue = DOUBLECOLORGEMVALUE;
	}
	
	IEnumerator Decay() {
    	yield return new WaitForSeconds(DECAYTIME);
		scoreValue -= DECAYVALUE;
		bDecayRunning = false;		
    }
	
	void StartDecay() {		
		StartCoroutine("Decay");	
	}
	
	// Update is called once per frame
	void Update () {
		if(!bDecayRunning)
		{			
			StartDecay();
			bDecayRunning = true;
		}		
	}
	
	void CollectedWrongGem()
	{
		GameManager.Manager.LoseLife();
		if( bSoundEnabled)
			AudioManager.Manager.PlayFX(AudioManager.AudioType.WRONG);
	}
	
	void CollectedGeneralGem()
	{
		if (bSoundEnabled)
			AudioManager.Manager.PlayFX(AudioManager.AudioType.GENERAL);
		GameManager.Manager.UpdateScore(scoreValue);
	}
	
	void CollectedRightGem()
	{
		if (bSoundEnabled)
			AudioManager.Manager.PlayFX(AudioManager.AudioType.RIGHT);
		GameManager.Manager.UpdateScore(scoreValue);
	}
	
	void OnTriggerEnter(Collider other) 
	{
		if(other.gameObject.tag == "Player")
		{
			Player playerComponent = other.GetComponent<Player>();
			if(playerComponent != null)
			{
				if(playerComponent.color == GameSettings.Colors.COLOR_YELLOW)
				{
					if(gemColor == GameSettings.Colors.COLOR_RED
						|| gemColor == GameSettings.Colors.COLOR_BLUE
						|| gemColor == GameSettings.Colors.COLOR_PURPLE)						
							CollectedWrongGem();
				
					else if(gemColor == GameSettings.Colors.COLOR_WHITE)
						CollectedGeneralGem();
					else
						CollectedRightGem();					
				}
				else if(playerComponent.color == GameSettings.Colors.COLOR_BLUE)
				{
					if(gemColor == GameSettings.Colors.COLOR_YELLOW
						|| gemColor == GameSettings.Colors.COLOR_RED
						|| gemColor == GameSettings.Colors.COLOR_ORANGE)
							CollectedWrongGem();
					else if(gemColor == GameSettings.Colors.COLOR_WHITE)
						CollectedGeneralGem();
					else
						CollectedRightGem();
				}
				else if(playerComponent.color == GameSettings.Colors.COLOR_RED)
				{
					if(gemColor == GameSettings.Colors.COLOR_YELLOW
						|| gemColor == GameSettings.Colors.COLOR_BLUE
						|| gemColor == GameSettings.Colors.COLOR_GREEN)
						CollectedWrongGem();
					else if(gemColor == GameSettings.Colors.COLOR_WHITE)
						CollectedGeneralGem();
					else
						CollectedRightGem();						
				}
				GameManager.Manager.SpawnPickupEffect(gemColor, transform.position);				
				Destroy(gameObject);
			}
		}		
	}
}
