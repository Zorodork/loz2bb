using UnityEngine;

public class OrbController : MonoBehaviour {
    [SerializeField]
    private GameObject block, orb; //specify children

    private BoxCollider2D[] blocks; //all switch blocks
    private BoxCollider2D[] orbs; //all orbs

    [SerializeField]
    private Sprite red, blue; //sprites used for orbs
    private bool isRed = false; //if orbs are red
	// Use this for initialization
	void Start () {
        blocks = block.GetComponentsInChildren<BoxCollider2D>();
        orbs = orb.GetComponentsInChildren<BoxCollider2D>();

        //changes blocks to appropriate state
        for (int i = 0; i < blocks.Length; i++)
        {
            //checks to see if block is a Red Block
            if (blocks[i].name.Contains("Red Block"))
                blocks[i].enabled = isRed;
            else //block is Blue
                blocks[i].enabled = !isRed;
            //change the transparency of the block accordingly
            changeAlpha(i);
        }

        //changes orbs to appropriate state
        for (int j = 0; j < orbs.Length; j++)
        {
            SpriteRenderer rend = orbs[j].GetComponent<SpriteRenderer>();
            rend.sprite = (isRed) ? red : blue;
        }
    }
    //switches the current active color
	public void switchColor()
    {
        isRed = !isRed;
        for (int i = 0; i < blocks.Length; i++)
        {
            if (blocks[i] != gameObject.GetComponent<BoxCollider2D>())
            {
                blocks[i].enabled = !blocks[i].enabled;
                changeAlpha(i);
            }
        }
        for (int j = 0; j < orbs.Length; j++)
        {
            SpriteRenderer rend = orbs[j].GetComponent<SpriteRenderer>();
            rend.sprite = (isRed) ? red : blue;
        }
    }
    //changes the alpha of blocks
    void changeAlpha(int i)
    {
        Color col = blocks[i].gameObject.GetComponent<SpriteRenderer>().color;
        if (blocks[i].enabled == false)
            col.a = .25f;
        else
            col.a = 1;
        blocks[i].gameObject.GetComponent<SpriteRenderer>().color = col;
    }
}
