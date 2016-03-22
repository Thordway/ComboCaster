using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PowerScript : MonoBehaviour {

    public enum SpellType {lift = 0, fireball=1,teleport=2,none = 3};
    public SpellType currentSpell = 0;

	public bool usingSpell = false;
	public bool spellEnding = false;


    public GameObject magickBallPrefab;
	public Transform magickBallPoint;
	public Transform magickBallParent;
	private GameObject magickBall;
	public GameObject magickBallDiePrefab;
	public float magickBallFadeTime = 0.5f;

    public Animation anim;

    private GameObject liftingObject;
    public Transform liftPoint;
    //public float liftForce = 650;
    public float liftSpeed;
    public bool liftReachedTarget = false;
    public float lifeRotationRate = 10;
    public Color[] liftColor;

    public Color[] fireballColor;
	public Color[] teleportColor;

    public float fireballChargeTime = 0.9f;
    private float fireballChargeTimer;
    public bool fireballCharged = false;
    public GameObject fireballPrefab;

	public float teleportChargeTime = 0.5f;
	private float teleportChargeTimer;
	public bool teleportCharged = false;
	public float teleportDistance = 50.0f;
	public GameObject teleportMarkerPrefab;
	private GameObject teleportMarker;
    private bool teleportClimbUp = false;
    private RaycastHit climbUpHit;


	public RectTransform spellHighlight;
	public float spellHighlightStartX;
	public float spellHighlightIncrement;
    // Use this for initialization
    void Start () {
		currentSpell = SpellType.lift;
        fireballChargeTimer = fireballChargeTime;
		teleportChargeTimer = teleportChargeTime;
		spellHighlightStartX = spellHighlight.anchoredPosition.x;
	}
	
	// Update is called once per frame
	void Update () {
		SelectSpell();
		if (Input.GetMouseButtonDown(0) && usingSpell == false) {
            anim.Play();
			if (SpellStart((int)currentSpell)){
				usingSpell = true;
			}
            /*if (SpellStart(1) || SpellStart(2)) {
                if (currentSpell == SpellType.lift) {
                    magickBallPrefab.GetComponent<ParticleSystem>().startColor = liftColor[0];
                    magickBallPrefab.transform.GetChild(0).GetComponent<ParticleSystem>().startColor = liftColor[1];
                    magickBallDiePrefab.GetComponent<ParticleSystem>().startColor = liftColor[0];
                }
                else {
                    magickBallPrefab.GetComponent<ParticleSystem>().startColor = fireballColor[0];
                    magickBallPrefab.transform.GetChild(0).GetComponent<ParticleSystem>().startColor = fireballColor[1];
                    magickBallDiePrefab.GetComponent<ParticleSystem>().startColor = fireballColor[0];
                }
                magickBall = Instantiate(magickBallPrefab, magickBallPoint.position, Quaternion.identity) as GameObject;
                magickBall.transform.SetParent(magickBallParent);
                done = false;
            }*/
          
        }
		if (Input.GetMouseButtonUp(0)) {
			SpellEnd((int)currentSpell);
			/*if (currentSpell != SpellType.none && done == false) {
				StartCoroutine(KillMagickBall());
				if (currentSpell == SpellType.lift)
					SpellEnd(1);
				else if (currentSpell == SpellType.fireball)
					SpellEnd(2);
			}

			currentSpell = 0;
			*/
		}
			
		if (currentSpell != SpellType.lift)
			SpellUpdate((int)currentSpell);

        
	}
    void FixedUpdate() {
		if (currentSpell == SpellType.lift)
			SpellUpdate((int)currentSpell);
    }
	public void SelectSpell() {
		if (Input.GetKeyDown(KeyCode.Alpha1)) {
			if (currentSpell != SpellType.lift && usingSpell)
				SpellEnd((int)currentSpell);
			currentSpell = SpellType.lift;
			float temp = spellHighlightStartX+(spellHighlightIncrement*(0));
			spellHighlight.anchoredPosition = new Vector2(temp,spellHighlight.anchoredPosition.y);
		}
		if (Input.GetKeyDown(KeyCode.Alpha2)) {
			if (currentSpell != SpellType.fireball && usingSpell)
				SpellEnd((int)currentSpell);
			currentSpell = SpellType.fireball;
			float temp = spellHighlightStartX+(spellHighlightIncrement*(1));
			spellHighlight.anchoredPosition = new Vector2(temp,spellHighlight.anchoredPosition.y);
		}
		if (Input.GetKeyDown(KeyCode.Alpha3)) {
			if (currentSpell != SpellType.teleport && usingSpell)
				SpellEnd((int)currentSpell);
			currentSpell = SpellType.teleport;
			float temp = spellHighlightStartX+(spellHighlightIncrement*(2));
			spellHighlight.anchoredPosition = new Vector2(temp,spellHighlight.anchoredPosition.y);
		}
		if (Input.GetKeyDown(KeyCode.Alpha4)) {
			if (currentSpell != SpellType.none && usingSpell)
				SpellEnd((int)currentSpell);
			currentSpell = SpellType.none;
			float temp = spellHighlightStartX+(spellHighlightIncrement*(3));
			spellHighlight.anchoredPosition = new Vector2(temp,spellHighlight.anchoredPosition.y);
		}
	}
    public bool SpellStart(int spell) {
        if (spell == 0) {//lifting
            RaycastHit hit;
            Transform cam = Camera.main.transform;
            if (Physics.Raycast(cam.position, cam.forward, out hit, 500)) {
                if (hit.collider.CompareTag("PhysicObject")) {
					//spell part
                    liftingObject = hit.collider.gameObject;
                    liftingObject.GetComponent<Collider>().enabled = true;
                    liftingObject.GetComponent<Rigidbody>().useGravity = false;
                    liftingObject.GetComponent<Rigidbody>().velocity = ((liftPoint.position - liftingObject.transform.position).normalized * liftSpeed);

					//magickBall part
					magickBallPrefab.GetComponent<ParticleSystem>().startColor = liftColor[0];
					magickBallPrefab.transform.GetChild(0).GetComponent<ParticleSystem>().startColor = liftColor[1];
					magickBallDiePrefab.GetComponent<ParticleSystem>().startColor = liftColor[0];
					magickBall = Instantiate(magickBallPrefab, magickBallPoint.position, Quaternion.identity) as GameObject;
					magickBall.transform.SetParent(magickBallParent);

					return(true);
                }
            }
        }
		else if (spell == 1) {// fireball
			//spell part

			//magickball part
			magickBallPrefab.GetComponent<ParticleSystem>().startColor = fireballColor[0];
			magickBallPrefab.transform.GetChild(0).GetComponent<ParticleSystem>().startColor = fireballColor[1];
			magickBallDiePrefab.GetComponent<ParticleSystem>().startColor = fireballColor[0];
			magickBall = Instantiate(magickBallPrefab, magickBallPoint.position, Quaternion.identity) as GameObject;
			magickBall.transform.SetParent(magickBallParent);
            return (true);
        }
		else if (spell == 2) {//teleport
			//spell part

			//magickball part
			magickBallPrefab.GetComponent<ParticleSystem>().startColor = teleportColor[0];
			magickBallPrefab.transform.GetChild(0).GetComponent<ParticleSystem>().startColor = teleportColor[1];
			magickBallDiePrefab.GetComponent<ParticleSystem>().startColor = teleportColor[0];
			magickBall = Instantiate(magickBallPrefab, magickBallPoint.position, Quaternion.identity) as GameObject;
			magickBall.transform.SetParent(magickBallParent);
			return (true);
		}
        return (false);
    }

    public void SpellUpdate(int spell) {
		if (spellEnding == false && usingSpell) {
        	if (spell == 0) {//lift
            	if (liftReachedTarget == false && liftingObject!= null) {
                	float lifeRotationRate = 10;
                	Vector3 currentDirection = liftingObject.GetComponent<Rigidbody>().velocity.normalized;
                	Vector3 targetDirection = (liftPoint.position - liftingObject.transform.position).normalized;
                	//float currentSpeed = liftingObject.GetComponent<Rigidbody>().velocity.magnitude;
                	float step;

                	step = lifeRotationRate * Time.deltaTime;
                	Vector3 newDir = Vector3.RotateTowards(currentDirection, targetDirection, step, 0.0f);
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
        	else if (spell == 1) {//fireball
            	if (fireballCharged == false) {
                	fireballChargeTimer -= Time.deltaTime;
                	if (fireballChargeTimer <= 0) {
                    	fireballCharged = true;
                	}
            	}

        	}
			else if (spell == 2) {//teleport
				if (teleportCharged == false) {
					teleportChargeTimer -= Time.deltaTime;
					if(teleportChargeTimer <= 0) {
						teleportCharged = true;
						teleportMarker = Instantiate(teleportMarkerPrefab, new Vector3(10000,10000,10000), Quaternion.identity) as GameObject;
					}
				}
				else {
					RaycastHit hit;
					Transform cam = Camera.main.transform;
					if (Physics.Raycast(cam.position, cam.forward, out hit, teleportDistance)) {
						teleportMarker.transform.position = hit.point+ (hit.normal*GetComponent<Collider>().bounds.extents.y);
                        if (hit.normal.y == 0) {
                            teleportMarker.transform.LookAt(hit.point);
                            if (Physics.Raycast(teleportMarker.transform.position, teleportMarker.transform.forward + new Vector3(0, 1, 0), 2)) {
                                teleportMarker.GetComponent<MeshRenderer>().enabled = false;
                                teleportClimbUp = false;
                            }
                            else {
                                Vector3 tempPos = teleportMarker.transform.position + (teleportMarker.transform.forward + new Vector3(0, 1, 0) * 2);
                                
                                if (Physics.Raycast(tempPos,Vector3.down,out climbUpHit, 2)) {
                                    teleportMarker.GetComponent<MeshRenderer>().enabled = true;
                                    teleportClimbUp = true;
                                }
                                else {
                                    teleportMarker.GetComponent<MeshRenderer>().enabled = false;
                                    teleportClimbUp = false;
                                }
                            }
                        }
						//if (Vector3.Angle(hit.normal,Vector3.up) <= 5)
						//transform.position = hit.point + new Vector3(0,GetComponent<Collider>().bounds.extents.y,0);
					}
					else {
                        teleportClimbUp = false;
                        teleportMarker.GetComponent<MeshRenderer>().enabled = false;
                        teleportMarker.transform.position = transform.position + (cam.forward*teleportDistance);
					}
				}
    		}
		}
	}

    public void SpellEnd(int spell) {
		spellEnding = true;
		if (usingSpell) {
        	if (spell == 0) {//lift
				//spellPart
            	liftingObject.GetComponent<Collider>().enabled = true;
            	liftingObject.GetComponent<Rigidbody>().useGravity = true;
            	liftingObject.GetComponent<Rigidbody>().isKinematic = false;
            	if (liftingObject.transform.parent == liftPoint) {
                	liftingObject.transform.SetParent(null);
                	RaycastHit hit;
                	Transform cam = Camera.main.transform;
                	if (Physics.Raycast(cam.position, cam.forward, out hit, 1000)) {
                    	liftingObject.GetComponent<Rigidbody>().AddForce((hit.point - liftingObject.transform.position).normalized * 1500);
                	}
                	else {
                    	liftingObject.GetComponent<Rigidbody>().AddForce(cam.position + (cam.forward * 1500));
                	}
            	}
            	liftReachedTarget = false;
            	liftingObject = null;

				//magickBall part
				StartCoroutine(KillMagickBall());


        	}
        	else if (spell == 1) {//fireball
            	if (fireballCharged) {
                	GameObject temp;
                	temp = Instantiate(fireballPrefab, magickBallPoint.position, Quaternion.identity) as GameObject;
                	RaycastHit hit;
                	Transform cam = Camera.main.transform;
                	if (Physics.Raycast(cam.position, cam.forward, out hit, 1000))
                    	temp.GetComponent<Fireball>().Fly(hit.point);
                	else
                	    temp.GetComponent<Fireball>().Fly(cam.position + (cam.forward * 1500));
            	}
				StartCoroutine(KillMagickBall());
            	fireballCharged = false;
            	fireballChargeTimer = fireballChargeTime;
        	}
			else if (spell == 2) {//teleport
				if (teleportCharged) {
                    //RaycastHit hit;
                    //Transform cam = Camera.main.transform;
                    //if (Physics.Raycast(cam.position, cam.forward, out hit, teleportDistance)) {
                    //transform.position = hit.point+ (hit.normal*GetComponent<Collider>().bounds.extents.y);
                    //if (Vector3.Angle(hit.normal,Vector3.up) <= 5)
                    //transform.position = hit.point + new Vector3(0,GetComponent<Collider>().bounds.extents.y,0);
                    //}
                    //else {
                    //transform.position = transform.position + (cam.forward*teleportDistance);
                    //}
                    if (teleportClimbUp== false) {
                        transform.position = teleportMarker.transform.position;
                    }
                    else {
                        transform.position = climbUpHit.point + (climbUpHit.normal * GetComponent<Collider>().bounds.extents.y);
                    }
                    
					Destroy(teleportMarker);
					teleportMarker = null;

				}
                teleportClimbUp = false;
                teleportCharged = false;
				teleportChargeTimer = teleportChargeTime;
				StartCoroutine(KillMagickBall());

			}
		}
    }

	public IEnumerator KillMagickBall() {
        Destroy(magickBall);
        magickBall = Instantiate(magickBallDiePrefab, magickBallPoint.position, magickBallDiePrefab.transform.rotation) as GameObject;
        magickBall.transform.SetParent(magickBallParent);
        yield return new WaitForSeconds(magickBallFadeTime);
        Destroy(magickBall);
		usingSpell = false;
		spellEnding = false;
	}
}
