using System;
using UnityEngine;
using Vuforia;

public class HomerScript : MonoBehaviour, ITrackableEventHandler {

    public int homerId;

    private GameObject board;
    private BoardScript boardScript;
    private bool active, boardDetected, justDetected;
    private int prevHomerId;

	void Start ()
    {
        board = GameObject.Find("Board");
        board.GetComponent<TrackableBehaviour>().RegisterTrackableEventHandler(this);
        boardScript = FindObjectOfType(typeof(BoardScript)) as BoardScript;
        prevHomerId = -1;
        active = false;
        boardDetected = false;
	}

    void SetVisible(bool val)
    {
        GetComponent<Renderer>().enabled = boardDetected && active && val;
    }

    public void Hit()
    {
        if (!boardDetected || ! active)
            return;
        SetVisible(false);
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

    public void OnTrackableStateChanged(TrackableBehaviour.Status previousStatus, TrackableBehaviour.Status newStatus)
    {
        boardDetected = newStatus >= TrackableBehaviour.Status.DETECTED;
        if (previousStatus < TrackableBehaviour.Status.DETECTED && newStatus >= TrackableBehaviour.Status.DETECTED)
        {
            justDetected = true;
        }
    }

    void LateUpdate ()
    {
        if (justDetected)
            SetVisible(false);
    }
}
