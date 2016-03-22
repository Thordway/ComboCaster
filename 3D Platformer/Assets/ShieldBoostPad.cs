using UnityEngine;
using System.Collections;

public class ShieldBoostPad : MonoBehaviour {
    public float speed = 10;
    public float turnRate = 10;
    private Vector3 currentDirection;
    private Vector3 targetDirection;
    public Transform player;

    public Vector3 targetPoint;

    public bool returningToPlayer = false;
    public bool projectile = false;
    public bool boostPad = false;

    public float boostPadForce = 1000;
    public float outOfSightGrabRange = 5.0f;
    public GameObject particleSystemPrefab;
    private GameObject particleSystem;
    // Use this for initialization
    void Start () {
        
    }
	
	// Update is called once per frame
	void Update () {
        if (projectile == true) {
            transform.GetChild(0).transform.Rotate(new Vector3(0, 2, 0) * 360 * Time.deltaTime);

            transform.LookAt(transform.position + GetComponent<Rigidbody>().velocity.normalized);

            currentDirection = GetComponent<Rigidbody>().velocity.normalized;
            GetComponent<Rigidbody>().velocity = currentDirection * speed;
            Vector3 targetDirection;
            if (returningToPlayer)
                targetDirection = (player.position - transform.position).normalized;
            else
                targetDirection = (targetPoint - transform.position).normalized;

            float currentSpeed = GetComponent<Rigidbody>().velocity.magnitude;
            Vector3 newDir = Vector3.RotateTowards(currentDirection, targetDirection, turnRate * Time.deltaTime, 0.0f);
            newDir *= currentSpeed;
            GetComponent<Rigidbody>().velocity = newDir;

            if (returningToPlayer == false && boostPad == false && Vector3.Distance(transform.position,targetPoint) <= 1) {
                
                transform.rotation = Quaternion.identity;
                GetComponent<Rigidbody>().isKinematic = true;
                GetComponent<Collider>().isTrigger = true;
                particleSystem = Instantiate(particleSystemPrefab, transform.position, particleSystemPrefab.transform.rotation) as GameObject;
                particleSystem.transform.SetParent(transform);
                projectile = false;
                boostPad = true;
            }
            if (returningToPlayer == false) {
                if (GetComponentInChildren<Renderer>().isVisible == false && Vector3.Distance(transform.position, player.position) <= outOfSightGrabRange) {
                    //this.enabled = false;
                    //GetComponent<Rigidbody>().velocity = Vector3.zero;
                    player.GetComponent<Shield>().hasShield = true;
                    Destroy(gameObject);
                }
            }
        }
        else if (boostPad) {
            
        }
	}
    public void Project() {
        GetComponent<Collider>().enabled = false;
        Color temp;
        temp = GetComponentInChildren<MeshRenderer>().material.color;
        temp.a = 0.5f;
        GetComponentInChildren<MeshRenderer>().material.color = temp;
        this.enabled = false;
    }
    public void Throw(Vector3 theTargetPoint) {
        targetPoint = theTargetPoint;
        GetComponent<Rigidbody>().velocity = (targetPoint - transform.position).normalized * speed;
        projectile = true;
    }
    public void Return() {
        Destroy(particleSystem);
        returningToPlayer = true;
        GetComponent<Rigidbody>().isKinematic = false;
        GetComponent<Rigidbody>().velocity = (player.position - transform.position).normalized*speed;
        boostPad = false;
        projectile = true;
        GetComponent<Collider>().isTrigger = false;

    }
    void OnCollisionEnter(Collision col) {
        if (returningToPlayer)
            if (col.collider.CompareTag("Player")) {
                col.collider.GetComponent<Shield>().hasShield = true;
                Destroy(gameObject);
            }
    }
    void OnTriggerEnter(Collider other) {
        if (boostPad)
            if (other.CompareTag("Player")) {
                Vector3 temp;
                temp = other.GetComponent<Rigidbody>().velocity;
                temp.y = 0;
                other.GetComponent<Rigidbody>().velocity = temp;
                other.GetComponent<Rigidbody>().AddForce(Vector3.up * boostPadForce);
            }
    }
}
