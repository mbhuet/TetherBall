using UnityEngine;
using System.Collections;

public class Target : MonoBehaviour {
	public AudioClip explodeSound;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void Explode(){
		audio.PlayOneShot (explodeSound);
		collider2D.enabled = false;
		renderer.enabled = false;

		particleSystem.Emit (20);
		//StartCoroutine (TimedDeath (2));
	}

	public void Reset(){
		collider2D.enabled = true;
		renderer.enabled = true;
	}


	IEnumerator TimedDeath(float t){
		yield return new WaitForSeconds(t);
		GameObject.Destroy (this.gameObject);
	}
}
