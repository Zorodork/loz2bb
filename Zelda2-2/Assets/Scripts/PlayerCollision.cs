using UnityEngine;
using System.Collections;

public class PlayerCollision : MonoBehaviour
{
    PlayMove2 player;
    // Use this for initialization
    void Start()
    {
        player = GetComponentInParent<PlayMove2>();
    }
    //If hero collides with enemy trigger
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Enemy"|| other.gameObject.tag == "EnProj")
        {
            player.getHit(other);
        }
        if (other.tag == "Health")
        {
            //get object in parent to do this
            player.collectHealth(other);
        }
    }
    //If Collider hits another one
    void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.tag == "Enemy" || other.gameObject.tag == "EnProj")
        {
            player.getHit(other);
        }
    }
}
