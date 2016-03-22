using UnityEngine;
using System.Collections;

public class Fireball : MonoBehaviour {
    public float speed = 20;
    public GameObject explosionPrefab;
	// Use this for initialization
	void Start () {
        //GetComponent<Rigidbody>().isKinematic = true;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void Fly(Vector3 targetPoint) {
        Vector3 dir = (targetPoint - transform.position).normalized;
        GetComponent<Rigidbody>().isKinematic = false;
        GetComponent<Rigidbody>().velocity = dir * speed;
    }

    void OnTriggerEnter(Collider other) {
        if (!(other.CompareTag("Player")|| other.CompareTag("KillPlane")) )
            Explode();

    }
    public void Explode() {
        Instantiate(explosionPrefab, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
