using UnityEngine;
using System.Collections;

/*
 * Writer: Cody - xKriostar
 */

public class Movement : MonoBehaviour {

	public float movementSpeed = 10.0f;
	public float jumpHeight = 20.0f;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {

		// Four direction movement

		if(Input.GetKey(KeyCode.W)) { // Forward
			rigidbody.velocity = transform.forward * movementSpeed;
		}
		if(Input.GetKey(KeyCode.S)) { // Backwards
			rigidbody.velocity = -transform.forward * movementSpeed;
		}
		if(Input.GetKey(KeyCode.A)) { // Left
			rigidbody.velocity = -transform.right * movementSpeed;
		}
		if(Input.GetKey(KeyCode.D)) { // Right
			rigidbody.velocity = transform.right * movementSpeed;
		}

		if(Input.GetKeyDown(KeyCode.Space)) { // Jumping
			rigidbody.AddForce(Vector3.up * jumpHeight, ForceMode.Impulse);
		}
	}
}
