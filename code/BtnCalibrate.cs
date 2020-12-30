using UnityEngine;
using System.Collections;

public class BtnCalibrate : MonoBehaviour {
			
	void OnTouch()
	{
		if (PlayerPrefs.GetInt("Sound", 1) == 1)
			AudioManager.Manager.PlayFX(AudioManager.AudioType.UISOUND);
		GetComponent<Animation>().Play();
	}
	
	void UpdateText()
	{
		Activate();	
	}
	
	void Activate()	
	{
		JTInput.CalibrateAccelerometer();
		if(gameObject.GetComponent<TextMesh>() != null)
		{
			gameObject.GetComponent<TextMesh>().text = "Calibrated";
		}
	}
}
