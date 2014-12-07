using UnityEngine;
using System.Collections;

public class SingleBall : MonoBehaviour {
	public LineCircle circle;
	public Ball ball;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		CheckInput ();
	}

	void CheckInput(){
		if (Input.GetMouseButtonDown (0)) {
			Vector3 p = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			p.z = 0;
			ball.SetAnchor(p);

			circle.renderer.enabled = true;
			circle.transform.position = p;
			circle.SetRadius(Vector3.Distance(p, ball.transform.position));
			circle.SetThickness(.1f);
			circle.SetPercentage(1);
		}
		if (Input.GetMouseButtonUp(0)) {
			ball.Release();
			circle.renderer.enabled = false;
		}
		
	}
}
