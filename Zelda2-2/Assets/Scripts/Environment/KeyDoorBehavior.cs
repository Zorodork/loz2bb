using UnityEngine;

public class KeyDoorBehavior : MonoBehaviour {

    private PlayMove2 player;
    private AudioSource auSource;
    public AudioClip open;
    private bool isOpen;
    [SerializeField]
    private bool isBoss = false;
    // Use this for initialization
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayMove2>();
        auSource = GetComponent<AudioSource>();
        auSource.clip = open;
    }
    void OnCollisionEnter2D(Collision2D other)
    {
        if(other.gameObject.tag == "Player")
        {
            if (isBoss)
            {
                if (player.bossKeys[0] == true)
                {
                    isOpen = true;
                    auSource.Play();
                    Destroy(gameObject, open.length);
                }
            }
            else
            {
                //TODO: replace keys[0] with dungeon number, isOpen to prevent multiple key leeches
                if (player.keys[0] > 0 && !isOpen)
                {

                    isOpen = true;
                    player.keys[0]--;
                    auSource.Play();
                    Destroy(gameObject, open.length);
                }
            }
        }
    }
}
