using UnityEngine;
using Vuforia;

public class HitScript : MonoBehaviour, IVirtualButtonEventHandler, ITrackableEventHandler {

	public AudioClip doh;
    public int homerId;

    private GameObject board;
    private BoardScript boardScript;
    private bool active, boardDetected, justDetected;
    private int prevHomerId;

	void Start ()
    {
        board = GameObject.Find("Board");
        boardScript = board.GetComponent<BoardScript>();

        prevHomerId = -1;
        active = false;
        boardDetected = false;
        board.GetComponent<TrackableBehaviour>().RegisterTrackableEventHandler(this);
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

    public void OnTrackableStateChanged(TrackableBehaviour.Status previousStatus, TrackableBehaviour.Status newStatus)
    {
        boardDetected = newStatus >= TrackableBehaviour.Status.DETECTED;
        if (previousStatus < TrackableBehaviour.Status.DETECTED && newStatus >= TrackableBehaviour.Status.DETECTED)
            justDetected = true;
    }

	void FixedUpdate ()
    {
        justDetected = false;
        if ((prevHomerId != homerId) == (boardScript.homerId == homerId))
        {
            active = boardScript.homerId == homerId;
            SetVisible(active);
        }
        prevHomerId = boardScript.homerId;
	}

    void LateUpdate ()
    {
        if (justDetected)
            SetVisible(false);
    }
}
