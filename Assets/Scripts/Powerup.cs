using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerup: MonoBehaviour {
	public GameController gc;

	public GameObject pickupPrefab;

	public Rigidbody2D rb;

	public Pilot pilot;

	public float health = 20f;
	public int bounce = 0;
	public float damage = 0f;
	public float cooldown = 0f;

	void Update() {
		// Teleport rock to the other side of the screen.
		float x = transform.position.x;
		float y = transform.position.y;
		if (x > gc.width / 2)
			x -= gc.width;
		if (x < -gc.width / 2)
			x += gc.width;
		if (y > gc.height / 2)
			y -= gc.height;
		if (y < -gc.height / 2)
			y += gc.height;

		transform.position = new Vector3(x, y, transform.position.z);

		if (((Vector2)pilot.transform.position - (Vector2)transform.position).sqrMagnitude < pilot.coll.radius*pilot.coll.radius * 8) {
			Pickup();

		}

	}

	public void Pickup() {
		Instantiate(pickupPrefab, new Vector3(transform.position.x, transform.position.y, 1f), Quaternion.identity);
		pilot.health += health;
		pilot.bounce += bounce;
		pilot.bulletDamage += damage;
		pilot.shootingCooldown = Mathf.Clamp(pilot.shootingCooldown + cooldown, 0.25f, 1f);

		Destroy(gameObject);

	}

}
