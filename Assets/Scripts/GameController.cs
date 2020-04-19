using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameController: MonoBehaviour {
	public Camera cam;
	public Text text;

	public GameObject gameoverScreen;

	public GameObject[] rockPrefabs;

	public GameObject[] powerupPrefabs;
	
	public Transform pilot;

	public float difficulty = 5f;
	public int score = 0;
	public int rocks = 0;

	public float width = 0f;
	public float height = 0f;

	private float spawn = 0;

	private float powerupSpawn = 0;

	public bool playing = true;

	void Start() {
		float ratio = (float)Screen.width / Screen.height;
        height = cam.orthographicSize * 2 + 4f;
		width = cam.orthographicSize * 2 * ratio + 4f;

	}
	
	void Update() {
		if (spawn > Time.deltaTime)
			spawn -= Time.deltaTime;
		else
			spawn = 0f;

		if (powerupSpawn > Time.deltaTime)
			powerupSpawn -= Time.deltaTime;
		else
			powerupSpawn = 0f;

		if (rocks < difficulty && spawn == 0f) {
			MoreRock();
			spawn = Random.Range(.1f, .5f);

		}

		if (powerupSpawn == 0f) {
			MorePowerUp();
			powerupSpawn = Random.Range(10f, 30f) + difficulty / 2f;

		}

		text.text = "Score: " + score;

	}

	public void EndGame() {
		playing = false;
		gameoverScreen.SetActive(true);

	}

	public void RestartGame() {
		AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("Game");

	}

	private void MoreRock() {
		Vector2 pos = Vector2.zero;
		do {
			pos.x = Random.Range(-width / 2, width / 2);
			pos.y = Random.Range(-height / 2, height / 2);

		} while (((Vector2)pilot.position - pos).sqrMagnitude < 36f);
		float rot = Random.Range(0f, 360f);

		GameObject go = Instantiate(rockPrefabs[Random.Range(0, rockPrefabs.Length)], new Vector3(pos.x, pos.y, 0f), Quaternion.Euler(0f, 0f, rot));
		Rigidbody2D rb = go.GetComponent<Rigidbody2D>();
		Collider2D[] results = new Collider2D[1];
		if (rb.OverlapCollider(new ContactFilter2D(), results) > 0) {
			Destroy(go);
			return;

		}

		Rock rock = go.GetComponent<Rock>();
		rock.gc = this;
		rock.first = true;
		rock.split = Random.Range(1, 3);
		rock.health = (40 + difficulty) * (rock.split + 1);

		go.transform.localScale = new Vector3(1 + rock.split, 1 + rock.split, 1f);

		rb.mass = 10 * (rock.split + 1);
		rb.AddForce(new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized * Random.Range(0f, difficulty / 10f), ForceMode2D.Impulse);
		rb.AddTorque(Random.Range(-5f, 5f), ForceMode2D.Impulse);

		rocks ++;

	}

	private void MorePowerUp() {
		Vector2 pos = Vector2.zero;
		do {
			pos.x = Random.Range(-width / 3, width / 3);
			pos.y = Random.Range(-height / 3, height / 3);

		} while (((Vector2)pilot.transform.position - pos).sqrMagnitude < 36f);
		float rot = Random.Range(0f, 360f);

		GameObject go = Instantiate(powerupPrefabs[Random.Range(0, powerupPrefabs.Length)], new Vector3(pos.x, pos.y, 0f), Quaternion.Euler(0f, 0f, rot));
		Rigidbody2D rb = go.GetComponent<Rigidbody2D>();

		Powerup powerup = go.GetComponent<Powerup>();

		powerup.gc = this;
		powerup.pilot = pilot.GetComponent<Pilot>();
		powerup.health = 10f + difficulty;
		powerup.bounce = (int)(Random.Range(0, 50) / 50);
		powerup.damage = Random.Range(0f, .5f) * difficulty;
		powerup.cooldown = -Random.Range(0f, .0025f) * difficulty;

		rb.velocity = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized * Random.Range(1, 2f);
		rb.angularVelocity = Random.Range(-10, 10f);
	}

}
