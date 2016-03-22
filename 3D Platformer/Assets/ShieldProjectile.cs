using UnityEngine;
using System.Collections;

public class ShieldProjectile : MonoBehaviour {
    public float speed = 10;
    public float turnRate = 10;
    private Vector3 currentDirection;
    private Vector3 targetDirection;
    public Transform player;

    public float forwardTime = 2.0f;
    public float returnTime = 3.0f;
    private Timer forwardTimer;
    public bool rotate = false;

    public float outOfSightGrabRange = 5.0f;
	// Use this for initialization
	void Start () {
        forwardTimer = new Timer(forwardTime);
	}
	
	// Update is called once per frame
	void Update () {
        transform.GetChild(0).transform.Rotate(new Vector3(0, 2, 0) * 360 * Time.deltaTime);
        transform.LookAt(transform.position + GetComponent<Rigidbody>().velocity.normalized);

        currentDirection = GetComponent<Rigidbody>().velocity.normalized;
        GetComponent<Rigidbody>().velocity = currentDirection * speed;
        if (rotate) {
            Vector3 targetDirection = (player.position - transform.position).normalized;
            float currentSpeed = GetComponent<Rigidbody>().velocity.magnitude;
            Vector3 newDir = Vector3.RotateTowards(currentDirection, targetDirection, turnRate * Time.deltaTime, 0.0f);
            newDir *= currentSpeed;
            GetComponent<Rigidbody>().velocity = newDir;
            if (GetComponentInChildren<Renderer>().isVisible == false && Vector3.Distance(transform.position,player.position) <= outOfSightGrabRange) {
                //this.enabled = false;
                //GetComponent<Rigidbody>().velocity = Vector3.zero;
                player.GetComponent<Shield>().hasShield = true;
                Destroy(gameObject);

            }
            if (forwardTimer.RunTimer()) {
                player.GetComponent<Shield>().hasShield = true;
                Destroy(gameObject);
            }
        }
        else {
            if (forwardTimer.RunTimer()) {
                rotate = true;
                forwardTimer = new Timer(returnTime);
            }
        }





        //float step;

        //step = lifeRotationRate * Time.deltaTime;
        //Vector3 newDir = Vector3.RotateTowards(currentDirection, targetDirection, lifeRotationRate * Time.deltaTime, 0.0f);
        //newDir *= liftSpeed;
        //liftingObject.GetComponent<Rigidbody>().velocity = newDir;
    }

    public void Initiate(Vector3 targetPoint) {
        Vector3 dir = (targetPoint - transform.position).normalized;
        GetComponent<Rigidbody>().velocity = dir * speed;

    }
    void OnCollisionEnter(Collision col) {
        if (rotate)
        if (col.collider.CompareTag("Player")) {
            col.collider.GetComponent<Shield>().hasShield = true;
            Destroy(gameObject);
        }
    }
}
