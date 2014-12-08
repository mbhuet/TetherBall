using UnityEngine;
using System.Collections;
using Vectrosity;

public class SingleBall : MonoBehaviour {
	public LineCircle circle;
	public LineRenderer tether;
	public Ball ball;
	public Material dottedLineMaterial;

	bool readyToRestart = false;

	Vector3 anchor;
	// Use this for initialization
	void Start () {
		anchor = Vector3.zero;
		Vector2[] testPoints = {Vector2.zero, Vector2.one};
		//tether = VectorLine.SetLine (Color.white, Vector3.zero, Vector3.zero);
		//tether = new VectorLine("Tether", testPoints, dottedLineMaterial, 2,LineType.Continuous);

		/*
		tether = new VectorLine("Rectangle", new Vector2[8], dottedLineMaterial, 16.0f);
		tether.capLength = 8.0f;
		tether.MakeRect (new Rect(100, 300, 176, 112));
		tether.textureScale = 1.0f;
		tether.Draw();
		*/
	}

	// Update is called once per frame
	void Update () {
		CheckInput ();

		tether.SetPosition (0, ball.transform.position);
		tether.SetPosition (1, anchor);
	}

	void CheckInput(){
		if (Input.GetMouseButtonDown (0)) {
			if (ball.dead){
				ball.Respawn();
				readyToRestart = true;
			}
			else{

			Vector3 p = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			p.z = 0;
			anchor = p;
			ball.SetAnchor(p);

			//circle = VectorLine.setl
			float r = Vector3.Distance (ball.transform.position, p);
			float circumference = 2 * Mathf.PI * r;

			
			circle.renderer.enabled = true;
			circle.renderer.material.mainTextureScale = new Vector2(circumference/2, 1);
			circle.transform.position = p + Vector3.forward;
			circle.SetRadius(Vector3.Distance(p, ball.transform.position));
			circle.SetThickness(.2f);
			circle.SetPercentage(1);
			
				tether.enabled = true;

			}
		}
		if (Input.GetMouseButtonUp(0)) {
			if (ball.dead && readyToRestart){
				ball.dead = false;
				readyToRestart = false;
			}
			tether.enabled = false;
			ball.Release();
			circle.renderer.enabled = false;
		}
		
	}
}
