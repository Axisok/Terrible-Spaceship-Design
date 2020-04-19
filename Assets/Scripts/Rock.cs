using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rock: MonoBehaviour {
	public GameController gc;

	public GameObject explosionPrefab;
	public GameObject collisionPrefab;

	public float health = 100f;
	public int split = 0;
	public bool first = false;

	void Update() {
		// Destroy rock when at 0 health and increase difficulty.
		if (health <= 0f) {
			gc.score += 100;

			if (first)
				gc.rocks --;

			gc.difficulty += .45f;

			Explode();

		}

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

	}

	public void Hit(float damage) {
		health -= damage;

	}

	public void Explode() {
		Instantiate(explosionPrefab, new Vector3(transform.position.x, transform.position.y, 1f), Quaternion.identity);
		if (split > 0) {
			int qnt = Random.Range(2, 4);
			Rigidbody2D thisrb = GetComponent<Rigidbody2D>();
			float offset = Random.Range(0f, 360f);
			for (int i = 0; i < qnt; i ++) {
				Vector2 pos = Quaternion.Euler(0f, 0f, offset + 360f / qnt * i) * new Vector2(0f, transform.localScale.y / 2);
				GameObject go = Instantiate(gc.rockPrefabs[Random.Range(0, gc.rockPrefabs.Length)], transform.position + (Vector3)pos, Quaternion.Euler(0f, 0f, Random.Range(0f, 360f)));

				Rock rock = go.GetComponent<Rock>();
				rock.gc = gc;
				rock.first = false;
				rock.split = split - 1;
				rock.health = (40 + gc.difficulty) * (rock.split + 1);

				go.transform.localScale = new Vector3(1 + rock.split, 1 + rock.split, 1f);

				Rigidbody2D rb = go.GetComponent<Rigidbody2D>();
				rb.mass = 10 * (rock.split + 1);
				rb.AddForce(thisrb.velocity + pos.normalized * Random.Range(5f, 15f), ForceMode2D.Impulse);
				rb.AddTorque(Random.Range(-5f, 5f), ForceMode2D.Impulse);

			}

		}

		Destroy(gameObject);
		
	}

	void OnCollisionEnter2D(Collision2D collision) {
		if (collision.relativeVelocity.sqrMagnitude < 16f)
			return;

		ContactPoint2D[] contacts = new ContactPoint2D[collision.contactCount];
		collision.GetContacts(contacts);
		foreach (ContactPoint2D contact in contacts) {
			Instantiate(collisionPrefab, contact.point, Quaternion.identity);

		}

	}

}
