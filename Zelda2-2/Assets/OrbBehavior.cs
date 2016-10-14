using UnityEngine;
using System.Collections;

public class OrbBehavior : MonoBehaviour {
    public GameObject thing;
    [SerializeField]
    private Sprite red, blue;
    private SpriteRenderer rend;
	// Use this for initialization
	void Start () {
        rend = GetComponent<SpriteRenderer>();
	}
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Arrow")
        {
            thing.SetActive(!thing.activeInHierarchy);
            if (thing.activeInHierarchy)
                rend.sprite = blue;
            else
                rend.sprite = red;
        }
    }
}
