using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Pilot: MonoBehaviour {
	public GameController gc;

	public Slider healthSlider;
	public Rigidbody2D rb;
	public CircleCollider2D coll;
	public AudioSource aS;
	public GameObject bulletPrefab;

	public AudioClip[] shootingSounds;
	public AudioClip[] hurtSounds;
	public AudioClip[] deathSounds;

	public float health = 100f;
	public float drainSpeed = 1f;

	// Time between shots.
	public float shootingCooldown = 1f;
	// Impulse given when shooting.
	public float shootingImpulse = 100f;
	public int bounce = 0;
	public float bulletDamage = 35f;
	public float bulletSpeed = 10f;

	// Time until the player can shoot again.
	private float nextShot = 0f;
	// Time until the player can get hit again.
	private float immunity = 0f;

    void Start() {
        
    }

	void Update() {
		// A few values that are reduced over time, this is mainly to count.
		if (nextShot > Time.deltaTime)
			nextShot -= Time.deltaTime;
		else
			nextShot = 0f;

		if (immunity > Time.deltaTime)
			immunity -= Time.deltaTime;
		else
			immunity = 0f;

		if (health > 0f)
			health -= Time.deltaTime;

		// Make sure health is between 0 and 100.
		health = Mathf.Clamp(health, 0f, 100f);

		// Show current health on the UI.
		healthSlider.value = health;

		if (gc.playing) {
			// Death.
			if (health == 0f) {
				gc.EndGame();
				PlaySound(deathSounds[Random.Range(0, deathSounds.Length)]);
				
			}

			// Aiming and shooting.
			Shoot();

		}

	}

	private void CheckHits() {

	}

	private void Shoot() {
		// Look at the mouse position.
		Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        transform.rotation = Quaternion.LookRotation(Vector3.forward, mousePos - transform.position);

		// Check if it's time to shoot, and if so, do it.
		if (Input.GetButton("Fire1") && nextShot == 0f) {
			// Play a random shooting sound.
			PlaySound(shootingSounds[Random.Range(0, shootingSounds.Length)]);
			
			GameObject go = Instantiate(bulletPrefab, new Vector3(transform.position.x, transform.position.y, 1f), transform.rotation);
			Bullet bullet = go.GetComponent<Bullet>();
			bullet.speed = bulletSpeed;
			bullet.damage = bulletDamage;
			bullet.gc = gc;
			bullet.bounce = bounce;

			rb.AddForce(-transform.up * shootingImpulse);
			nextShot = shootingCooldown;

		}
		
	}

	private void PlaySound(AudioClip clip) {
		// This is required every time, so I put it into a small function and it now only takes a single line.
		aS.clip = clip;
		aS.Play();

	}

	void OnCollisionEnter2D(Collision2D collision) {
		// React to collisions.
		Rock rock = collision.gameObject.GetComponent<Rock>();
		if (rock && immunity == 0f) {
			immunity = .5f;
			health -= 15f * gc.difficulty / 20f;
			PlaySound(hurtSounds[Random.Range(0, hurtSounds.Length)]);
			
		}
		
	}

}
