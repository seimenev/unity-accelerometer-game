using UnityEngine;
using System.Collections;

public class UIScaler : MonoBehaviour {
	
	public static float uiScaleWidth { get; private set; }
	public static float uiScaleHeight { get; private set; }
	public static float uiScale { get; private set; }
	
	void Awake()
	{		
		uiScaleWidth = Screen.width / 800.0f;
		uiScaleHeight = Screen.height / 480.0f;
		uiScale = uiScaleWidth / uiScaleHeight;
		transform.localScale = new Vector3(uiScaleWidth, uiScaleHeight, uiScale);
	}
}
