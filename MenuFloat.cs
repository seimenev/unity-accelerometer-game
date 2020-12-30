using UnityEngine;
using System.Collections;

public class MenuFloat : MonoBehaviour {

	public float yDifference;
	public float ySpeed;
	Transform myTransform;
	float yStart;

	void Start()
	{
		myTransform = transform;
		yStart = myTransform.position.y;
	}
	void Update()
	{
		Vector3 position = myTransform.position;
		position.y = yStart + Mathf.Sin(Time.time * ySpeed * yDifference);
		myTransform.position = position;
	}
}
