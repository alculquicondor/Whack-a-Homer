using UnityEngine;
using Vuforia;

public class HitScript : MonoBehaviour, IVirtualButtonEventHandler {

    private HomerScript homerScript;
	public AudioClip doh;

	void Start ()
    {
        homerScript = GetComponentInChildren<HomerScript>();
        VirtualButtonBehaviour[] vbs = GetComponentsInChildren<VirtualButtonBehaviour>();
        for (int i = 0; i < vbs.Length; ++i)
            vbs[i].RegisterEventHandler(this);
	}

	void Update ()
    {
    }

    public void OnButtonPressed(VirtualButtonAbstractBehaviour vb)
    {
        homerScript.Hit();
    }

    public void OnButtonReleased(VirtualButtonAbstractBehaviour vb)
    {
    }
}
