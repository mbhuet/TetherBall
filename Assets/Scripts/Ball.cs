using UnityEngine;
using System.Collections;

public class Ball : MonoBehaviour {
	public Vector3 moveDirection;
	public float speed = 1;
	public bool moveWhileFree = true;
	public bool reverseDirection = false;
	bool tethered;
	Vector3 anchor;

	//-1 for counterCW, 1 for CW
	int rotationDirection = 0;

	// Use this for initialization
	void Start () {
		moveDirection = Vector3.up;
	
	}
	
	// Update is called once per frame
	void Update () {
		//CheckInput ();
		Move ();
	}

	void Move(){
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
		tethered = false;
		anchor = Vector3.zero;
	}


	bool isLeft(Vector3 a, Vector3 b, Vector3 c){
		return ((b.x - a.x)*(c.y - a.y) - (b.y - a.y)*(c.x - a.x)) > 0;
	}

}
