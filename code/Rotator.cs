using UnityEngine;
using System.Collections;

public class Rotator : MonoBehaviour {
	
	public bool bRotateX;
	public bool bRotateY;
	public bool bRotateZ;
	public float rotationSpeed;
	float xRot;
	float yRot;
	float zRot;
	
	// Use this for initialization
	void Start () {
		xRot = 0;
		yRot = 0;
		zRot = 0;
		if(bRotateX)
			xRot = Random.Range(-2.0f, 2.0f);
		if(bRotateY)
			yRot = Random.Range(-2.0f, 2.0f);
		if(bRotateZ)
			zRot = Random.Range(-2.0f, 2.0f);
	}
	
	// Update is called once per frame
	void Update () {
		transform.Rotate(xRot * rotationSpeed * Time.deltaTime, 
						yRot * rotationSpeed * Time.deltaTime, 
						zRot * rotationSpeed * Time.deltaTime, 
						Space.Self);
	}
}
