using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet: MonoBehaviour {
	public GameController gc;
	public BoxCollider2D coll;

	public GameObject hitPrefab;

	public float speed = 10f;
	public float damage = 0f;
	public int bounce = 0;

	public float lifeTime = 5f;

    void Start() {
        
    }

    void FixedUpdate() {
		lifeTime -= Time.deltaTime;
		if (lifeTime <= 0)
			Destroy(gameObject);

		transform.position += transform.up * speed * Time.deltaTime;

		bool destroy = false;
		Collider2D[] hits = Physics2D.OverlapBoxAll(transform.position, coll.size, transform.rotation.eulerAngles.z);
		foreach (Collider2D hit in hits) {
			Rock rock = hit.gameObject.GetComponent<Rock>();
			if (rock) {
				Rigidbody2D rrb = rock.GetComponent<Rigidbody2D>();
				if (rrb)
					rrb.AddForceAtPosition(transform.up * speed * 50, transform.position);
				
				rock.Hit(damage);
				Instantiate(hitPrefab, transform.position, transform.rotation);
				if (bounce > 0) {
					Vector3 delta = transform.position - hit.transform.position;
					transform.LookAt(transform.position - transform.forward, Vector2.Reflect(transform.up, delta));
					bounce --;

				} else
					destroy = true;

			}

		}
        if (destroy)
			Destroy(gameObject);

    }

	void Update() {
		// Teleports the bullet to the other side of the screen.
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

}
