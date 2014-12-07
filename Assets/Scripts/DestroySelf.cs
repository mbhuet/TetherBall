using UnityEngine;
using System.Collections;

public class DestroySelf : MonoBehaviour {
	float bufferDist;
	// Use this for initialization
	void Start () {
		bufferDist = Screen.width*2;
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 screenPos = (Camera.main.WorldToScreenPoint (this.transform.position));
		//		Debug.Log (screenPos);
		//the builder is in view and needs to generate
		if (screenPos.x < 0 - bufferDist || screenPos.x > Screen.width + bufferDist || screenPos.y < -bufferDist || screenPos.y > Screen.height + bufferDist) {
			Destroy(this.gameObject);
		}
	}
}
