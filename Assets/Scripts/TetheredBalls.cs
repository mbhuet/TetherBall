using UnityEngine;
using System.Collections;

public class TetheredBalls : MonoBehaviour {
	public LineCircle circle;
	public Ball[] balls;
	int ballIndex = 0;
	// Use this for initialization
	void Start () {


		for(int i = 0; i < balls.Length; i++){
			if (i==ballIndex){
				balls[i].Release();
			}
			else{
				balls[i].SetAnchor(balls[ballIndex].transform.position);
			}

		}
	}
	
	// Update is called once per frame
	void Update () {
		CheckInput ();
	}

	void CheckInput(){
		if (Input.GetMouseButtonDown (0)) {
			ballIndex++;
			if (ballIndex>=balls.Length){
				ballIndex = 0;
			}

			for(int i = 0; i < balls.Length; i++){
				if (i==ballIndex){
					balls[i].Release();
				}
				else{
					balls[i].SetAnchor(balls[ballIndex].transform.position);
				}
			}

			circle.renderer.enabled = true;
			circle.transform.position = balls[ballIndex].transform.position;
			circle.SetRadius(Vector3.Distance(balls[0].transform.position, balls[1].transform.position));
			circle.SetThickness(.1f);
			circle.SetPercentage(1);
		}

		
	}
}
