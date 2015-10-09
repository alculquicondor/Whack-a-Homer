using UnityEngine;
using System.Collections;

public class Click : MonoBehaviour {

	public AsyncOperation op;

	void Start () {
        StartCoroutine(LoadBoard());
	}

    IEnumerator LoadBoard()
    {
        op = Application.LoadLevelAsync("Main");
        op.allowSceneActivation = false;
        yield return op;
    }

	void Update () {
        if (Cardboard.SDK.Triggered) {
            op.allowSceneActivation = true;
        }
	}
}
