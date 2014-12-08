using UnityEngine;
using System.Collections;

public class Ball : MonoBehaviour {
	public AudioClip wallCollisionSound;
	public AudioClip targetCollisionSound;
	public AudioClip setAnchorSound;
	public AudioClip respawnSound;
	public AudioClip releaseSound;

	public Vector3 moveDirection;
	public float speed = 1;
	public bool moveWhileFree = true;
	public bool reverseDirection = false;
	public bool trailCollision = true;
	bool tethered;
	Vector3 anchor;
	public bool dead = true;
	public ParticleSystem crashParticles;

	GameObject trailColliderRoot;

	TrailRenderer trail;

	Vector3 resetPoint = Vector3.zero;

	//-1 for counterCW, 1 for CW
	int rotationDirection = 0;

	// Use this for initialization
	void Start () {
		resetPoint = Vector3.down * 27;

		CreateTrail ();
		moveDirection = Vector3.up;
	
	}
	
	// Update is called once per frame
	void Update () {

		if (!dead) {
						Move ();
				}
	}

	void Move(){
		StartCoroutine("DelayedPath", this.transform.position);
		if (!tethered){
			if(moveWhileFree) {
				this.transform.Translate (moveDirection.normalized * speed * Time.deltaTime, Space.Self);
			}
		} 

		else {

			float r = Vector3.Distance (this.transform.position, anchor);
			float circumference = 2 * Mathf.PI * r;
			
			float distPerDeg = circumference / 360f;
			float angle = (speed * Time.deltaTime) / distPerDeg;

			this.transform.RotateAround(anchor, Vector3.forward, angle * rotationDirection);
		
		}
		/*
		//forces the ball's up direction to match its move direction
		if (true) {
			float angle = Mathf.Atan2(moveDirection.y, moveDirection.x) * Mathf.Rad2Deg;
			transform.rotation = Quaternion.AngleAxis(angle -90, Vector3.forward);
		} 
		*/
	}

	public void SetAnchor(Vector3 a){
		audio.PlayOneShot (setAnchorSound);
		tethered = true;
		anchor = a;




		Vector3 tangent = Vector3.Cross ((a - this.transform.position), (this.transform.position + Vector3.back));

		float turnAngle = Mathf.Atan2(tangent.y, tangent.x) * Mathf.Rad2Deg;

		if (isLeft (this.transform.position, this.transform.position - transform.up, a)) {
						rotationDirection = -1;		
				} else {
			rotationDirection = 1;		

			turnAngle +=180;
		}

		if (reverseDirection) {
						rotationDirection = -rotationDirection;
				}

		transform.rotation = Quaternion.AngleAxis(turnAngle -90, Vector3.forward);

	}

	public void Release(){
		audio.PlayOneShot (releaseSound);

		tethered = false;
		anchor = Vector3.zero;
	}


	bool isLeft(Vector3 a, Vector3 b, Vector3 c){
		return ((b.x - a.x)*(c.y - a.y) - (b.y - a.y)*(c.x - a.x)) > 0;
	}

	void OnCollisionEnter2D(Collision2D coll) {
		Debug.Log ("hit");
		Explode ();
	}

	void OnTriggerEnter2D(Collider2D other) {
		Debug.Log ("trigger hit");
		if (other.tag == "Pickup") {

			other.GetComponent<Target>().Explode();	
		}
	}

	void CreateTrail(){
		GameObject trailObj = new GameObject ();
		trail = trailObj.AddComponent<TrailRenderer> ();
		trailObj.transform.position = this.transform.position;
		trailObj.transform.parent = this.transform;
		trail.time = 60;
		trail.endWidth = this.transform.localScale.x;
		trail.startWidth = this.transform.localScale.x;

		if (trailColliderRoot != null)
						GameObject.Destroy (trailColliderRoot.gameObject);
		trailColliderRoot = new GameObject ();
		trailColliderRoot.name = "Trail Colliders";

		
	}

	void Explode(){
		audio.PlayOneShot (wallCollisionSound);
		ParticleSystem partObj = GameObject.Instantiate (crashParticles, this.transform.position, Quaternion.identity) as ParticleSystem;
		//Debug.Log (partObj);
		partObj.Emit (20);
		//Debug.Log (partObj.GetComponent<ParticleSystem>());
		//partObj.particleSystem.Emit (20);
		//particleSystem.Emit (20);
		//moveWhileFree = false;
		Release ();
		collider2D.enabled = false;
		renderer.enabled = false;
		dead = true;


	}

	public void Respawn(){
		audio.PlayOneShot (respawnSound);
		//destroy the old trail, start new one
		GameObject.Destroy (trail.gameObject);

		CreateTrail ();

		transform.rotation = Quaternion.identity;
		transform.position = resetPoint;
		particleSystem.Emit (20);
		collider2D.enabled = true;
		renderer.enabled = true;

	}

	IEnumerator DelayedPath(Vector3 position){
		Debug.Log ("path");
		yield return new WaitForSeconds (.2f);
		makePath (position);
	}

	void makePath(Vector3 position)
	{
		GameObject newPath = new GameObject ();//GameObject.Instantiate (PathZone) as GameObject;
		newPath.name = "PathZone";
		CircleCollider2D col = newPath.AddComponent<CircleCollider2D> ();
		//col.isTrigger = true;
		newPath.transform.localScale = this.transform.localScale;
		newPath.tag = "Trail";
		newPath.transform.position = position;

		newPath.transform.parent = trailColliderRoot.transform;
	}

}
