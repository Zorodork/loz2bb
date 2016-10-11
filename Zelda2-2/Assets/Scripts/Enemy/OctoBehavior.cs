using UnityEngine;

public class OctoBehavior : EnemyBehavior {
    private SpriteRenderer rend;
    [SerializeField]
    private float shootTimer;
    public GameObject rock;

    // Use this for initialization
    void Start () {
        //values
        _health = 3;
        speed = 0;
        height = 8;
        timer = timerSet();
        shootTimer = timerSet()+.5f;
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
        //if grounded, then enemy can flip
        if (ground)
            rend.flipX = (rb.position.x > rbp.position.x) ? true : false;
        //if is dead
        if (_health <= 0 && !isDead)
        {
            enDeath();
        }
        //die if falls into pit
        if (rb.position.y < -10)
        {
            _health = 0;
        }
        if (_health > 0)
        {
            //if grounded and falling
            if (ground && rb.velocity.y <= 0)
                rb.velocity = new Vector2(0, rb.velocity.y);
            //timer for jump
            if (timer < .5 && ground.collider != null)
                anim.speed = 2;
            if (timer > 0)
            {
                timer -= Time.deltaTime;
            }
            else if (ground.collider != null)
                hop();
            //timer for shooting
            if (shootTimer > 0)
                shootTimer -= Time.deltaTime;
            else
                shoot();
        }
    }
	//tell octo to hop
	void hop(){
        rb.velocity = new Vector2(speed, height);
        timer = timerSet();
        anim.speed = 1;
	}
    //tell octo to shoot in the right direction
    void shoot()
    {
        GameObject rock1 = (GameObject)Instantiate(rock, transform.position + (Vector3.up*.2f), transform.rotation);
        if (rend.flipX == true)
            rock1.GetComponent<RockProjectile>().isLeft = true;
        else
            rock1.GetComponent<RockProjectile>().isLeft = false;
        shootTimer = timerSet();
    }
    //set the timer
    float timerSet(){
        float timers = Random.Range(3,6);
        return timers;
    }
}
