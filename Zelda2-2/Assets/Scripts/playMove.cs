using UnityEngine;
using System.Collections;

public class playMove : MonoBehaviour {

	public float Speed = 0f; //player speed, DEFAULT 4
	public float Height = 10f; //height of jump, DEFAULT 12
	[SerializeField]
	private float movex = 0f;
	[SerializeField]
	private float moveupy = 0f;
    [SerializeField]
    private int _health;

    private float stun = 0;

	[SerializeField]
	private bool inAir = false;
	[SerializeField]
	private bool isAttacking = false;
	[SerializeField]
	private bool isDucking = false;
    [SerializeField]
    private bool isHit = false;

    private Rigidbody2D rb;
	private SpriteRenderer rend;
	private Animator anim;
	private BoxCollider2D coll;
	private AudioSource auSource;
	public AudioClip slash, hit, hurt, heart;
	private BoxCollider2D sword;

    //GAME VARIABLES
	private float attTime; //float used to keep track of attack length
    [SerializeField]
    private int power = 1; //how strong link is, DEFAULT 1
    [SerializeField]
    private int _maxHealth = 3; //how much health link can have, DEFAULT 3
    private Vector2 respawn;

	// Use this for initialization
	void Start () {
		rb = GetComponent<Rigidbody2D> ();
		rend = GetComponent<SpriteRenderer> ();
		anim = GetComponent<Animator> ();
		coll = GetComponent<BoxCollider2D> ();
		auSource = GetComponent<AudioSource> ();
        _health = _maxHealth;
        respawn = transform.position;

    }
	//controls here to avoid input drops
	void Update(){

        //flicker when stunned
        if (stun > 0)
            rend.enabled = !rend.enabled;
        else if (!rend.enabled)
            rend.enabled = true;

        //knockback tester
        if(Input.GetKeyDown("k"))
        {
            isHit = true;
            auSource.clip = hurt;
            auSource.Play();
            stun = 2;
            rb.AddForce(new Vector2(500, 500));
        }

        //player movement
		movex = Input.GetAxis ("Horizontal");
		//to fix: if attacking, they can change hitbox by pressing down (Fixed?)
		if (!isAttacking && !isHit) {
            if (Input.GetKeyDown("space") && !inAir)
            {
                jump();
            }
            if (Input.GetKeyDown("e"))
			{
				attack();
			}
			else if (Input.GetKey ("s") || Input.GetKey ("down")) {
				isDucking = true;
				anim.SetBool ("isDucking", isDucking);
				coll.offset = new Vector2 (coll.offset.x, 0.845f);
				coll.size = new Vector2 (coll.size.x, 1.69f);
			} else {
				isDucking = false;
				anim.SetBool ("isDucking", isDucking);
				if (!inAir) {
					coll.offset = new Vector2 (coll.offset.x, 1);
					coll.size = new Vector2 (coll.size.x, 2);
				}
			}
		}
        //check if attacking to reset attack
		if (isAttacking) {
			attTime += Time.deltaTime;
			if (attTime > .3f)
				resetAttack ();
		}
        //countdown for stun time
        if(stun > 0)
        {
            stun -= Time.deltaTime;
        }
	}
	// Update is called once per frame, physics updates here
	void FixedUpdate () {

		//reset position if dead and done with knockback
		if (rb.position.y < -10 || (_health <= 0&&!isHit)) {
            death();
		}
		//jumping checks
		if (rb.velocity.y == 0) {
            if (inAir)
                isHit = false;
			inAir = false;
			anim.SetBool ("inAir", inAir);
			if (!isDucking) {
				coll.offset = new Vector2 (coll.offset.x, 1);
				coll.size = new Vector2 (coll.size.x, 2);
			}
		} else {
			inAir = true;
			anim.SetBool ("inAir", inAir);
			coll.offset = new Vector2(coll.offset.x, 0.845f);
			coll.size = new Vector2(coll.size.x, 1.69f);
		}
        //movements only while not ducking or attacking, hit overrides
        if (!isHit)
        {
            if ((!isAttacking && !isDucking) || inAir)
                rb.velocity = new Vector2(movex * Speed, rb.velocity.y);
            else
                rb.velocity = new Vector2(rb.velocity.x / 1.25f, rb.velocity.y);
        }

		//sprite and collision flippings, prevents flipping while attacking or hit
		if(!isAttacking&&!isHit){
			if (Input.GetAxis ("Horizontal") < 0) {
				rend.flipX = true;
				//change collider to face left
				coll.offset = new Vector2(-.125f, coll.offset.y);
				anim.SetBool ("isWalking", true);
			} else if (Input.GetAxis ("Horizontal") > 0) {
				rend.flipX = false;
				//change collider to face right
				coll.offset = new Vector2(.125f, coll.offset.y);
				anim.SetBool ("isWalking", true);
			} else if (Mathf.Abs(rb.velocity.x) < 1){
				anim.SetBool ("isWalking", false);
			}
		}
	}


