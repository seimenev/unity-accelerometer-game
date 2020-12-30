using UnityEngine;
using System.Collections;

public class SpeechBubble : MonoBehaviour
{
	Transform myTransform;
	Vector3 screenPos;
	Vector3 viewportPos;
	
	public int bubbleWidth = 200;
	public int bubbleHeight = 100;
	public float padding = 10;
	public float offsetX = 0;
	public float offsetY = 150;

	int centerOffsetX;
	int centerOffsetY;
		
	public Material mat;		
	public GUISkin guiSkin;
	public Camera cameraToUse;
	public string shownText;
	
	void Awake()
	{		
		myTransform = transform;
	}

	//use this for initialization
	void Start()
	{
		//Calculate the X and Y offsets to center the speech balloon exactly on the center of the game object
		centerOffsetX = bubbleWidth / 2;
		centerOffsetY = bubbleHeight / 2;
	}
		
	void LateUpdate()
	{
		screenPos = cameraToUse.WorldToScreenPoint(myTransform.position);

		viewportPos.x = screenPos.x / (float)Screen.width;
		viewportPos.y = screenPos.y / (float)Screen.height;
	}

	void OnGUI()
	{
		//Begin the GUI group centering the speech bubble at the same position of this game object. After that, apply the offset
		GUI.BeginGroup(new Rect(screenPos.x - centerOffsetX - offsetX, Screen.height - screenPos.y - centerOffsetY - offsetY, bubbleWidth, bubbleHeight));

		//Render the round part of the bubble
		GUI.Label(new Rect(0, 0, bubbleWidth, bubbleHeight), "", guiSkin.customStyles[0]);

		//Render the text
		GUI.Label(new Rect(padding, padding, bubbleWidth - padding, bubbleHeight - padding), shownText, guiSkin.label);
		
		GUI.EndGroup();
	}
	
	void OnRenderObject()
	{
		GL.PushMatrix();
		mat.SetPass(0);
		GL.LoadOrtho();		
		GL.Begin(GL.TRIANGLES);
		GL.Color(Color.white);		
		GL.Vertex3(viewportPos.x, viewportPos.y + (offsetY / 3) / Screen.height, 0.1f);
		GL.Vertex3(viewportPos.x - (bubbleWidth / 3) / (float)Screen.width, viewportPos.y + offsetY / Screen.height, 0.1f);
		GL.Vertex3(viewportPos.x - (bubbleWidth / 8) / (float)Screen.width, viewportPos.y + offsetY / Screen.height, 0.1f);
		GL.End();
		GL.PopMatrix();
	}
}