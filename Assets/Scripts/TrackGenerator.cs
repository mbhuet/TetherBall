using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TrackGenerator : MonoBehaviour {
	public static TrackGenerator Instance;

	float bufferDist;
	public float sectionDist = 10;
	public float trackWidth = 15;
	float maxSpread;
	public float spread = 0;

	public float barrierWidth = 4;
	public GameObject barrier;
	public GameObject obstacle;
	public GameObject zone;
	GameObject oldDummy;

	private int angleHold;
	private float heldAngle;

	static List<Zone> sections;
	List<float> angleVals;

	float obsSpawnCounter = 0;

	int numSections = 0;



	void Awake(){
		Instance = this;
	}
	// Use this for initialization
	void Start () {
		bufferDist = Screen.width / 10;
		oldDummy = new GameObject();
		oldDummy.transform.position = this.transform.position;

		sections = new List<Zone> ();
		angleVals = new List<float> ();

		maxSpread = 90.0f - Mathf.Rad2Deg*Mathf.Acos ((sectionDist / 2.0f) / trackWidth); //This is just a trig operation
		spread = maxSpread;
	}
	
	// Update is called once per frame
	void Update () {
		if (spread < maxSpread) {
						spread += Time.deltaTime / 10;
				}
		Vector3 leftScreenPos = (Camera.main.WorldToScreenPoint (this.transform.position - this.transform.right * trackWidth));
		Vector3 rightScreenPos = (Camera.main.WorldToScreenPoint (this.transform.position + this.transform.right * trackWidth));
		Vector3 midScreenPos = (Camera.main.WorldToScreenPoint (this.transform.position));

		int STOP = 0;
//		Debug.Log (screenPos);
		//the builder is in view and needs to generate
		while ((leftScreenPos.x > 0 - bufferDist && leftScreenPos.x < Screen.width + bufferDist && leftScreenPos.y > -bufferDist && leftScreenPos.y < Screen.height + bufferDist) ||
		       (rightScreenPos.x > 0 - bufferDist && rightScreenPos.x < Screen.width + bufferDist && rightScreenPos.y > -bufferDist && rightScreenPos.y < Screen.height + bufferDist) ||
		       (midScreenPos.x > 0 - bufferDist && midScreenPos.x < Screen.width + bufferDist && midScreenPos.y > -bufferDist && midScreenPos.y < Screen.height + bufferDist) && STOP <= 10) {

			MakeSection();
			leftScreenPos = (Camera.main.WorldToScreenPoint (this.transform.position - this.transform.right * trackWidth));
			rightScreenPos = (Camera.main.WorldToScreenPoint (this.transform.position + this.transform.right * trackWidth));
			midScreenPos = (Camera.main.WorldToScreenPoint (this.transform.position));
			STOP++;
		}
	}
	void MakeSection(){
		Vector3 newPos = GetNext ();
		Vector3 oldPos = oldDummy.transform.position;

		Vector3 oldLeft = oldPos - oldDummy.transform.right * trackWidth;
		Vector3 oldRight = oldPos + oldDummy.transform.right * trackWidth;

		Vector3 newLeft = newPos - this.transform.right * trackWidth;
		Vector3 newRight = newPos + this.transform.right * trackWidth;

		Vector3 midLeft = (oldLeft + newLeft) / 2.0f;
		Vector3 midRight = (oldRight + newRight) / 2.0f;

		Vector3 midPos = (newPos + oldPos)/2.0f;
//		Debug.Log (midPos);

		GameObject left = (GameObject)GameObject.Instantiate (barrier, midLeft, this.transform.rotation);
		GameObject right = (GameObject)GameObject.Instantiate (barrier, midRight, this.transform.rotation);

		left.transform.localScale = new Vector3 (barrierWidth, Vector3.Distance(oldLeft, newLeft), 1);
		right.transform.localScale = new Vector3 (barrierWidth, Vector3.Distance(oldRight, newRight),1);

		GameObject freshZone = (GameObject)GameObject.Instantiate (zone, midPos, this.transform.rotation);
		freshZone.gameObject.transform.localScale = new Vector3 (trackWidth*2, sectionDist, 1);
		freshZone.name = "Zone " + numSections;

		left.transform.parent = freshZone.transform;
		right.transform.parent = freshZone.transform;

		sections.Add (freshZone.GetComponent<Zone>());
		angleVals.Add (heldAngle);

		if (obsSpawnCounter >= 50) {
						SpawnObstacle (oldLeft, oldRight, newLeft, newRight);
						obsSpawnCounter = 0;
				} else {
			obsSpawnCounter += Random.Range(1.0f,5.0f);		
		}
		/*
		int obsSpawn = (Random.Range (0, 20));
		if (obsSpawn >=19) {
			SpawnObstacle(oldLeft, oldRight, newLeft, newRight);

			for (int i = 0; i <obsSpawn; i++){
			SpawnObstacle(oldLeft, oldRight, newLeft, newRight);
			}
		}
					*/


		numSections++;
	}

	//RULES
	//total angle for all existing track cannot exceed 180 or it might doubling back on itself
	//the max angle change for new section is ACOS(sectionLength/2/trackWidth). This will produce the tightest possible curve.
	Vector3 GetNext(){
		oldDummy.transform.position = this.transform.position;
		oldDummy.transform.rotation = this.transform.rotation;
		angleHold--;
		if (angleHold <= 0) {

			//restricts the new angle so that the total on the screen will not exceed 180
			float minBound = 0;
			float maxBound = 0;

			if (totalTrackAngle() >=0){
				minBound = -spread;
				maxBound = Mathf.Min(spread, 180 - totalTrackAngle());
			}
			else{
				minBound = Mathf.Max (-spread, -180 - totalTrackAngle());
				maxBound = spread;
			}


			//maxBound = Mathf.Min (spread, 180 - totalPosTrackAngle());
			//minBound = Mathf.Max (-spread, -180 - totalNegTrackAngle());


			float midRange = (minBound + maxBound) /2;
			heldAngle = Random.Range (minBound, maxBound);

			// Marc's Method
			/*
			if (heldAngle > midRange){
				heldAngle += Random.Range(0, Mathf.Abs(maxBound-heldAngle));
			}
			else {
				heldAngle -= Random.Range(0, Mathf.Abs(minBound-heldAngle));

			}
			*/

			// Always max or min bound

			if (heldAngle > midRange){
				heldAngle = maxBound;
			}
			else {
				heldAngle = minBound;

			}


			//the most sections we can create at this angle without exceeding 180 degrees
			if (heldAngle == 0) heldAngle = .01f; //to prevent divide by 0 errors.
			int maxHold = 0;
			/*
			if (heldAngle <0){
				maxHold = (int)((180 - Mathf.Abs(totalNegTrackAngle()))/Mathf.Abs(heldAngle));
			}
			else{
				maxHold = (int)((180 - Mathf.Abs(totalPosTrackAngle()))/Mathf.Abs(heldAngle));
			}
			*/

			maxHold = (int)((180 - Mathf.Abs(totalTrackAngle()))/Mathf.Abs(heldAngle));
			maxHold = Mathf.Clamp(maxHold, 0, (int)(Camera.main.orthographicSize/sectionDist));

			//angleHold = Random.Range(1, maxHold+1);
			angleHold = maxHold;

			Debug.Log("section:: " + numSections + ", minBound::: " + minBound + ", maxBound:: " + maxBound + ", heldAngle:: " + heldAngle + ", angleHold:: " + angleHold + ", totalAngle:: " + totalTrackAngle());

		}
		this.transform.Rotate (0, 0, heldAngle);
		this.transform.Translate (0, -sectionDist, 0, Space.Self);
		return this.transform.position;
	}

	void SpawnObstacle(Vector3 oL, Vector3 oR, Vector3 nL, Vector3 nR){
		Vector3 midpoint = (oL + oR + nL + nR) / 4.0f;
		Vector3 obsPoint = midpoint + Random.Range (0f, 1.0f) * (oL - midpoint)
						+ Random.Range (0f, 1.0f) * (nL - midpoint)
						+ Random.Range (0f, 1.0f) * (oR - midpoint)
						+ Random.Range (0f, 1.0f) * (nR - midpoint);
		GameObject obs = (GameObject) GameObject.Instantiate (obstacle, obsPoint, this.transform.rotation);
	}

	public void RemoveSection(Zone section){
		angleVals.RemoveAt (sections.IndexOf (section));
		sections.Remove (section);
	}

	float totalPosTrackAngle(){
		float total = 0;
		foreach (float a in angleVals){
			if (a >=0)
			total += a;
		}
	
		return total;
	}

	float totalNegTrackAngle(){
		float total = 0;
		foreach (float a in angleVals){
			if (a <=0)
			total -= a;
		}
		
		return total;
	}

	float totalTrackAngle(){
		float total = 0;
		foreach (float a in angleVals){
				total += a;
		}
		
		return total;
	}
}
