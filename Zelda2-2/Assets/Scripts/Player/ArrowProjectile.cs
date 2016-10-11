using UnityEngine;
using System.Linq;

public class ArrowProjectile : MonoBehaviour {
    private Animator anim;
    private AudioSource auSource;
    public AudioClip dink, hits, shoot;
    private Rigidbody2D rb;
    private Collider2D coll;
    [SerializeField]
    private bool _isLeft, _isAlive, isDeflect;
    private int _power;
    private float _speed;
    private GameObject player;
    // Use this for initialization
    void Start () {
        _isAlive = true;
        rb = GetComponent<Rigidbody2D>();
        coll = GetComponent<Collider2D>();
        auSource = GetComponent<AudioSource>();
        auSource.clip = shoot;
        auSource.Play();
        player = GameObject.FindGameObjectWithTag("Player");
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
            if(!_isLeft)
                transform.Rotate(Vector3.forward, Time.deltaTime * 360);
            else
                transform.Rotate(Vector3.forward, Time.deltaTime * -360);
        }
        //if object is a certain distance from player, destroy it
        if(Mathf.Abs(transform.position.y - player.transform.position.y)>25|| Mathf.Abs(transform.position.x - player.transform.position.x) > 25)
        {
            Destroy(gameObject);
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
            Destroy(gameObject);
        }
        
        coll.enabled = false;
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        string[] noColl = { "Hero", "Health", "Key" };
        if (!noColl.Contains(other.gameObject.tag))
        {
            if (other.tag == "Enemy")
            {
                hit(true);
                if (!other.GetComponent<EnemyBehavior>())
                    other.GetComponent<BlobBehavior>().health -= power;
                else
                    other.GetComponent<EnemyBehavior>().health -= power;
            }
            else
                hit(false);
        }

    }
    //getters/setters
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
    public float speed
    {
        get
        {
            return _speed;
        }
        set
        {
            _speed = value;
        }
    }
    public int power
    {
        get
        {
            return _power;
        }
        set
        {
            _power = value;
        }
    }
}
