using System;
using UnityEngine;
using Vuforia;

public class HitScript : MonoBehaviour, IVirtualButtonEventHandler {

    private GameObject homer;
    private bool prevPressed;

	// Use this for initialization
	void Start ()
    {
        homer = transform.GetChild(0).gameObject;
        prevPressed = false;
        VirtualButtonBehaviour[] vbs = GetComponentsInChildren<VirtualButtonBehaviour>();
        for (int i = 0; i < vbs.Length; ++i)
            vbs[i].RegisterEventHandler(this);
	}

    void SetVisible(bool val)
    {
        homer.GetComponent<Renderer>().enabled = val;
    }

	// Update is called once per frame
	void Update ()
    {
    }

    public void OnButtonPressed(VirtualButtonAbstractBehaviour vb)
    {
        SetVisible(false);
    }

    public void OnButtonReleased(VirtualButtonAbstractBehaviour vb)
    {
        SetVisible(true);
    }
}
