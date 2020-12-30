using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {
	
	float physicsMoveSpeed;
	float moveSpeed;
	bool bUsePhysics;
	bool bUpsideDown;
		
	public GameSettings.Colors color;
	
	Material[] materials;
	
	SkinnedMeshRenderer meshComponent;

	Vector3 startPosition;
	GameSettings.Colors startColor;

	float delayUpdate;

	public void Reset()
	{
		transform.position = startPosition;
		color = startColor;
		GetComponent<Rigidbody>().velocity = new Vector3(0.0f, 0.0f, 0.0f);
	}

	void Awake()
	{
		meshComponent = gameObject.GetComponentInChildren<SkinnedMeshRenderer>();
		
		UnityEngine.Object[] tempArray = Resources.LoadAll(Paths.PLAYERMATERIALS);
		materials = new Material[tempArray.Length];
		int index = 0;
		foreach(UnityEngine.Object obj in tempArray)
		{
			materials[index] = obj as Material;
			index++;
		}
		
		startPosition = transform.position;
	}
	
	// Use this for initialization
	void OnEnable () {
		
		if (GameManager.Manager.currentState == GameManager.GameState.CLASSICMODE)
		{
			if(gameObject.name == "Player1")
			{				
				color = GameSettings.Colors.COLOR_YELLOW;
				UpdateMeshColor();
			}
			else
			{
				transform.position = GameObject.Find("Player1").transform.position;				
				color = GameSettings.Colors.COLOR_BLUE;
				UpdateMeshColor();
			}
		}
		else
		{			
			if(gameObject.name == "Player2")
			{
				return;
			}
			else
			{
				color = GameSettings.Colors.COLOR_YELLOW;
				UpdateMeshColor();
			}			
		}
		
		physicsMoveSpeed = GameObject.Find("Data").GetComponent<GameSettings>().physicsMoveSpeed;
		moveSpeed = GameObject.Find("Data").GetComponent<GameSettings>().moveSpeed;
		bUsePhysics = GameObject.Find("Data").GetComponent<GameSettings>().bUsePhysics;
		
		if(!bUsePhysics)
		{
			GetComponent<Rigidbody>().isKinematic = true;
		}
		delayUpdate = Time.time + 2.0f;
		startColor = color;
	}
	
	public void SwapPlayer(GameSettings.Colors colorChange) {
		color = colorChange;
		UpdateMeshColor();
	}
	
	void UpdateMeshColor()
	{
		if(meshComponent == null)
			return;

		Material[] newMaterials = new Material[2];

		newMaterials[0] = meshComponent.sharedMaterials[0];

		if (color == GameSettings.Colors.COLOR_BLUE)
		{
			newMaterials[1] = materials[0];
		}
		else if (color == GameSettings.Colors.COLOR_YELLOW)
		{
			newMaterials[1] = materials[2];
		}
		else
		{
			newMaterials[1] = materials[1];
		}

		meshComponent.sharedMaterials = newMaterials;
	}	
	
	void PlayerOneMobileInput() 
	{
		Vector3 acceleration = Vector3.zero;
		Vector3 adjustedInput = JTInput.GetAccelerometer(Input.acceleration);
		
		if(Screen.orientation == ScreenOrientation.LandscapeLeft)
		{
			acceleration.z = adjustedInput.x;
			acceleration.x = -adjustedInput.y;			
		}
		else if(Screen.orientation == ScreenOrientation.Portrait)
		{
			acceleration.z = adjustedInput.x;
			acceleration.x = -adjustedInput.y;
		}
		else if(Screen.orientation == ScreenOrientation.LandscapeRight)
		{
			acceleration.z = -adjustedInput.x;
			acceleration.x = adjustedInput.y;			
		}
		else if(Screen.orientation == ScreenOrientation.PortraitUpsideDown)
		{
			acceleration.z = -adjustedInput.x;
			acceleration.x = adjustedInput.y;			
		}
		
		if(acceleration.sqrMagnitude > 1)
		{
			acceleration.Normalize();
		}
				
		if(bUsePhysics)
		{
			acceleration *= (physicsMoveSpeed * Time.deltaTime);
			GetComponent<Rigidbody>().velocity = acceleration;
		}
		else
		{
			acceleration *= (moveSpeed * Time.deltaTime);
			transform.Translate(acceleration, Space.World);
		}
	}
	
	void PlayerTwoMobileInput() 
	{
		Vector3 acceleration = Vector3.zero;
		Vector3 adjustedInput = JTInput.GetAccelerometer(Input.acceleration);

		if (Screen.orientation == ScreenOrientation.LandscapeLeft)
		{
			acceleration.z = -adjustedInput.x;
			acceleration.x = adjustedInput.y;
		}
		else if (Screen.orientation == ScreenOrientation.Portrait)
		{
			acceleration.z = -adjustedInput.x;
			acceleration.x = adjustedInput.y;
		}
		else if (Screen.orientation == ScreenOrientation.LandscapeRight)
		{
			acceleration.z = adjustedInput.x;
			acceleration.x = -adjustedInput.y;
		}
		else if (Screen.orientation == ScreenOrientation.PortraitUpsideDown)
		{
			acceleration.z = adjustedInput.x;
			acceleration.x = -adjustedInput.y;
		}
		
		if(acceleration.sqrMagnitude > 1)
		{
			acceleration.Normalize();
		}
				
		if(bUsePhysics)
		{
			acceleration *= (physicsMoveSpeed * Time.deltaTime);
			GetComponent<Rigidbody>().velocity = acceleration;
		}
		else
		{
			acceleration *= (moveSpeed * Time.deltaTime);
			transform.Translate(acceleration, Space.World);
		}
	}

	void Update () 
	{
		if (delayUpdate > Time.time)
			return;
		
		if(Input.GetKeyDown(KeyCode.Escape))
		{
			GameManager.Manager.ChangeGameState(GameManager.GameState.GAMEOVER);
		}
		
		if (gameObject.name == "Player2" && GameManager.Manager.currentState == GameManager.GameState.CLASSICMODE)
		{
			PlayerTwoMobileInput();
		}
		else
		{
			PlayerOneMobileInput();
		}

		if (GetComponent<Rigidbody>().velocity.magnitude > 0.0f)
		{
			Quaternion lookAt = Quaternion.LookRotation((transform.position + GetComponent<Rigidbody>().velocity.normalized * 2.0f) - transform.position);
			transform.rotation = lookAt;
		}
	}
}

