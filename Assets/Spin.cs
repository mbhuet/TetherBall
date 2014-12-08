using UnityEngine;
using System.Collections;

public class Spin : MonoBehaviour {
	public float rotationSpeed;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		this.transform.Rotate (Vector3.forward, rotationSpeed * Time.deltaTime);
	}
}
