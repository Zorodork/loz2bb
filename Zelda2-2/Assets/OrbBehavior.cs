using UnityEngine;
using System.Collections;

public class OrbBehavior : MonoBehaviour {
    public GameObject thing;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter2D(Collider2D other)
    {
        thing.SetActive(!thing.activeInHierarchy);
    }
}
