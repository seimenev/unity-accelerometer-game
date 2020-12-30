using UnityEngine;
using System.Collections;

public class Pad : MonoBehaviour {
	
	public GameSettings.Colors color;
		
	void OnTriggerEnter(Collider other) {
		if(other.gameObject.CompareTag("Player"))
		{
			other.gameObject.GetComponent<Player>().SwapPlayer(color);
			GameManager.Manager.SpawnTagEffect(color, transform.position);
		}
	}
}
