using UnityEngine;
using System.Collections;

public class ShieldBlockBash : MonoBehaviour {
    public Shield player;
    public bool blocking = true;
    public bool bashing = false;
    public bool isPlaying = false;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetMouseButtonUp(1)) {
            blocking = false;
        }
        if (blocking == false && GetComponent<Animation>().isPlaying == false && bashing == false) {
            //print("lol");
            GetComponent<Animation>().Play("Bash");
            bashing = true;
        }
        if (bashing) {
            if (GetComponent<Animation>().isPlaying == false) {
                player.hasShield = true;
                Destroy(gameObject);
            }
        }
    }
}
