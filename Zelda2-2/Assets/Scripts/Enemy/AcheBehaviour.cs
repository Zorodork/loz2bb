using UnityEngine;

public class AcheBehaviour : EnemyBehavior {

    //private SpriteRenderer rend;
    //int i = 1;
    [SerializeField]
    bool isFlying, isMovingLeft; //checks if bat is in flying state, moving left
    [SerializeField]
    float dist, horSpeed; //velocity based on player's y position, horizontal speed

    // Use this for initialization
    void Start() {
        
        //values
        _health = 1;
        speed = 5; //controls how fast bat swoops
        horSpeed = 2; //bat's horizontal speed
        timer = timerSet();
        layerMask = ~(1 << 8); //check against layers that AREN'T 8 (enemy layer)
        //objects
        auSource = GetComponent<AudioSource>();
        coll = GetComponent<BoxCollider2D>();
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        rbp = GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody2D>();
        //rend = GetComponent<SpriteRenderer>();
    }

	// Update is called once per frame
	void Update () {
        /*
        if(i % 100 == 0) {
            rb.gravityScale = 1;
            rb.AddForce(Trajectory.GetParableInitialVelocity(transform.position, rbp.position, 0), ForceMode2D.Impulse);
            print("We flew!");
            
        }
        i = i + 1;*/
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
            if (Mathf.Abs(rbp.position.x - rb.position.x) < 3 && !isFlying && timer <= 0)
            {
                //print("On the hunt");
                isFlying = true;
                anim.SetBool("isFlying", true);
                //check distance down from bat to player's feet (approx)
                dist = rbp.position.y - rb.position.y - 1;
                //check if player is to left
                if (rbp.position.x < rb.position.x)
                    isMovingLeft = true;
                else
                    isMovingLeft = false;
            }
            if (isFlying)
            {
                //check if moving left
                float dir = (isMovingLeft) ? -1 : 1;
                rb.velocity = new Vector2(horSpeed * dir, dist);
                //change velocity to go up by a certain amount
                dist += Time.deltaTime * speed;
            }
            if (timer > 0)
                timer -= Time.deltaTime;
        }
	}

    float timerSet() {
        float timers = 1;
        return timers;
    }
    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Untagged")
        {
            if (other.transform.position.y > rb.position.y)
            {
                anim.SetBool("isFlying", false);
                isFlying = false;
                rb.velocity = Vector2.zero;
                timer = timerSet();
            }
        }
    }
}
