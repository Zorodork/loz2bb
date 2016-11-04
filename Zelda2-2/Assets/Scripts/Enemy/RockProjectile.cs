using UnityEngine;

public class RockProjectile : MonoBehaviour {
    private Animator anim;
    private AudioSource auSource;
    public AudioClip dink;
    private Rigidbody2D rb;
    private Collider2D coll;
    [SerializeField]
    private bool _isLeft, _isAlive;
    private int speed;
	// Use this for initialization
	void Start () {
        _isAlive = true;
        speed = 6;
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        coll = GetComponent<Collider2D>();
        auSource = GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update () {
        if (_isAlive)
        {
            if (_isLeft)
                rb.velocity = Vector2.left * speed;
            else
                rb.velocity = Vector2.right * speed;
        }
	}
    void hit(bool player)
    {
        _isAlive = false;
        anim.SetBool("isDead", true);
        if (!player)
        {
            auSource.clip = dink;
            auSource.Play();
        }
        rb.velocity = new Vector2(0, 0);
        coll.enabled = false;
        Destroy(gameObject, .5f);
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        //print(other);
        if (other.tag != "Enemy" && other.tag != "Player")
        {
            if (other.tag == "PlayerColl")
                hit(true);
            else
                hit(false);
        }
            
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
    public bool isAlive
    {
        get
        {
            return _isAlive;
        }
        set
        {
            _isAlive = value;
        }
    }
}
