using UnityEngine;
using System.Collections;

public class Timer : MonoBehaviour {
    private float theTime;
    private float theTimer;
    private bool trigger;
    public Timer (float time) {
        theTime = theTimer = time;
        trigger = false;
    }
    // Use this for initialization
    public void Reset() {
        theTimer = theTime;
        trigger = false;
	}
	public bool RunTimer() {
        if (trigger == false) {
            theTimer -= Time.deltaTime;
            if (theTimer <= 0)
                trigger =true;
        }
        return (trigger);
    }
}
