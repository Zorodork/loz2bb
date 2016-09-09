using UnityEngine;
using System.Collections;

public class AcheBehaviour : EnemyBehavior {

    private SpriteRenderer rend;
    int i = 1;
    Vector2 target;
    [SerializeField]
    bool isFlying, isMovingLeft;

    // Use this for initialization
    void Start() {
        
        //values
        _health = 1;
        speed = 5;
        height = 0;
        timer = timerSet();
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
        /*
        if(i % 100 == 0) {
            rb.gravityScale = 1;
            rb.AddForce(Trajectory.GetParableInitialVelocity(transform.position, rbp.position, 0), ForceMode2D.Impulse);
            print("We flew!");
            
        }
        i = i + 1;*/
        //if is dead
        if (_health <= 0 && !auSource.isPlaying)
        {
            enDeath();
        }
        if (Mathf.Abs(rbp.position.x - rb.position.x) < 3 && !isFlying && timer<=0)
        {
            print("On the hunt");
            target = rbp.position;
            isFlying = true;
            if (rbp.position.x < rb.position.x)
                isMovingLeft = true;
            else
                isMovingLeft = false;
        }
        if (isFlying)
        {
            //remember: y = x^2
            float pos, dir;
            //has the bat passed the target?
            if (isMovingLeft)
            {
                pos = (rb.position.x < target.x) ? -1 : 1;
                dir = 1;
            }
            else
            {
                pos = (rb.position.x > target.x) ? -1 : 1;
                dir = -1;
            }

                
            rb.velocity = new Vector2(-Time.deltaTime*100*speed*dir, -Time.deltaTime*100*speed*pos);

        }
        if(timer>0)
            timer -= Time.deltaTime;
	}

    float timerSet() {
        float timers = 1;
        return timers;
    }
    void OnCollisionEnter2D(Collision2D other)
    {
        if(other.transform.position.y > rb.position.y)
        {
            isFlying = false;
            rb.velocity = Vector2.zero;
            timer = timerSet();
        }
    }
}
