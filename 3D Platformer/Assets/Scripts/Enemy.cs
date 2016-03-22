using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour {
    public Transform target;
    private NavMeshAgent agent;
    public float attackDistance = 2;

    public bool canAttack = true;
    public float attackTime = 1;
    private float attackTimer;
    public int damage = 1;
    
	// Use this for initialization
	void Start () {

        attackTimer = attackTime;
        agent = GetComponent<NavMeshAgent>();
	}

    // Update is called once per frame
    void Update() {
        if (agent.enabled)
            agent.SetDestination(target.position);

        
        if (canAttack == false) {
            attackTimer -= Time.deltaTime;
            if (attackTimer <= 0) {
                canAttack = true;
                attackTimer = attackTime;
            }    
        }
        else {
            if (Vector3.Distance(transform.position, target.position) <= attackDistance) {
                Attack();
            }
        }
	}

    public void Fireballed(Vector3 point, float force, float radius,float upMod) {
        if (GetComponent<Rigidbody>().isKinematic)
            GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>().numOfEnemies--;
        agent.enabled = false;
        GetComponent<Rigidbody>().isKinematic = false;
        GetComponent<Rigidbody>().AddExplosionForce(force, point, radius, upMod,ForceMode.Impulse);
    }

    public void Attack() {
        if (canAttack) {
            canAttack = false;
            attackTimer = attackTime;
            target.GetComponent<PlayerHealth>().Damage(damage);

        }

    }
}
