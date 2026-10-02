using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour {

	[Header("Respawn")]
	public float respawnBackDistance = 1f;
	public float respawnLift = 1f;
	public float controlLockDuration = 2f;

	[Header("Scenes")]
	public string gameplaySceneName = "GameScene-ALU";
	public string endSceneName = "EndScene";
	public string mainMenuSceneName = "StartScene";

	[Header("Pause")]
	public GameObject pauseMenuCanvas;

	[Header("Run Timer")]
	private float elapsed;
	private TextMeshProUGUI timerText;

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

		elapsed = 0f;
		GameObject timerObj = GameObject.Find ("TimerText");
		if (timerObj != null) {
			timerText = timerObj.GetComponent<TextMeshProUGUI> ();
		}
		UpdateTimerText ();
	}

	void Update () {
		if (Input.GetKeyDown (KeyCode.Escape)) {
			TogglePause ();
		}

		elapsed += Time.deltaTime;
		UpdateTimerText ();
	}

	void UpdateTimerText () {
		if (timerText != null) {
			int minutes = Mathf.FloorToInt (elapsed / 60f);
			int seconds = Mathf.FloorToInt (elapsed % 60f);
			timerText.text = minutes.ToString ("00") + ":" + seconds.ToString ("00");
		}
	}

	public void TogglePause () {
		bool paused = Time.timeScale == 0f;
		Time.timeScale = paused ? 1f : 0f;
		if (pauseMenuCanvas != null) {
			pauseMenuCanvas.SetActive (!paused);
		}
	}

	public void QuitToMenu () {
		Time.timeScale = 1f;
		SceneManager.LoadScene (mainMenuSceneName);
	}

	void OnTriggerEnter2D (Collider2D target) {
		if (target.CompareTag (MyTags.FINISH_TAG)) {
			SceneManager.LoadScene (endSceneName);
			return;
		}
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
		#if UNITY_EDITOR
		UnityEditor.EditorApplication.ExitPlaymode ();
		#else
		Application.Quit ();
		#endif
	}

} // class
