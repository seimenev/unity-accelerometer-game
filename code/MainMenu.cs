using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using MiniJSON;

public class MainMenu : MonoBehaviour {

	public AudioClip clickSound;
	public LayerMask guiLayer;
	public float cameraSpeed;

	const int MENUMIN = -1;
	const int MENUMAX = 1;

	bool bNotTranslating;
	float swipeSpeed = 0.05F;
	float swipeThreshold = 0.05f;
	float inputX;
	float inputY;
	Vector2 touchDeltaPosition;
	int targetMenu;
	Vector3 cameraTarget;
	GameObject touchedUIElement;
	bool bHaveNewTouch;
	bool bSoundEnabled;

	Quaternion targetRotation;

	void Start () 
	{
		Screen.sleepTimeout = SleepTimeout.NeverSleep;
	}

	GameObject DoTrace(Vector2 pointOfOrigin)
	{
		Ray traceRay = Camera.main.ScreenPointToRay(pointOfOrigin);
		RaycastHit hitInfo = new RaycastHit();
		if (Physics.Raycast(traceRay, out hitInfo, Mathf.Infinity, guiLayer))
		{	
			bHaveNewTouch = true;
			return hitInfo.collider.gameObject;
		}
		else
			return null;
	}

	void TranslateCamera()
	{		
		if(cameraTarget != Vector3.zero)
		{
			targetRotation = Quaternion.LookRotation((cameraTarget - transform.position), new Vector3(0,1,0));
		}
		else
		{
			targetRotation = transform.localRotation;
		}
		if(targetRotation != transform.localRotation)
		{
			transform.localRotation = Quaternion.RotateTowards(transform.localRotation, targetRotation, Time.deltaTime * cameraSpeed);
		}
		else
		{
			bNotTranslating = true;
		}
	}

	void ChangeMenuUp()
	{
		if(targetMenu < MENUMAX)
			targetMenu++;
		//else
		//	targetMenu = MENUMIN;
		bNotTranslating = false;
		SetCameraTarget();
	}

	void ChangeMenuDown()
	{
		if(targetMenu > MENUMIN)
			targetMenu--;
		//else
		//	targetMenu = MENUMAX;
		bNotTranslating = false;
		SetCameraTarget();
	}

	void SetCameraTarget()
	{
		cameraTarget = GameObject.Find("FixPoint" + targetMenu.ToString()).transform.position;
	}

	// Update is called once per frame
	void Update () {
		if (GameManager.Manager.currentState != GameManager.GameState.MAINMENU
			&& GameManager.Manager.currentState != GameManager.GameState.GAMEOVER)
			return;

		bHaveNewTouch = false;
		if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Moved && bNotTranslating && GameManager.Manager.currentState != GameManager.GameState.GAMEOVER) 
		{
			touchDeltaPosition = Input.GetTouch(0).deltaPosition;
			inputX += touchDeltaPosition.x * swipeSpeed;
			// KEPT JUST IN CASE I NEED VERTICAL SWIPE. 
			// inputY += touchDeltaPosition.y * swipeSpeed;
			if(inputX < swipeThreshold )
			{
				ChangeMenuUp();
			}
			else if(inputX > swipeThreshold)
			{
				ChangeMenuDown();
			}
		}
		else if(Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended && bNotTranslating)
		{
			touchedUIElement = DoTrace(Input.GetTouch(0).position);
		}

		if(bHaveNewTouch)
		{
			touchedUIElement.BroadcastMessage("OnTouch", SendMessageOptions.DontRequireReceiver);
		}
	}

	void LateUpdate()
	{
		if (GameManager.Manager.currentState != GameManager.GameState.MAINMENU)
			return;

		TranslateCamera();
	}
}
