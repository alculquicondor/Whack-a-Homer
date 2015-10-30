using UnityEngine;
using System.Collections;

public class Click : MonoBehaviour {

	public AsyncOperation op;
    public GameObject hommer;
    private float time;

	void Start () {
        StartCoroutine(LoadBoard());
        time = 0;
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
        time += Time.deltaTime;
        hommer.transform.Translate(new Vector3(-Time.deltaTime * 15, 0.15f * Mathf.Sin(10 * time), 0));
        if (hommer.transform.position.x < -40)
            hommer.transform.Translate(new Vector3(80, 0, 0));
	}
}
