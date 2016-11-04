using UnityEngine;

public class OrbBehavior : MonoBehaviour {
    private OrbController orbc;
    void Start()
    {
        orbc = GetComponentInParent<OrbController>();
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Arrow" || other.tag == "Sword")
        {
            orbc.switchColor();
        }
    }
}
