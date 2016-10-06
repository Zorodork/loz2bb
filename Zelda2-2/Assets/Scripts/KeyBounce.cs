using UnityEngine;

public class KeyBounce : MonoBehaviour {

    private AudioSource auSource;
	// Use this for initialization
	void Start () {
        auSource = GetComponent<AudioSource>();
    }
    void OnCollisionEnter2D(Collision2D other) {
        auSource.Play();
    }
}