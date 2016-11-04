using UnityEngine;
using System.Collections;

public class CollectableBehavior : MonoBehaviour {
    float invTime = .5f; //half second spawn invincibility
	// Use this for initialization
	void Start () {
        gameObject.GetComponent<BoxCollider2D>().enabled = false;
	}
	
	// Update is called once per frame
	void Update () {
        if (invTime > 0)
            invTime -= Time.deltaTime;
        else
            gameObject.GetComponent<BoxCollider2D>().enabled = true;
    }
}
