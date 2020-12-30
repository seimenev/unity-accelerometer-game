using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Announcer : MonoBehaviour {

	public float messageShowTime;
	float messageTime;
	static Announcer instance;
	TextMesh mesh;
	Queue<string> messageQueue;

	public static Announcer Instance
	{
		get
		{
			return instance;
		}
	}

	void Awake()
	{
		if (instance != null)
		{
			Debug.LogWarning("Announcer instance already created.");
			Destroy(gameObject);
		}

		instance = this;
		mesh = GetComponent<TextMesh>();
		messageQueue = new Queue<string>();
	}

	public void ShowMessage(string message)
	{
		messageQueue.Enqueue( message );		
	}

	void Update()
	{
		if (messageTime <= Time.time)
		{
			if (messageQueue.Count > 0)
			{
				messageTime = Time.time + messageShowTime;
				mesh.text = messageQueue.Dequeue();
			}
			else
				mesh.text = "";
		}
	}
}
