using UnityEngine;
using System.Collections;

//Class Used As A Skeleton For Enemies
public class EnemyBehavior : MonoBehaviour {
	[SerializeField]
	protected int _health;
	protected AudioSource auSource;
	public AudioClip death;
	protected BoxCollider2D coll;
	protected Rigidbody2D rb, rbp;
	protected Animator anim;
    [SerializeField]
    protected float timer, speed, height;
    protected RaycastHit2D ground;
    protected LayerMask layerMask;
    public GameObject drop;
    [SerializeField]
    private int prob = 1;
    protected bool isDead;
	
    /*
	// Update is called once per frame
	void Update () {
        //check if grounded
        ground = Physics2D.Raycast(transform.position, Vector2.down, 0.1f, layerMask);
        //if blob is dead
		if (_health <= 0 && !auSource.isPlaying) {
			
		}
        //die if falls into pit
		if (rb.position.y < -10) {
			_health = 0;
		}
        //if grounded and falling
        if (ground && rb.velocity.y <= 0)
            rb.velocity = new Vector2(0, rb.velocity.y);
	}*/
    protected void enDeath()
    {
        isDead = true;
        anim.SetBool("isDead", true);
        auSource.Play();
        rb.velocity = new Vector2(0, 0);
        rb.isKinematic = true;
        coll.enabled = false;
        int random = Random.Range(1, prob);
        if (random == 1)
            Instantiate(drop, transform.position, transform.rotation);
        Destroy(gameObject, death.length);
    }
    //blobs health
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
}
