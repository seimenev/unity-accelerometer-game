using UnityEngine;
using System.Collections;

public class GameSettings : MonoBehaviour {
	
	public static string previousGameMode = "";
	public float physicsMoveSpeed;
	public float moveSpeed;
	public bool bUsePhysics;
	public enum Colors {
		COLOR_BLUE,
		COLOR_GREEN,
		COLOR_ORANGE,
		COLOR_PURPLE,
		COLOR_RED,		
		COLOR_WHITE,
		COLOR_YELLOW
	}
			
	void Awake()
	{
		DontDestroyOnLoad(gameObject);		
	}
		
	void OnLevelWasLoaded(int newLevel)
	{
		Resources.UnloadUnusedAssets();	
	}
}
