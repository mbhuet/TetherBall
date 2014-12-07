using UnityEngine;
using System.Collections;

public class Move : MonoBehaviour {
	public float velocity;
	public float maxVelocity;
	public float minVelocity;
	float friction;
	bool stunned;
	float stunTimer;
	public float speedFactor = 1;
	// Use this for initialization
	void Start () {
		rigidbody.velocity = Vector3.down * minVelocity;
	}

	void Update(){

	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if (rigidbody.velocity.magnitude < minVelocity/2) {
			Application.LoadLevel(Application.loadedLevel);		
		}
		if (stunned == true) {
						stunTimer -= Time.deltaTime;
				}

		if (stunTimer <= 0)
						stunned = false;

		if (velocity < maxVelocity && rigidbody.velocity.magnitude > velocity - 5) {
			velocity += Time.fixedDeltaTime * speedFactor;
		}

		velocity = Mathf.Clamp (velocity, minVelocity, maxVelocity);

		Vector3 dir = Vector3.zero;
		if (Application.platform == RuntimePlatform.WindowsEditor || Application.platform == RuntimePlatform.WindowsPlayer){
			float tilt = Input.GetAxis("Horizontal")* Time.deltaTime *10;
			//			Debug.Log(tilt);
			Camera.main.transform.Rotate(0,0,tilt * 10);

			float zTest = Input.GetAxis("Vertical");
			dir = new Vector3(-Camera.main.transform.up.x, -Camera.main.transform.up.y, 0);
			//Physics.gravity = new Vector2(-this.transform.up.x, -this.transform.up.y)*gravity;

		}
		else if (Application.platform == RuntimePlatform.Android){
			//Physics.gravity = new Vector3(Input.acceleration.x, Input.acceleration.y, 0)*gravity;

			dir = new Vector3(Input.acceleration.x, Input.acceleration.y, 0);
		}

		if (dir.magnitude >1) {
			dir.Normalize();
		}
		this.rigidbody.velocity = dir * velocity;
	}

	void OnCollisionStay(Collision col){
		velocity = rigidbody.velocity.magnitude;
		if (rigidbody.velocity.magnitude < minVelocity) {
			velocity = minVelocity;
		}
	}

	void OnCollisionEnter(Collision col){
		//Handheld.Vibrate();
		particleSystem.Emit (10);
		//velocity = -velocity;
		//stunned = true;
		//stunTimer = .5f;
		Camera.main.SendMessage("DoShake");
	}

	void OnGUI(){
		GUI.Label (new Rect (0, Screen.height - 20, 80, 80), Input.acceleration.x.ToString());
	}
}
