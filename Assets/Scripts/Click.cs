using UnityEngine;
using System.Collections;

public class Click : MonoBehaviour {

	public AsyncOperation op;
	// Use this for initialization
	void Start () {
		//op = Application.LoadLevelAsync ( "Main" );
		//op.allowSceneActivation = false;
	}
	
	// Update is called once per frame
	void Update () {
		if (Cardboard.SDK.Triggered) {
			Application.LoadLevel("Main");
			//op.allowSceneActivation = true;
		}
	}
}