	void jump(){
		moveupy = Height;
		rb.velocity = new Vector2 (rb.velocity.x, moveupy);
	}
	void attack(){
		auSource.clip = slash;
		auSource.Play ();
		isAttacking = true;
		Invoke("attackHitBox",.05f);
		anim.SetBool ("isAttacking", isAttacking);
	}
	void resetAttack()
	{
		isAttacking = false;
		anim.SetBool ("isAttacking", isAttacking);
		attTime = 0;
		Destroy (sword);
	}
	void attackHitBox()
	{
		sword = gameObject.AddComponent<BoxCollider2D> ();
		sword.isTrigger = true;
		if (isDucking) {
			if (rend.flipX == false) {
				sword.offset = new Vector2 (.7f, .6f);
				sword.size = new Vector2 (1, .5f);
			} else {
				sword.offset = new Vector2 (-.7f, .6f);
				sword.size = new Vector2 (1, .5f);
			}
		} else {
			if (rend.flipX == false) {
				sword.offset = new Vector2 (.7f, 1.25f);
				sword.size = new Vector2 (1, .5f);
			} else {
				sword.offset = new Vector2 (-.7f, 1.25f);
				sword.size = new Vector2 (1, .5f);
			}
		}
	}

    //Use this to reset variables upon death
    void death()
    {
        rb.velocity = Vector2.zero;
        rb.position = respawn;
        stun = 0;
        _health = _maxHealth;

    }

    //GETTERS/SETTERS
    public int health
    {
        get
        {
            return _health;
        }
        set
        {
            _health = value;
        }
    }
    public int maxHealth
    {
        get
        {
            return _maxHealth;
        }
    }

    //If Sword (trigger) enters another Collider
    void OnTriggerEnter2D(Collider2D other) {
		if (other.tag == "Enemy") {
			print ("hit");
			auSource.clip = hit;
			auSource.Play ();
            if (!other.GetComponent<EnemyBehavior>())
                other.GetComponent<BlobBehavior>().health -= power;
            else
                other.GetComponent<EnemyBehavior>().health -= power;
		}
        if (other.tag == "Health")
        {
            if (_health<_maxHealth)
                _health++;
            auSource.clip = heart;
            auSource.Play();
            Destroy(other.gameObject);
        }
	}

    //If Collider hits another one
    void OnCollisionStay2D(Collision2D other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            if (stun <= 0)
            {
                auSource.clip = hurt;
                auSource.Play();
                _health--;
                isHit = true;
                //find where he was hit to apply direction
                float pos;
                if (other.gameObject.transform.position.x < this.gameObject.transform.position.x)
                    pos = 1;
                else
                    pos = -1;
                //apply knockback!
                rb.AddForce(new Vector2(pos/2 ,1) * 500);
                stun = 2;  
            }
        }
    }
}
