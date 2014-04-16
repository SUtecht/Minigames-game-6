using UnityEngine;
using System.Collections;

public class GUIScript : MonoBehaviour {
	public bool lost = false;
	public bool won = false;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnGUI(){
		if(won){
			//GUI.Label(new Rect(60, -72, 200, 40), "VICTORY");
			if(GUI.Button(new Rect(60, -32, 200, 100), "VICTORY! Play Again?")){
				Application.LoadLevel("spider");
			}
		}
		if(lost){
			Debug.Log ("LOST");
			//GUI.Label(new Rect(60, -72, 200, 40), "Game Over");
			if(GUI.Button(new Rect(60, -32, 200, 100), "Game Over, Retry?")){
				Application.LoadLevel("spider");
			}
		}
	}
}
