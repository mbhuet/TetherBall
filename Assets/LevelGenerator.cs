using UnityEngine;
using System.Collections;

public class LevelGenerator : MonoBehaviour {
	public GameObject obstacle;
	public GameObject target;

	public float step = 1.0f;
	public float sampleScale = 1.0f;
	float randomStart;

	GameObject player;

	// Use this for initialization
	void Start () {
		player = GameObject.FindGameObjectWithTag ("Player");
		Create ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void Create(){
		randomStart = Random.Range (0, 10000);
		float height = Camera.main.orthographicSize * 2;
		float width = (Camera.main.aspect * Camera.main.orthographicSize) * 2;
		//height
		for (float i = 0; i< height; i+=step) {
			//width
			for (float j = 0; j< width; j+= step){
				float x = i*sampleScale + randomStart;
				float y = j*sampleScale + randomStart;
				float p = Mathf.PerlinNoise(x, y);
				Debug.Log(x + ", " + y + ", " + p);

				if (p > .7f){
					GameObject obj = GameObject.Instantiate(obstacle) as GameObject;
					obj.transform.position = new Vector3(j - width/2f,i - height/2f, 0);
				}
				if (p < .1f){
					GameObject obj = GameObject.Instantiate(target) as GameObject;
					obj.transform.position = new Vector3(j - width/2f,i - height/2f, 0);
				}
			}
		}

		//Cleanup
		Collider2D[] colls = Physics2D.OverlapCircleAll (player.transform.position, 10);
		foreach (Collider2D coll in colls) {
			if (coll.tag != "Player" && coll.tag != "Wall"){
				GameObject.Destroy(coll.gameObject);	
			}
		}
	}
}
