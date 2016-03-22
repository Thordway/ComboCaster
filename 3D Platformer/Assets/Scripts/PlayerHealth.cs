using UnityEngine.UI;
using UnityEngine;
using System.Collections;


public class PlayerHealth : MonoBehaviour {
    public float health = 10;
    private float healthMax;
    public Text text;

	// Use this for initialization
	void Start () {
        healthMax = health;
	}
	
	// Update is called once per frame
	void Update () {
	    if(health <= 0) {
            GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>().GameOver();
        }

        text.text = "Health: " + (health*10 / healthMax)*10 +"%";
	}

    public void Damage(float damage) {
        health -= damage;

    }
}
