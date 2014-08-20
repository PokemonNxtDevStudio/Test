using UnityEngine;
using System.Collections;

public class WaterCollision : MonoBehaviour {

	void OnCollisionEnter(Collision collision) {
		if(collision.gameObject.tag == "watercollider") {
			audio.Play ();
		}
	}
}
