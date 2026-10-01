using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

	[Header("Respawn")]
	public float respawnBackDistance = 1f;
	public float respawnLift = 1f;
	public float controlLockDuration = 2f;

	[Header("Scenes")]
	public string gameplaySceneName = "GameScene-ALU";
	public string endSceneName = "EndScene";

	private GameObject player;
	private PlayerDamage playerDamage;
	private PlayerMovement playerMovement;
	private int waterLayer;

	void Start () {
		waterLayer = LayerMask.NameToLayer ("Water");

		player = GameObject.FindGameObjectWithTag (MyTags.PLAYER_TAG);
		if (player != null) {
			playerDamage = player.GetComponent<PlayerDamage> ();
			playerMovement = player.GetComponent<PlayerMovement> ();
		}
	}

	void OnTriggerEnter2D (Collider2D target) {
		if (target.gameObject.layer == waterLayer) {
			TryWaterRespawn (target);
		}
	}

	void OnTriggerStay2D (Collider2D target) {
		if (target.gameObject.layer == waterLayer) {
			TryWaterRespawn (target);
		}
	}

	void TryWaterRespawn (Collider2D waterCollider) {
		float waterTop = waterCollider.bounds.max.y;
		if (player.transform.position.y < waterTop) {
			HandleWaterEntry (waterCollider.transform);
		}
	}

	void HandleWaterEntry (Transform water) {
		if (playerDamage != null) {
			playerDamage.DealDamage ();

			if (playerDamage.LivesRemaining > 0) {
				RespawnNearWater (water);
			} else {
				SceneManager.LoadScene (endSceneName);
			}
		}
	}

	void RespawnNearWater (Transform water) {
		Rigidbody2D rb = player.GetComponent<Rigidbody2D> ();

		float dir = Mathf.Sign (rb.linearVelocity.x);
		if (dir == 0f) {
			dir = Mathf.Sign (player.transform.localScale.x);
		}
		if (dir == 0f) {
			dir = -1f;
		}

		Vector3 respawnPos = water.position;
		respawnPos.x -= dir * respawnBackDistance;
		respawnPos.y += respawnLift;
		player.transform.position = respawnPos;
		rb.linearVelocity = Vector2.zero;

		if (playerMovement != null) {
			playerMovement.LockControls (controlLockDuration);
		}
	}

	// Wire to the Replay button onClick in the End scene.
	public void ReplayGame () {
		Time.timeScale = 1f;
		SceneManager.LoadScene (gameplaySceneName);
	}

	// Wire to the Quit button onClick in the End scene.
	public void QuitGame () {
		Application.Quit ();
	}

} // class
