using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceShip: MonoBehaviour {
	public GameController gc;
	
	public Rigidbody2D rb;
	public LineRenderer ln;
	public Transform pilot;

    void Start() {
        
    }

    void FixedUpdate() {
		if (!gc.playing)
			return;
			
		// Get WASD/arrow keys/controller movement and normalize to make diagonal movement just as fast as any other direction.
		Vector2 movementDirection = (Input.GetAxisRaw("Horizontal") * rb.transform.right + Input.GetAxisRaw("Vertical") * rb.transform.up).normalized;

		// Move.
		rb.AddForce(movementDirection * 500 * rb.mass * Time.deltaTime);
        
    }

	void Update() {
		// Change the line points so it connects spaceship to pilot.
		ln.SetPosition(0, pilot.position);
		ln.SetPosition(1, rb.transform.position);

		// Teleports the spaceship to the other side of the screen.
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
