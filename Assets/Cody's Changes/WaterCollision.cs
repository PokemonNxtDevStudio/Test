using UnityEngine;
using System.Collections;

[RequireComponent(typeof(AudioSource))]
public class WaterCollision : MonoBehaviour {

    public AudioClip audio;
    AudioSource source;

    void Start()
    {
        if (!GetComponent<AudioSource>())
        {
            source = gameObject.AddComponent<AudioSource>();
        }
        else
        {
            source = GetComponent<AudioSource>();
        }

        source.clip = audio;
        source.playOnAwake = false;
        source.Stop();
    }

	void OnCollisionEnter(Collider col) {
        if (col.tag == "Player")
        {
            Debug.Log("Hit");
            source.Play();
        }
	}
}
