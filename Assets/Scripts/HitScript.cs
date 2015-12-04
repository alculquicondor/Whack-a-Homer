using UnityEngine;
using Vuforia;

public class HitScript : MonoBehaviour, IVirtualButtonEventHandler {

	public AudioClip doh;
	public AudioClip woohoo;
    public int homerId;
    public Texture []textures;
    public Material regular, gold;
    public int goldStart;
    public float goldProbability;

    private System.Random random;
    private HomerColor color;
    private GameObject homer;
    private BoardScript boardScript;
    private bool active, justDetected;
    private int prevHomerId;
    private float homerTime;

	void Start ()
    {
        homer = transform.Find("homer").gameObject;
        boardScript = GameObject.Find("Board").GetComponent<BoardScript>();

        random = new System.Random();
        prevHomerId = -1;
        active = false;
        GetComponent<VirtualButtonBehaviour>().RegisterEventHandler(this);
	}

    void Update ()
    {
        homerTime += Time.deltaTime;
        if (homerTime > 2.4)
        {
            SetVisible(false);
            active = false;
        }
        else if (homerTime > 2)
        {
            homer.transform.localPosition = new Vector3(-6, 0.15f - (homerTime - 2) * .5f, 0);
        }
        else if (homerTime < 0.4f)
        {
            homer.transform.localPosition = new Vector3(-6, -0.05f + homerTime * .5f, 0);
        }
    }

    void SetVisible(bool val)
    {
        homer.SetActive(val);
    }

    public void OnButtonPressed(VirtualButtonAbstractBehaviour vb)
    {
        if (!active || homerTime < 0.3f)
            return;

        SetVisible(false);

        int points = 0;

		if (boardScript.currentColor == color || color == HomerColor.ORO) {
            points = color == HomerColor.ORO ? 1 : 2;
			GetComponent<AudioSource> ().clip = woohoo;
			GetComponent<AudioSource> ().PlayOneShot (GetComponent<AudioSource> ().clip);
		} else {
            points = -1;
			GetComponent<AudioSource> ().clip = doh;
			GetComponent<AudioSource> ().PlayOneShot (GetComponent<AudioSource> ().clip);
		}

        boardScript.ChangeColor(points, color == HomerColor.ORO);
    }

    public void OnButtonReleased(VirtualButtonAbstractBehaviour vb)
    {
    }

    public void ChangeColor()
    {
        int noHits = Mathf.Min(boardScript.counter / 4 + 1, boardScript.maxNoHits);
        if (boardScript.noHits % noHits == 0)
            color = boardScript.currentColor;
        else
            color = (HomerColor)(random.Next() % 6);
        Renderer homerRenderer = homer.GetComponent<Renderer>();
        if ((boardScript.counter > goldStart && random.NextDouble() < goldProbability) || boardScript.goldTimer > 0)
        {
            color = HomerColor.ORO;
            homerRenderer.material = gold;
        }
        else
        {
            homerRenderer.material = regular;
            homerRenderer.material.mainTexture = textures[(int)color];
            homer.GetComponent<Renderer>().material.mainTexture = textures[(int)color];
        }
    }

	void FixedUpdate ()
    {
        if ((prevHomerId != homerId) == (boardScript.homerId == homerId))
        {
            active = boardScript.homerId == homerId;
            if (active)
            {
                homerTime = 0;
                ChangeColor();
            }
            SetVisible(active);
        }
        prevHomerId = boardScript.homerId;
	}

}
