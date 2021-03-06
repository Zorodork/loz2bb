﻿using UnityEngine;

public class PlayMove2 : MonoBehaviour {
    //stuck test
    public bool stuck;

    //MOVEMENT VARIABLES
    public float Speed = 4f; //player speed, DEFAULT 4
    public float Height = 12f; //height of jump, DEFAULT 12
    [SerializeField]
    private float movex = 0f;
    [SerializeField]
    private float moveupy = 0f;
    [SerializeField]
    private int _health;


    //STATE VARIABLES
    [SerializeField]
    private bool inAir = false; //is hero in air/not grounded?
    [SerializeField]
    private bool isAttacking = false; //is hero attacking?
    [SerializeField]
    private bool isDucking = false; //is hero ducking?
    [SerializeField]
    private bool isHit = false; //is hero hit?
    [SerializeField]
    private bool isItem = false; //is hero holding a new item?

    //OBJECT VARIABLES
    private Rigidbody2D rb;
    private SpriteRenderer rend;
    private Animator anim;
    private BoxCollider2D coll;
    private AudioSource auSource;
    public AudioClip slash, hit, hurt, heart, key, dodododo;
    public GameObject arrow;
    private BoxCollider2D sword, shield, playerc; //child hitboxes

    //GAME VARIABLES
    private float attTime; //float used to keep track of attack length, arrow length
    private float stun = 0; //float used to keep track of stun time
    [SerializeField]
    private int power = 1; //how strong link is, DEFAULT 1
    [SerializeField]
    private int _maxHealth = 3; //how much health link can have, DEFAULT 3
    [SerializeField]
    private int _dungeonID; //current map
    private int[] _keys = new int[] { 0, 0, 0, 0, 0, 0 }; //add keys by dungeon to this array
    private bool[] _weapons = new bool[] { false, false, false, false, false, false }; //add weapons to this array
    private bool[] _bossKeys = new bool[] { false, false, false, false, false, false }; //add bosskeys to this array

    //PLAYER PROPERTIES
    private float stunTime = 2; // default stun time, DEFAULT 2
    private float friction = 1.25f; // default friction, DEFAULT 1.25

    private Vector2 respawn; //where link will respawn
    public float minHeight; //where link will die
    private RaycastHit2D ground, bump; //tells if link is grounded
    private LayerMask player; //ignores player

    // Use this for initialization
    void Start () {
        rb = GetComponent<Rigidbody2D>();
        rend = GetComponentInChildren<SpriteRenderer>();
        anim = GetComponentInChildren<Animator>();
        coll = GetComponent<BoxCollider2D>();
        auSource = GetComponent<AudioSource>();
        _health = _maxHealth;
        respawn = transform.position;
        player = ~((1 << 9) + (1 << 11) + (1 << 12) + (1 << 13)); //check against layers that AREN'T 9 (player, weapon, pickup, GoThrough)
        sword = transform.Find("Sword").GetComponent<BoxCollider2D>();
        shield = transform.Find("Shield").GetComponent<BoxCollider2D>();
        playerc = transform.Find("Hero").GetComponent<BoxCollider2D>();
    }

