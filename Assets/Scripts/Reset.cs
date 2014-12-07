using UnityEngine;
using System.Collections;

public class Reset : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnGUI(){
		if (GUI.Button(new Rect(0, 0, 80, 80), "RESET")){
			Application.LoadLevel(Application.loadedLevel);
		}
	}
}
