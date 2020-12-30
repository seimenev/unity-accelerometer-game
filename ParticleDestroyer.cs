using UnityEngine;
using System.Collections;

public class ParticleDestroyer : MonoBehaviour {

	ParticleSystem ps;
    bool played;
 
    void Start () {
       ps = gameObject.GetComponent<ParticleSystem>();
    }
 
    void LateUpdate () {
       if(!played && ps.isPlaying) {
         played = true;
       }
       if(played && !ps.IsAlive(true)) {
         Destroy(gameObject);
       }
    }
}