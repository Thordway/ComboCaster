using UnityEngine;
using System.Collections;

public class Explosion : MonoBehaviour {
    public float radius = 10;
    public float explosionForce = 100;
    public float pExplosionForce = 100;
    public float timeTillDestroy = 1;
    public float upwardsModifier = 5;
    public float pUpwardsModifier = 5;
	// Use this for initialization
	void Start () {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, radius);
        //int i = 0;
        foreach(Collider x in hitColliders) {
			if (x.CompareTag("Enemy")) {
                x.GetComponent<Enemy>().Fireballed(transform.position, explosionForce, radius,upwardsModifier);
            }
			else if(x.CompareTag("PhysicObject")) {
				x.GetComponent<Rigidbody>().AddExplosionForce(explosionForce,transform.position,radius, upwardsModifier,ForceMode.Impulse);
			}
            else if (x.CompareTag("Player")) {
                x.GetComponent<Rigidbody>().AddExplosionForce(pExplosionForce, transform.position, radius, pUpwardsModifier, ForceMode.Impulse);
            }
        }
    }
	
	// Update is called once per frame
	void Update () {
        timeTillDestroy -= Time.deltaTime;
        if (timeTillDestroy <= 0)
            Destroy(gameObject);
	}
}
