using UnityEngine;

public class ArrowProjectile : MonoBehaviour {
    private Animator anim;
    private AudioSource auSource;
    public AudioClip dink, hits;
    private Rigidbody2D rb;
    private Collider2D coll;
    [SerializeField]
    private bool _isLeft, _isAlive, isDeflect;
    private int speed;
	// Use this for initialization
	void Start () {
        _isAlive = true;
        speed = 10;
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
        if (isDeflect)
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y - 1);
            transform.Rotate(Vector3.forward, Time.deltaTime*360);
        }
	}
    void hit(bool other)
    {
        _isAlive = false;
        if (!other)
        {
            isDeflect = true;
            rb.velocity = new Vector2(-rb.velocity.x / 4, rb.velocity.y);
            auSource.clip = dink;
            auSource.Play();
        }
        else
        {
            rb.velocity = Vector2.zero;
            auSource.clip = hits;
            auSource.Play();
        }
        
        coll.enabled = false;
        Destroy(gameObject, 1f);
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        print(other);
        if (other.tag != "Player" && other.tag != "Hero")
        {
            if (other.tag == "Enemy")
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
