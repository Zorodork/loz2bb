using UnityEngine;
using System.Collections;

public class RockProjectile : MonoBehaviour {
    Animator anim;
    Rigidbody2D rb;
    Collider2D coll;
    [SerializeField]
    private bool _isLeft, isAlive;
    private int speed;
	// Use this for initialization
	void Start () {
        isAlive = true;
        speed = 6;
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        coll = GetComponent<Collider2D>();
	}
	
	// Update is called once per frame
	void Update () {
        if (isAlive)
        {
            if (_isLeft)
                rb.velocity = Vector2.left * speed;
            else
                rb.velocity = Vector2.right * speed;
        }
	}
    void hit()
    {
        isAlive = false;
        anim.SetBool("isDead", true);
        //auSource.Play();
        rb.velocity = new Vector2(0, 0);
        coll.enabled = false;
        Destroy(gameObject, .5f);
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag != "Enemy")
            hit();
    }
    public bool isLeft
    {
        get
        {
            return _isLeft;
        }
        set
        {
            _isLeft = value;
        }
    }
}
