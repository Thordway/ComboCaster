using UnityEngine;
using System.Collections;

public class MovingPlatform : MonoBehaviour {
    public Transform startPoint;

    public Transform endPoint;
    public bool toEndPoint = true;
    public bool toStartPoint = false;

    public float speed = 10;
    public float positionDistance = 1;
	// Use this for initialization
	void Start () {
        transform.position = startPoint.position;
	}
	
	// Update is called once per frame
	void Update () {

        if (toStartPoint) {
            transform.position = Vector3.Lerp(transform.position, endPoint.position, Time.deltaTime * speed);
            if (Vector3.Distance(transform.position, endPoint.position) <= positionDistance) {
                toStartPoint = false;
                toEndPoint = true;
            }
        }
        else if (toEndPoint) {
            transform.position = Vector3.Lerp(transform.position, startPoint.position, Time.deltaTime * speed);
            if (Vector3.Distance(transform.position, startPoint.position) <= positionDistance) {
                toStartPoint = true;
                toEndPoint = false;
            }
        }
    }

    void OnCollisionEnter(Collision colide) {
        if (colide.collider.CompareTag("Player")) {
            colide.transform.SetParent(transform);
        }
    }
    void OnCollisionExit (Collision colide) {
        if (colide.collider.CompareTag("Player")) {
            colide.transform.parent = null;
        }
    }
}
