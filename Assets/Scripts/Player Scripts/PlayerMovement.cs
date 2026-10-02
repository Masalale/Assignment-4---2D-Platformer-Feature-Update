using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {

	public float speed = 5f;

	private Rigidbody2D myBody;
	private Animator anim;

	public Transform groundCheckPosition;
	public LayerMask groundLayer;

	private bool isGrounded;
	private bool jumped;

	private float controlLockTimer;

	public void LockControls (float duration) {
		controlLockTimer = duration;
	}

	private float jumpPower = 12f;

	void Awake() {
		myBody = GetComponent<Rigidbody2D>();
		anim = GetComponent<Animator> ();
	}

	void Start () {
		
	}

	void Update () {
		if (controlLockTimer > 0f) {
			controlLockTimer -= Time.deltaTime;
		}
		CheckIfGrounded();
		PlayerJump();
	}

	void FixedUpdate() {
		PlayerWalk ();
	}

	void PlayerWalk() {

		float h = 0f;
		if (controlLockTimer <= 0f) {
			h = Input.GetAxis("Horizontal"); //replace "0" as the value of float h with the correct axis of movement.
		}
		//Note: The value of h must use the right and left arrow or "a" and "d" keys to move the player
		//right and left.

		if (h > 0) {
			myBody.linearVelocity = new Vector2 (speed, myBody.linearVelocity.y);

			ChangeDirection (1);

		} else if (h < 0) {
			myBody.linearVelocity = new Vector2 (-speed, myBody.linearVelocity.y);

			ChangeDirection (-1);

		} else {
			myBody.linearVelocity = new Vector2 (0f, myBody.linearVelocity.y);
		}

		anim.SetInteger ("Speed", Mathf.Abs((int)myBody.linearVelocity.x));

	}

	void ChangeDirection(int direction) {
		Vector3 tempScale = transform.localScale;
		tempScale.x = direction;
		transform.localScale = tempScale;
	}

	//Checking if the player is on the ground
	void CheckIfGrounded() {
		isGrounded = false;
		RaycastHit2D[] hits = Physics2D.RaycastAll (groundCheckPosition.position, Vector2.down, 0.2f, groundLayer);
		foreach (RaycastHit2D hit in hits) {
			if (!hit.collider.isTrigger) {
				isGrounded = true;
				break;
			}
		}

		if (isGrounded) {
			// and we jumped before
			if (jumped) {
				
				jumped = false;

				anim.SetBool ("Jump", false);
			}
		}

	}

	//Make the player jump
	void PlayerJump() {
		if (isGrounded) {
			if (controlLockTimer <= 0f && Input.GetButtonDown("Jump")) {
				jumped = true;
				myBody.linearVelocity = new Vector2 (myBody.linearVelocity.x, jumpPower);

				anim.SetBool ("Jump", true);
			}
		}
	}

} // class














































