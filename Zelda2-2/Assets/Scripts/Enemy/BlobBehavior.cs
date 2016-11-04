using UnityEngine;

public class BlobBehavior : EnemyBehavior {

	// Use this for initialization
	void Start () {
        //values
		_health = 2;
        speed = 2;
        height = 8;
        timerSet();
        layerMask = ~(1 << 8); //check against layers that AREN'T 8 (enemy layer)
        //objects
        auSource = GetComponent<AudioSource> ();
		coll = GetComponent<BoxCollider2D> ();
		anim = GetComponent<Animator> ();
		rb = GetComponent<Rigidbody2D> ();
        rbp = GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody2D>();
	}
	
	// Update is called once per frame
	void Update () {
        //check if grounded
        ground = Physics2D.Raycast(transform.position, Vector2.down, 0.1f, layerMask);
        Debug.DrawLine(transform.position, transform.position - new Vector3(0,.1f,0), Color.green);
        //if blob is dead
        if (_health <= 0 && !isDead)
        {
            enDeath();
        }
        //die if falls into pit
        if (rb.position.y < minHeight)
        {
            _health = 0;
        }
        if (_health > 0)
        {
            //if grounded and falling
            if (ground && rb.velocity.y <= 0)
                rb.velocity = new Vector2(0, rb.velocity.y);
            //timer for jump
            if (timer < .5 & ground.collider != null)
                anim.speed = 2;
            if (timer > 0)
                timer -= Time.deltaTime;
            else if (ground.collider != null)
                hop();
        }
	}
	//tell blob to hop
	void hop(){
        float value = (rb.position.x < rbp.position.x) ? 1 : -1;
        rb.velocity = new Vector2(value*speed, height);
        timerSet();
        anim.speed = 1;
	}
    //set the timer
    void timerSet(){
        timer = Random.Range(3,6);
    }
}
