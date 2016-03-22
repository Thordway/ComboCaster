using UnityEngine;
using System.Collections;

public class TeleportSpell : Spell {
    public float teleportChargeTime = 0.5f;
    private Timer teleportChargeTimer;
    public bool teleportCharged = false;
    public float teleportDistance = 50.0f;
    public GameObject teleportMarkerPrefab;
    private GameObject teleportMarker;
    private bool teleportClimbUp = false;
    private RaycastHit climbUpHit;
    public override void Start() {
        teleportChargeTimer = new Timer(teleportChargeTime);
        base.Start();
    }
    public override void Update() {
        base.Update();
        if (selected) {
            if (Input.GetMouseButtonDown(0)) {
                SpellStart();
            }
            if (Input.GetMouseButtonUp(0) && spellActive) {
                SpellEnd();
            }
        }
        if (spellActive)
            SpellUpdate();
    }

    public override bool SpellStart() {
        if (base.SpellStart()) {
            //spell part

            //magickball part
            magickBallPrefab.GetComponent<ParticleSystem>().startColor = primaryColor;
            magickBallPrefab.transform.GetChild(0).GetComponent<ParticleSystem>().startColor = secoundaryColor;
            magickBallDiePrefab.GetComponent<ParticleSystem>().startColor = primaryColor;
            magickBall = Instantiate(magickBallPrefab, magickBallPoint.position, Quaternion.identity) as GameObject;
            magickBall.transform.SetParent(magickBallPoint.parent);

            spellActive = true;

            return (true);
        }
        return (false);
    }
    public override void SpellUpdate() {
        //base.SpellUpdate();
        if (teleportCharged == false) {
            if (teleportChargeTimer.RunTimer()) {
                teleportCharged = true;
                teleportChargeTimer.Reset();
                teleportMarker = Instantiate(teleportMarkerPrefab, new Vector3(10000, 10000, 10000), Quaternion.identity) as GameObject;
            }
        }
        else {
            RaycastHit hit;
            Transform cam = Camera.main.transform;
            if (Physics.Raycast(cam.position, cam.forward, out hit, teleportDistance)) {
                teleportMarker.transform.position = hit.point + (hit.normal * GetComponent<Collider>().bounds.extents.y);
                if (hit.normal.y == 0) {
                    teleportMarker.transform.LookAt(hit.point);
                    if (Physics.Raycast(teleportMarker.transform.position, teleportMarker.transform.forward + new Vector3(0, 1, 0), 2)) {
                        teleportMarker.GetComponent<MeshRenderer>().enabled = false;
                        teleportClimbUp = false;
                    }
                    else {
                        Vector3 tempPos = teleportMarker.transform.position + (teleportMarker.transform.forward + new Vector3(0, 1, 0) * 2);

                        if (Physics.Raycast(tempPos, Vector3.down, out climbUpHit, 2)) {
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
                teleportMarker.transform.position = transform.position + (cam.forward * teleportDistance);
            }
        }
}
    public override void SpellEnd() {
        if (teleportCharged) {
            if (teleportClimbUp == false) {
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
        StartCoroutine(KillMagickBall());

        spellActive = false;

        base.SpellEnd();
    }
}
