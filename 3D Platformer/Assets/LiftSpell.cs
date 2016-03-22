using UnityEngine;
using System.Collections;

public class LiftSpell : Spell {
    private GameObject liftingObject;
    public Transform liftPoint;
    public float liftSpeed;
    public bool liftReachedTarget = false;
    public float liftRotationRate = 10;
    public float liftThrowForce = 15000;
    // Use this for initialization
    public override void Start () {
        base.Start();
	}
	
	// Update is called once per frame
	public override void Update () {
        base.Update();
        if (selected) {
            if(Input.GetMouseButtonDown(0)) {
                SpellStart();
            }
            if(Input.GetMouseButtonUp(0) && spellActive) {
                SpellEnd();
            }
        }
	}
    void FixedUpdate() {
        if (spellActive)
            SpellUpdate();
    }
    public override bool SpellStart() {
        if (base.SpellStart()) {
            RaycastHit hit;
            Transform cam = Camera.main.transform;
            if (Physics.Raycast(cam.position, cam.forward, out hit, 500) && hit.collider.CompareTag("PhysicObject")) {
                //spell part
                liftingObject = hit.collider.gameObject;
                liftingObject.GetComponent<Collider>().enabled = true;
                liftingObject.GetComponent<Rigidbody>().useGravity = false;
                liftingObject.GetComponent<Rigidbody>().velocity = ((liftPoint.position - liftingObject.transform.position).normalized * liftSpeed);

                //magickBall part
                magickBallPrefab.GetComponent<ParticleSystem>().startColor = primaryColor;
                magickBallPrefab.transform.GetChild(0).GetComponent<ParticleSystem>().startColor = secoundaryColor;
                magickBallDiePrefab.GetComponent<ParticleSystem>().startColor = primaryColor;
                magickBall = Instantiate(magickBallPrefab, magickBallPoint.position, Quaternion.identity) as GameObject;
                magickBall.transform.SetParent(magickBallPoint.parent);

                spellActive = true;

                return (true);
            }
            else {
                master.casting = false;
            }
        }
        SpellFailed();
        return (false);
    }
    public override void SpellUpdate() {
        //base.SpellUpdate();
        if (liftReachedTarget == false && liftingObject != null) {
            //float liftRotationRate = 10;
            Vector3 currentDirection = liftingObject.GetComponent<Rigidbody>().velocity.normalized;
            Vector3 targetDirection = (liftPoint.position - liftingObject.transform.position).normalized;
            //float currentSpeed = liftingObject.GetComponent<Rigidbody>().velocity.magnitude;
            Vector3 newDir = Vector3.RotateTowards(currentDirection, targetDirection, liftRotationRate * Time.deltaTime, 0.0f);
            newDir *= liftSpeed;
            liftingObject.GetComponent<Rigidbody>().velocity = newDir;

            if (Vector3.Distance(liftingObject.transform.position, liftPoint.position) <= 0.2) {
                liftingObject.GetComponent<Rigidbody>().isKinematic = true;
                liftReachedTarget = true;
                liftingObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
                liftingObject.transform.position = liftPoint.position;
                liftingObject.transform.SetParent(liftPoint);
            }
        }
    }
    public override void SpellEnd() {
        //spellPart
        liftingObject.GetComponent<Collider>().enabled = true;
        liftingObject.GetComponent<Rigidbody>().useGravity = true;
        liftingObject.GetComponent<Rigidbody>().isKinematic = false;
        if (liftingObject.transform.parent == liftPoint) {
            liftingObject.transform.SetParent(null);
            RaycastHit hit;
            Transform cam = Camera.main.transform;
            if (Physics.Raycast(cam.position, cam.forward, out hit, 1000)) {
                liftingObject.GetComponent<Rigidbody>().AddForce((hit.point - liftingObject.transform.position).normalized * liftThrowForce);
            }
            else {
                liftingObject.GetComponent<Rigidbody>().AddForce(cam.position + (cam.forward * liftThrowForce));
            }
        }
        liftReachedTarget = false;
        liftingObject = null;

        spellActive = false;

        //magickBall part
        StartCoroutine(KillMagickBall());


        base.SpellEnd();
    }

    private void SpellFailed() {

    }
}
