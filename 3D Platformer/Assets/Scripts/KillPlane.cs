using UnityEngine;
using System.Collections;

public class KillPlane : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
    void OnTriggerEnter(Collider other) {
        if (!other.CompareTag("KillPlane")) {
            Destroy(other.gameObject);
        }

    }
    void OnTriggerExit(Collider other) {
        if (!other.CompareTag("KillPlane")) {
            Destroy(other.gameObject);
        }

    }
}
