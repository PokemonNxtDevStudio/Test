using UnityEngine;
using System.Collections;

public class AudioController : MonoBehaviour {

    public AudioClip collisionClip;
    public string colliderToActivateAudio = "lakeCollider";

    private void OnCollisionEnter(Collision collision) {
        foreach (ContactPoint contact in collision.contacts) {

            if (colliderToActivateAudio.Equals(contact.otherCollider.name))
                audio.PlayOneShot(collisionClip);
        }        
    }
}
