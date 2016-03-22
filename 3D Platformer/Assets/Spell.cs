using UnityEngine;
using System.Collections;

public class Spell : MonoBehaviour {
    [System.NonSerialized]
    public SpellManager master;
    [System.NonSerialized]
    public int spellID;

    public string spellName = "Spell";

    public bool selected = false;
    public bool spellActive = false;
    public bool onCoolDown = false;

    public Color primaryColor;
    public Color secoundaryColor;


    private Timer coolDownTimer;
    public float coolDownTime = 1.0f;

    public GameObject magickBallPrefab;
    public Transform magickBallPoint;
    [System.NonSerialized]
    public GameObject magickBall;
    public GameObject magickBallDiePrefab;
    public float magickBallFadeTime = 0.5f;
    // Use this for initialization
    public virtual void Start () {
        coolDownTimer = new Timer(coolDownTime);
    }
	// Update is called once per frame
	public virtual void Update () {
        if (master.selectedSpell == spellID) {
            selected = true;
        }
        else
            selected = false;

	    if (onCoolDown)
            if (coolDownTimer.RunTimer()) {
                onCoolDown = false;
                coolDownTimer.Reset();
            }

	}
    public virtual bool SpellStart() {
        if (onCoolDown == false) {
            return (master.Casting(spellID));
        }
        return (false);
    }
    public virtual void SpellUpdate() {


    }
    public virtual void SpellEnd() {
        master.InEffect(spellID, false);
        master.casting = false;
        onCoolDown = true;
    }
    public IEnumerator KillMagickBall() {
        Destroy(magickBall);
        magickBall = Instantiate(magickBallDiePrefab, magickBallPoint.position, magickBallDiePrefab.transform.rotation) as GameObject;
        magickBall.transform.SetParent(magickBallPoint.parent);
        yield return new WaitForSeconds(magickBallFadeTime);
        Destroy(magickBall);
    }
}
