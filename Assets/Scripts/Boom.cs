using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boom: MonoBehaviour {
	public AudioSource aS;
	public AudioClip[] sounds;

	public bool particlesDone = false;

	void Start() {
		aS.clip = sounds[Random.Range(0, sounds.Length)];
		aS.Play();

	}

	void Update() {
		if (particlesDone && aS.isPlaying == false)
			Destroy(gameObject);

	}
	
	void OnParticleSystemStopped() {
		particlesDone = true;

	}

}
