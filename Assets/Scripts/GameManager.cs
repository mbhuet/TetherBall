using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {
	public float gravity = 10;
	public float tilt;
	public bool debug;
	public float camSize;
	//public Move target;
	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		//this.transform.position = new Vector3(target.transform.position.x, target.transform.position.y, -20);
		//camSize = Mathf.Lerp (camSize, Mathf.Clamp (20.0f * target.velocity / target.minVelocity, 20, 50), .5f);
		//Camera.main.orthographicSize = camSize;


	}
}