    //controls here to avoid input drops
    void Update()
    {
        if (Time.timeScale != 0 || !isItem) //mute changes if game is paused
        {
            ground = Physics2D.Raycast(transform.position - new Vector3(.4f, .1f), Vector3.right, .8f, player);
            Color col = ground ? Color.red : Color.green;
            Debug.DrawLine(transform.position - new Vector3(.4f, .1f), transform.position - new Vector3(.4f, .1f) + Vector3.right * .8f, col);
            if (ground)
                //print(ground.transform);
            //flicker when stunned
            if (stun > 0)
                rend.enabled = !rend.enabled;
            else if (!rend.enabled)
                rend.enabled = true;

            //knockback tester
            if (Input.GetKeyDown("k"))
            {
                isHit = true;
                auSource.clip = hurt;
                auSource.Play();
                stun = stunTime;
                rb.velocity = new Vector2(5, 10);
            }

            //player movement
            movex = Input.GetAxis("Horizontal");

            if (!isAttacking && !isHit)
            {
                if (Input.GetKeyDown("space") && !inAir)
                {
                    jump();
                }
                if (Input.GetKeyDown("e"))
                {
                    attack();
                }
                if (Input.GetKeyDown("f")&&_weapons[0])
                {
                    shoot();
                }
                else if (Input.GetKey("s") || Input.GetKey("down"))
                {
                    isDucking = true;
                    anim.SetBool("isDucking", isDucking);
                    //CHANGING PLAYER HITBOX
                    //coll.offset = new Vector2(coll.offset.x, 0.845f);
                    //coll.size = new Vector2(coll.size.x, 1.69f);
                    changeHitbox(coll.offset.x, 0.845f, coll.size.x, 1.69f);
                    //shield hitbox
                    shield.offset = new Vector2(shield.offset.x, .595f);
                    shield.size = new Vector2(shield.size.x, .815f);
                }
                else
                {
                    isDucking = false;
                    anim.SetBool("isDucking", isDucking);
                    if (!inAir)
                    {
                        //CHANGING PLAYER HITBOX
                        //coll.offset = new Vector2(coll.offset.x, 1);
                        //coll.size = new Vector2(coll.size.x, 2);
                        changeHitbox(coll.offset.x, 1, coll.size.x, 1.9f);
                        shield.offset = new Vector2(shield.offset.x, 1.31f);
                        shield.size = new Vector2(shield.size.x, .875f);
                    }
                }
            }
            //check if attacking to reset attack
            if (isAttacking)
            {
                attTime += Time.deltaTime;
                if (attTime > .3f)
                    resetAttack();
            }
            //countdown for stun time
            if (stun > 0)
            {
                stun -= Time.deltaTime;
            }
        }
        //return game play to normal after getting item
        if (isItem && !auSource.isPlaying)
        {
            isItem = false;
            Time.timeScale = 1;
        }
    }
    // Update is called once per frame, physics updates here
    void FixedUpdate()
    {
        if (stuck)
            print("Overlap detected!");
        //reset position if dead and done with knockback
        if (rb.position.y < minHeight || (_health <= 0 && !isHit))
        {
            death();
        }
        //jumping checks
        if (ground)
        {
            //checks if grounded after being launched, or if a certain time has passed while being grounded
            if (inAir || (stun < stunTime*.9 && !inAir))
                isHit = false;
            inAir = false;
            anim.SetBool("inAir", inAir);
            if (!isDucking)
            {
                //CHANGING PLAYER HITBOX
                //coll.offset = new Vector2(coll.offset.x, 1);
                //coll.size = new Vector2(coll.size.x, 2);
                changeHitbox(coll.offset.x, 1, coll.size.x, 1.9f);
                shield.offset = new Vector2(shield.offset.x, 1.31f);
                shield.size = new Vector2(shield.size.x, .875f);
            }
        }
        else
        {
            inAir = true;
            anim.SetBool("inAir", inAir);
            //CHANGING PLAYER HITBOX
            //coll.offset = new Vector2(coll.offset.x, 0.845f);
            //coll.size = new Vector2(coll.size.x, 1.69f);
            changeHitbox(coll.offset.x, 0.845f, coll.size.x, 1.69f);
            shield.offset = new Vector2(shield.offset.x, .595f);
            shield.size = new Vector2(shield.size.x, .815f);
        }
        //movements only while not ducking or attacking, hit overrides
        if (!isHit)
        {
            if ((!isAttacking && !isDucking) || inAir)
                rb.velocity = new Vector2(movex * Speed, rb.velocity.y);
            else
                rb.velocity = new Vector2(rb.velocity.x / friction, rb.velocity.y);
        }

        //sprite and collision flippings, prevents flipping while attacking or hit
        if (!isAttacking && !isHit)
        {
            if (Input.GetAxis("Horizontal") < 0)
            {
                rend.flipX = true;
                //change collider to face left
                //CHANGING PLAYER HITBOX
                //coll.offset = new Vector2(-.125f, coll.offset.y);
                //coll.offset = playerc.offset = new Vector2(-.125f, coll.offset.y);
                shield.offset = new Vector2(-.405f, shield.offset.y);
                anim.SetBool("isWalking", true);
            }
            else if (Input.GetAxis("Horizontal") > 0)
            {
                rend.flipX = false;
                //change collider to face right
                //CHANGING PLAYER HITBOX
                //coll.offset = new Vector2(.125f, coll.offset.y);
                //coll.offset = playerc.offset = new Vector2(.125f, coll.offset.y);
                shield.offset = new Vector2(.405f, shield.offset.y);
                anim.SetBool("isWalking", true);
            }
            else if (Mathf.Abs(rb.velocity.x) < 1)
            {
                anim.SetBool("isWalking", false);
            }
        }
    }


