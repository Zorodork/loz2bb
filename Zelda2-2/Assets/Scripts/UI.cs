using UnityEngine;

public class UI : MonoBehaviour {

    [SerializeField]
    private int dungeonID;
    public Texture2D heart, noheart, key, paused;
    private PlayMove2 player;
    public AudioClip pause;
    private AudioSource auSource;
    private bool isPaused;
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayMove2>();
        auSource = GetComponent<AudioSource>();
        player.dungeonID = dungeonID; //tell player what dungeon they are in
    }
    void OnGUI()
    {
        if (!heart || !noheart)
        {
            Debug.LogError("No texture assigned");
        }
        //draw health here
        int health = player.health;
        int maxHealth = player.maxHealth;
        for(int i = 0; i < maxHealth; i++)
        {
            Texture2D uih;
            if (i < health)
                uih = heart;
            else
                uih = noheart;
            if (i<10)
                GUI.DrawTexture(new Rect(16+i*16, 16, 14, 16), uih);
            else
                GUI.DrawTexture(new Rect(16 +(i-10)*16, 32, 14, 16), uih);
        }
        //draw keys here
        int keys = player.keys[0];
        for(int j = 0; j < keys; j++)
        {
            GUI.DrawTexture(new Rect(Screen.width-(j+2)*16, 16, 8, 16), key);
        }
        if (Time.timeScale == 0 && isPaused)
            GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), paused);

    }
    void Update()
    {
        if (Input.GetKeyDown("p"))
        {
            if (!isPaused && Time.timeScale == 1)
            {
                isPaused = true;
                Time.timeScale = 0;
                auSource.clip = pause;
                auSource.Play();

            }
            else if (isPaused && Time.timeScale == 0)
            {
                isPaused = false;
                Time.timeScale = 1;
                auSource.clip = pause;
                auSource.Play();
            }
        }
    }
}
