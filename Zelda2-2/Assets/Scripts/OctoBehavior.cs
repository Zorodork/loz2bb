using UnityEngine;
using System.Collections;

public class OctoBehavior : EnemyBehavior {
    SpriteRenderer rend;

    // Use this for initialization
    void Start () {
        //values
        _health = 4;
        speed = 2;
        height = 8;
        timerSet();
        layerMask = ~(1 << 8); //check against layers that AREN'T 8 (enemy layer)
        //objects
        auSource = GetComponent<AudioSource>();
        coll = GetComponent<BoxCollider2D>();
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        rbp = GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody2D>();
        rend = GetComponent<SpriteRenderer>();
	}
	
	// Update is called once per frame
	void Update () {
        //check if grounded
        ground = Physics2D.Raycast(transform.position, Vector2.down, 0.1f, layerMask);
        rend.flipX = (rb.position.x > rbp.position.x) ? true : false;
        //if blob is dead
        if (_health <= 0 && !auSource.isPlaying)
        {
            enDeath();
        }
        //die if falls into pit
        if (rb.position.y < -10)
        {
            _health = 0;
        }
        
        //if grounded and falling
        if (ground && rb.velocity.y <= 0)
            rb.velocity = new Vector2(0, rb.velocity.y);
        //timer for jump
        if (timer < .5 && ground.collider != null)
            anim.speed = 2;
        if (timer > 0)
            timer -= Time.deltaTime;
        else if (ground.collider != null)
            hop();
	}
	//tell octo to hop
	void hop(){
        rb.velocity = new Vector2(0, height);
        timerSet();
        anim.speed = 1;
	}
    //set the timer
    void timerSet(){
        timer = Random.Range(3,6);
    }
}
