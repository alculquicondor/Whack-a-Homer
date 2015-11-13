using UnityEngine;
using Vuforia;

public class HitScript : MonoBehaviour, IVirtualButtonEventHandler {

	public AudioClip doh;
    public int homerId;

    private GameObject homer;
    private BoardScript boardScript;
    private bool active, boardDetected, justDetected;
    private int prevHomerId;

	void Start ()
    {
        homer = transform.Find("homer").gameObject;
        boardScript = GameObject.Find("Board").GetComponent<BoardScript>();

        prevHomerId = -1;
        active = false;
        boardDetected = false;
        GetComponent<VirtualButtonBehaviour>().RegisterEventHandler(this);
	}

    void SetVisible(bool val)
    {
        GetComponentInChildren<Renderer>().enabled = boardDetected && active && val;
    }

    public void OnButtonPressed(VirtualButtonAbstractBehaviour vb)
    {
        if (!boardDetected || ! active)
            return;
        SetVisible(false);
		GetComponent<AudioSource>().clip = doh;
		GetComponent<AudioSource> ().PlayOneShot(GetComponent<AudioSource>().clip);
        boardScript.counter += 1;
    }

    public void OnButtonReleased(VirtualButtonAbstractBehaviour vb)
    {
    }

	void FixedUpdate ()
    {
        if ((prevHomerId != homerId) == (boardScript.homerId == homerId))
        {
            active = boardScript.homerId == homerId;
            homer.SetActive(active);
        }
        prevHomerId = boardScript.homerId;
	}

}
