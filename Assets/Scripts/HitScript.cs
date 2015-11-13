﻿using UnityEngine;
using Vuforia;

public class HitScript : MonoBehaviour, IVirtualButtonEventHandler {

	public AudioClip doh;
    public int homerId;
    public Texture []textures;

    private System.Random random;
    private HomerColor color;
    private GameObject homer;
    private BoardScript boardScript;
    private bool active, justDetected;
    private int prevHomerId;

	void Start ()
    {
        homer = transform.Find("homer").gameObject;
        boardScript = GameObject.Find("Board").GetComponent<BoardScript>();

        random = new System.Random();
        prevHomerId = -1;
        active = false;
        GetComponent<VirtualButtonBehaviour>().RegisterEventHandler(this);
	}

    void SetVisible(bool val)
    {
        homer.SetActive(val);
    }

    public void OnButtonPressed(VirtualButtonAbstractBehaviour vb)
    {
        if (!active)
            return;

        SetVisible(false);
		GetComponent<AudioSource>().clip = doh;
		GetComponent<AudioSource>().PlayOneShot(GetComponent<AudioSource>().clip);

        if (boardScript.currentColor == color)
            boardScript.counter += 1;
        else if (boardScript.counter > 0)
            boardScript.counter -= 1;

        boardScript.ChangeColor();
    }

    public void OnButtonReleased(VirtualButtonAbstractBehaviour vb)
    {
    }

    public void ChangeColor()
    {
        color = (HomerColor)random.Next(7);
        homer.GetComponent<Renderer>().material.mainTexture = textures[(int)color];
    }

	void FixedUpdate ()
    {
        if ((prevHomerId != homerId) == (boardScript.homerId == homerId))
        {
            active = boardScript.homerId == homerId;
            if (active)
                ChangeColor();
            SetVisible(active);
        }
        prevHomerId = boardScript.homerId;
	}

}
