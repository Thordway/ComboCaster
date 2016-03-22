using UnityEngine;
using System.Collections;

public class Shield : MonoBehaviour {

    public int mode = 1;// 0 is shield and shield bash, 1 is throw shield, 2 is launch pad, 3 is force wall
    public bool hasShield = true;
    public GameObject UIShield;
    public GameObject shieldProjectilePrefab;
    private GameObject shieldProjectile;
    public Transform shieldPoint;

    public GameObject blockShieldPrefab;
    private GameObject blockShield;

    public GameObject boostPadShieldPrefab;
    private GameObject boostPadShield;

    public GameObject shieldWallPrefab;
    private GameObject shieldWall;

    public float projectDistance = 25;

	// Use this for initialization
	void Start () {
        hasShield = true;
	}
	
	// Update is called once per frame
	void Update () {
        UIShield.GetComponent<MeshRenderer>().enabled = hasShield;
	    if (hasShield) {
            if (Input.GetKeyDown(KeyCode.Tab)) {
                mode++;
                if (mode == 4)
                    mode = 0;
            }
            if(Input.GetMouseButtonDown(1)) {
                if (mode == 0) {
                    hasShield = false;
                    ShieldBlock();
                }
                else if (mode == 1) {
                    ThrowShield();
                }
            }
            if (Input.GetMouseButton(1)) {
                if (mode == 2 || mode == 3) {
                    ProjectTarget(mode);
                }
            }
            if (Input.GetMouseButtonUp(1)) {
                if (mode == 2 || mode == 3) {
                    ThrowShieldOther(mode);
                }
            }
        }
        else {
            if (Input.GetMouseButtonDown(1)) {
                if (mode == 2 || mode == 3) {
                    ReturnShield(mode);
                }
            }
        }
	}

    public void ThrowShield() {
        hasShield = false;
        shieldProjectile = Instantiate(shieldProjectilePrefab, shieldPoint.position, Quaternion.identity) as GameObject;

        RaycastHit hit;
        Transform cam = Camera.main.transform;
       if (Physics.Raycast(cam.position, cam.forward, out hit, 1000))
            shieldProjectile.GetComponent<ShieldProjectile>().Initiate(hit.point);
        else
            shieldProjectile.GetComponent<ShieldProjectile>().Initiate(cam.position + (cam.forward*1000));

        shieldProjectile.GetComponent<ShieldProjectile>().player = transform;

    }

    public void ShieldBlock() {
        hasShield = false;
        blockShield = Instantiate(blockShieldPrefab, shieldPoint.position, shieldPoint.rotation) as GameObject;
        blockShield.transform.SetParent(shieldPoint.parent);
        blockShield.GetComponent<ShieldBlockBash>().player = this;
    }
    public void ProjectTarget (int type) {
        if (type == 2) {
            if (boostPadShield == null) {
                boostPadShield = Instantiate(boostPadShieldPrefab, shieldPoint.position, Quaternion.identity) as GameObject;
                boostPadShield.GetComponent<ShieldBoostPad>().Project();
            }
            else {
                RaycastHit hit;
                Transform cam = Camera.main.transform;
                if (Physics.Raycast(cam.position, cam.forward, out hit, projectDistance)) {
                    //shieldProjectile.GetComponent<ShieldProjectile>().Initiate(hit.point);
                    //boostPadShield.transform.position = hit.point + (hit.normal * boostPadShield.GetComponent<Collider>().bounds.extents.z);
                    boostPadShield.transform.position = hit.point + (hit.normal * 0.5f);
                }
                else {
                    //shieldProjectile.GetComponent<ShieldProjectile>().Initiate(cam.position + (cam.forward * 1000));
                    boostPadShield.transform.position = cam.position + (cam.forward * projectDistance);
                }
            }
        }
        else if (type == 3) {

        }
    }
    public void ThrowShieldOther (int type) {
        hasShield = false;
        if (type == 2) {
            Vector3 temp;
            temp = boostPadShield.transform.position;
            Destroy(boostPadShield);
            boostPadShield = Instantiate(boostPadShieldPrefab, shieldPoint.position, Quaternion.identity) as GameObject;
            boostPadShield.GetComponent<ShieldBoostPad>().Throw(temp);
            boostPadShield.GetComponent<ShieldBoostPad>().player = transform;
        }
        else if (type == 3) {

        }
    }
    public void ReturnShield(int type) {
        if (type == 2) {
            boostPadShield.GetComponent<ShieldBoostPad>().Return();
        }
        else if (type == 3) {

        }
    }
}
