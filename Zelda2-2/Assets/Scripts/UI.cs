using UnityEngine;
using System.Collections;

public class UI : MonoBehaviour {

    public Texture2D heart, noheart, paused;
    private PlayMove2 player;
    public AudioClip pause;
    private AudioSource auSource;
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayMove2>();
        auSource = GetComponent<AudioSource>();

    }
    void OnGUI()
    {
        if (!heart || !noheart)
        {
            Debug.LogError("No texture assigned");
        }
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
        if (Time.timeScale == 0)
            GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), paused);

    }
    void Update()
    {
        if (Input.GetKeyDown("p"))
        {
            if (Time.timeScale == 1)
            {
                Time.timeScale = 0;
                auSource.clip = pause;
                auSource.Play();

            }
            else
            {
                Time.timeScale = 1;
                auSource.clip = pause;
                auSource.Play();
            }
        }
    }
}
