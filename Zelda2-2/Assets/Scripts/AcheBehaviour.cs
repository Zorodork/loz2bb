using UnityEngine;
using System.Collections;

public class AcheBehaviour : EnemyBehavior {

    private SpriteRenderer rend;
    int i = 1;
    [SerializeField]

    // Use this for initialization
    void Start() {
        
        //values
        _health = 1;
        speed = 0;
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

        if(i % 100 == 0) {
            rb.gravityScale = 1;
            rb.AddForce(Trajectory.GetParableInitialVelocity(transform.position, rbp.position, 0), ForceMode2D.Impulse);
            print("We flew!");
            
        }
        i = i + 1;
	}

    float timerSet() {
        float timers = Random.Range(3, 6);
        return timers;
    }
}
