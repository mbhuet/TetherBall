using UnityEngine;
using System.Collections;

public class Zone : MonoBehaviour {
	public bool passed;
	float bufferDist;
	// Use this for initialization
	void Start () {
		bufferDist = Screen.width/3.0f;
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 screenPos = (Camera.main.WorldToScreenPoint (this.transform.position));
		//		Debug.Log (screenPos);
		//the builder is in view and needs to generate
		if (passed) {
						if (screenPos.x < 0 - bufferDist || screenPos.x > Screen.width + bufferDist || screenPos.y < -bufferDist || screenPos.y > Screen.height + bufferDist) {
								TrackGenerator.Instance.RemoveSection (this);
								Destroy (this.gameObject);
						}
				}
	}

	void OnTriggerExit(Collider col){
		if (col.GetComponent<Move> ()) {
			passed = true;		
		}
	}
}
