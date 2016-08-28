using UnityEngine;
using System.Collections;

public class BlobSpawner : MonoBehaviour {
	public GameObject blob;
	public float spawn = 10;
	// Use this for initialization
	void Start () {
		InvokeRepeating ("spawnBlob", spawn, spawn);
	}

	void spawnBlob()
	{
        Instantiate(blob, new Vector2(-7,5), Quaternion.identity);
	}
}
