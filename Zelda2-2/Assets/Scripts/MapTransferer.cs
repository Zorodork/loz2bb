using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class MapTransferer : MonoBehaviour {
    [SerializeField]
    private int sceneIndex;
    void OnTriggerEnter2D(Collider2D other)
    {
        print("woah");
        if (other.tag == "Player")
            print("transferring...");
            SceneManager.LoadSceneAsync(sceneIndex);
    }
}
