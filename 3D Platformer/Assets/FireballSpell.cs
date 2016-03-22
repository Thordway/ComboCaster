using UnityEngine;
using System.Collections;

public class FireballSpell : Spell {
    public float fireballChargeTime = 0.9f;
    private Timer fireballChargeTimer;
    public bool fireballCharged = false;
    public GameObject fireballPrefab;

    public override void Start() {
        base.Start();
        fireballChargeTimer = new Timer(fireballChargeTime);
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
        if (fireballCharged == false) {
            if (fireballChargeTimer.RunTimer()) {
                fireballCharged = true;
                fireballChargeTimer.Reset();
            }
        }
    }
    public override void SpellEnd() {
        //spell part
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

        //magickball part
        StartCoroutine(KillMagickBall());

        //rest everything
        fireballCharged = false;
        spellActive = false;

        base.SpellEnd();
    }
}
