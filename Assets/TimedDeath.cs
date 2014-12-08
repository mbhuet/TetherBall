using UnityEngine;
using System.Collections;

public class TimedDeath : MonoBehaviour {
	float t = 0;
	public float lifeTime;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		t += Time.deltaTime;
		if (t >= lifeTime) {
			GameObject.Destroy(this.gameObject);		
		}
	}
}
