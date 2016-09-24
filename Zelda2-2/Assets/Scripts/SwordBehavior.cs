﻿using UnityEngine;
using System.Collections;

public class SwordBehavior : MonoBehaviour
{
    PlayMove2 player;
    void Start()
    {
        player = GetComponentInParent<PlayMove2>();
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Enemy")
        {
            //get object in parent to do this
            player.landHit(other);
        }
        if (other.tag == "Break")
        {
            Destroy(other.gameObject, .1f);
        }
        if (other.tag == "Health")
        {
            //get object in parent to do this
            player.collectHealth(other);
        }
    }
}
