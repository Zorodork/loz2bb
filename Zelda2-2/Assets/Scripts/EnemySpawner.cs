using UnityEngine;
using System.Collections;

public class EnemySpawner : MonoBehaviour {
	public GameObject enemy;
	public float spawn = 10;
    public int limit;
	// Use this for initialization
	void Start () {
		InvokeRepeating ("spawnEnemy", spawn, spawn);
	}

	void spawnEnemy()
	{
        if(transform.childCount < limit)
        {
            GameObject enemy1 = (GameObject)Instantiate(enemy, transform.position, Quaternion.identity);
            enemy1.transform.parent = transform;
        }
	}
}
