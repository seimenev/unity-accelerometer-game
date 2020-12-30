using UnityEngine;
using System.Collections;

public class MaterialPanner : MonoBehaviour {
	
	public float scrollSpeed;
	float offset;
		
	void Update() {
    	offset += Time.deltaTime * scrollSpeed;
       	GetComponent<Renderer>().material.mainTextureOffset = new Vector2(offset, 0);
	}
}
