using UnityEngine;
using System.Collections;

public class ChestBehavior : MonoBehaviour {
    [SerializeField]
    private GameObject treasure; //what item is inside the chest
    private float velocity = 6; //velocity of item as it comes out of chest

    //open the chest!!
	public void open()
    {
        GameObject inChest = Instantiate(treasure);
        inChest.GetComponent<Rigidbody2D>().velocity = Vector2.up * velocity;
        Destroy(gameObject);
    }
}