    void jump()
    {
        moveupy = Height;
        rb.velocity = new Vector2(rb.velocity.x, moveupy);
    }
    void attack()
    {
        auSource.clip = slash;
        auSource.Play();
        isAttacking = true;
        Invoke("attackHitBox", .05f);
        anim.SetBool("isAttacking", isAttacking);
    }
    void resetAttack()
    {
        isAttacking = false;
        anim.SetBool("isAttacking", isAttacking);
        sword.offset = sword.size = Vector2.zero;
        attTime = 0;
        sword.enabled = false;
        shield.enabled = true;
    }
    void attackHitBox()
    {
        //enables sword hitbox on child and changes hitboxes accordingly
        sword.enabled = true;
        shield.enabled = false;
        if (isDucking&&!inAir)
        {
            if (rend.flipX == false)
            {
                sword.offset = new Vector2(.6f, .6f);
                sword.size = new Vector2(1, .5f);
            }
            else
            {
                sword.offset = new Vector2(-.6f, .6f);
                sword.size = new Vector2(1, .5f);
            }
        }
        else
        {
            if (rend.flipX == false)
            {
                sword.offset = new Vector2(.6f, 1.25f);
                sword.size = new Vector2(1, .5f);
            }
            else
            {
                sword.offset = new Vector2(-.6f, 1.25f);
                sword.size = new Vector2(1, .5f);
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
    void changeHitbox(float oa, float ob, float sa, float sb)
    {
        coll.offset = playerc.offset = new Vector2(oa, ob);
        coll.size = playerc.size = new Vector2(sa, sb);
    }
    //collider variant
    public void getHit(Collider2D other)
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
            rb.velocity = new Vector2(pos / 2, 1)*10;
            stun = stunTime;
        }
    }
    public void getHit(Collision2D other) //collision variant
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
            rb.velocity = new Vector2(pos / 2, 1)*10;
            stun = stunTime;
        }
    }

    //hit enemy method (USED BY SWORD/WEAPONS)
    public void landHit(Collider2D other)
    {
        auSource.clip = hit;
        auSource.Play();
        if (!other.GetComponent<EnemyBehavior>())
            other.GetComponent<BlobBehavior>().health -= power;
        else
            other.GetComponent<EnemyBehavior>().health -= power;
    }
    //get powerup method (USED BY SWORD/PLAYER)
    public void collectHealth(Collider2D other)
    {
        if (health<maxHealth)
                health++;
            auSource.clip = heart;
            auSource.Play();
            Destroy(other.transform.parent.gameObject);
      }
    //get powerup method (USED BY SWORD/PLAYER)
    public void collectKey(Collider2D other)
    {
        keys[_dungeonID]++; //TODO: replace 0 with dungeon number
        auSource.clip = key;
        auSource.Play();
        Destroy(other.transform.parent.gameObject);
    }
    public void collectBossKey(Collider2D other)
    {
        bossKeys[_dungeonID] = true; //TODO: replace 0 with dungeon number
        auSource.clip = key;
        auSource.Play();
        Destroy(other.transform.parent.gameObject);
    }
    //get powerup method (USED BY SWORD/PLAYER)
    public void collectWeapon(Collider2D other)
    {
        weapons[_dungeonID] = true; ; //TODO: replace 0 with item number
        auSource.clip = dodododo;
        auSource.Play();
        isItem = true;
        Time.timeScale = 0;
        Destroy(other.gameObject);
    }
    //method to use when hero shoots arrow
    void shoot()
    {
        float mult = (isDucking) ? .6f : 1.4f;
        GameObject arrow1 = (GameObject)Instantiate(arrow, transform.position + (Vector3.up * mult), transform.rotation);
        arrow1.GetComponent<ArrowProjectile>().speed = Speed * 3;
        arrow1.GetComponent<ArrowProjectile>().power = power; 
        if (rend.flipX == true)
        {
            arrow1.GetComponent<ArrowProjectile>().isLeft = true;
            arrow1.transform.rotation = Quaternion.Euler(0, 0, 90);
        }
        else
        {
            arrow1.GetComponent<ArrowProjectile>().isLeft = false;
            arrow1.transform.rotation = Quaternion.Euler(0, 0, -90);
        }
        isAttacking = true;
        attTime = 0;
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
    public int[] keys
    {
        get
        {
            return _keys;
        }
        set
        {
            _keys = value;
        }
    }
    public bool[] weapons
    {
        get
        {
            return _weapons;
        }
        set
        {
            _weapons = value;
        }
    }
    public bool[] bossKeys
    {
        get
        {
            return _bossKeys;
        }
        set
        {
            _bossKeys = value;
        }
    }
    public int dungeonID
    {
        set
        {
            _dungeonID = value;
        }
    }

    //Zip player up if stuck
    void OnCollisionStay2D(Collision2D other)
    {
        Bounds bound1 = coll.bounds;
        Debug.DrawLine(bound1.min, bound1.max, Color.blue);
        Bounds bound2 = other.collider.bounds;
        Debug.DrawLine(bound2.min, bound2.max, Color.blue);

        if (bound2.Intersects(bound1) && !other.gameObject.name.Contains("SemiSolid"))
        {
            stuck = true;
            //rb.position += (rend.flipX) ? Vector2.right*.5f : Vector2.left*.5f;
            //TODO: only do this if stuck between two colliders
            rb.position += Vector2.up*.15f;
        }
        else
        {
            stuck = false;
        }

    }
}
