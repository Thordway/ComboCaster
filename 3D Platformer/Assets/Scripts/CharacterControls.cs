using UnityEngine;
using System.Collections;

[RequireComponent (typeof (Rigidbody))]
[RequireComponent (typeof (CapsuleCollider))]

public class CharacterControls : MonoBehaviour {

	public float speed = 10.0f;
	public float gravity = 10.0f;
	public float maxGroundAccel = 10.0f;
	public float maxAirAccel = 0.1f;
	public bool canJump = true;
	public float jumpHeight = 2.0f;
	private bool grounded = false;


	public Camera cam;
	public float camSpeedH = 2.0f;
	public float camSpeedV = 2.0f;
	private float yaw = 0.0f;
	public Vector2 minMaxPitch = new Vector2(-50,50);
	private float pitch = 0.0f;


	void Awake () {
		GetComponent<Rigidbody>().freezeRotation = true;
		GetComponent<Rigidbody>().useGravity = false;
	}
	void Update() {
		yaw += camSpeedH * Input.GetAxis("Mouse X")*Time.deltaTime;
		pitch -= camSpeedV * Input.GetAxis("Mouse Y")*Time.deltaTime;
		pitch = Mathf.Clamp(pitch,minMaxPitch.x,minMaxPitch.y);
		cam.transform.eulerAngles = new Vector3(pitch,cam.transform.eulerAngles.y, 0.0f);
		transform.eulerAngles = new Vector3(0.0f,yaw,0.0f);
		//Mathf.Clamp(cam.transform.eulerAngles.x,minMaxPitch.x,minMaxPitch.y);
	}
	void FixedUpdate () {
		// Calculate how fast we should be moving
		Vector3 targetVelocity = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
		targetVelocity = transform.TransformDirection(targetVelocity);
		targetVelocity *= speed;

		if (grounded) {
			// Apply a force that attempts to reach our target velocity
			Vector3 velocity = GetComponent<Rigidbody>().velocity;
			Vector3 velocityChange = (targetVelocity - velocity);
			velocityChange.x = Mathf.Clamp(velocityChange.x, -maxGroundAccel, maxGroundAccel);
			velocityChange.z = Mathf.Clamp(velocityChange.z, -maxGroundAccel, maxGroundAccel);
			velocityChange.y = 0;
			GetComponent<Rigidbody>().AddForce(velocityChange, ForceMode.VelocityChange);

			// Jump
			if (canJump && Input.GetButton("Jump")) {
				GetComponent<Rigidbody>().velocity = new Vector3(velocity.x, CalculateJumpVerticalSpeed(), velocity.z);
			}
			//print(GetComponent<Rigidbody>().velocity);
		}
		else {
			// Apply a force that attempts to reach our target velocity
			Vector3 velocity = GetComponent<Rigidbody>().velocity;
			Vector3 velocityChange = (targetVelocity - velocity);
			velocityChange.x = Mathf.Clamp(velocityChange.x, -maxAirAccel, maxAirAccel);
			velocityChange.z = Mathf.Clamp(velocityChange.z, -maxAirAccel, maxAirAccel);
			velocityChange.y = 0;
			GetComponent<Rigidbody>().AddForce(velocityChange, ForceMode.VelocityChange);
		}

		// We apply gravity manually for more tuning control
		GetComponent<Rigidbody>().AddForce(new Vector3 (0, -gravity * GetComponent<Rigidbody>().mass, 0));

		grounded = false;

	}

	void OnCollisionStay (Collision collision) {
		if (collision.contacts.Length> 0) {
			ContactPoint contact = collision.contacts[0];
			if (Vector3.Dot(contact.normal, Vector3.up) > 0.5) {
				//collision was from below
				grounded = true;
			}
		}

			    
	}

	float CalculateJumpVerticalSpeed () {
		// From the jump height and gravity we deduce the upwards speed 
		// for the character to reach at the apex.
		return Mathf.Sqrt(2 * jumpHeight * gravity);
	}
}