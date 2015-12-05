using UnityEngine;
using Vuforia;

public class HitScript : MonoBehaviour, IVirtualButtonEventHandler {

	public AudioClip doh, woohoo, whistle;
    public int homerId;
    public Texture []textures;
    public Material regular, gold;
    public int goldStart;
    public float goldProbability;

    private System.Random random;
    private HomerColor color;
    private GameObject homer;
    private BoardScript boardScript;
    private bool active;
    private float homerTime;
    private int prevHomerId;
    private bool alternate;

	void Start ()
    {
        homer = transform.Find("homer").gameObject;
        boardScript = GameObject.Find("Board").GetComponent<BoardScript>();

        prevHomerId = -1;
        random = new System.Random(homerId * 10);
        active = false;
        GetComponent<VirtualButtonBehaviour>().RegisterEventHandler(this);
	}

    void Update ()
    {
        homerTime += Time.deltaTime;
        if (homerTime > .9 * boardScript.changeHomerTimerLenght)
        {
            homer.SetActive(false);
            active = false;
        }
        else if (homerTime > .8 * boardScript.changeHomerTimerLenght)
        {
            homer.transform.localPosition = new Vector3(-6, 0.15f - (homerTime - .8f * boardScript.changeHomerTimerLenght) *.5f, 0);
        }
        else if (homerTime < .1 * boardScript.changeHomerTimerLenght)
        {
            homer.transform.localPosition = new Vector3(-6, 0.15f - (.1f * boardScript.changeHomerTimerLenght - homerTime) * .5f, 0);
        }
    }

    public void OnButtonPressed(VirtualButtonAbstractBehaviour vb)
    {
        if (!active || homerTime < 0.3f)
            return;

        homer.SetActive(false);

        int points = 0;

		if (boardScript.currentColor == color || color == HomerColor.ORO) {
            points = color == HomerColor.ORO ? 1 : 2;
            GetComponent<AudioSource>().PlayOneShot(woohoo);
		} else {
            points = -1;
            GetComponent<AudioSource>().PlayOneShot(doh);
		}

        boardScript.ChangeColor(points, color == HomerColor.ORO);
    }

    public void OnButtonReleased(VirtualButtonAbstractBehaviour vb)
    {
    }

    public void ChangeColor()
    {
        int noHits = Mathf.Min(boardScript.counter / 20 + 1, boardScript.maxNoHits);
        if (!alternate && boardScript.noHits % noHits == 0)
            color = boardScript.currentColor;
        else
            color = (HomerColor)(random.Next() % 6);
        Renderer homerRenderer = homer.GetComponent<Renderer>();
        if (((boardScript.counter > goldStart && random.NextDouble() < goldProbability) || boardScript.goldTimer > 0))
        {
            color = HomerColor.ORO;
            homerRenderer.material = gold;
            if (boardScript.goldTimer == 0)
                GetComponent<AudioSource>().PlayOneShot(whistle);
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
        if (boardScript.homerId == prevHomerId)
            return;
        prevHomerId = boardScript.homerId;
        if (boardScript.homerId == -1)
        {
            active = false;
            homer.SetActive(false);
            return;
        }

        if (boardScript.homerId == homerId || (boardScript.goldTimer == 0 && (boardScript.homerId + 4) % 6 == homerId))
        {
            alternate = boardScript.homerId != homerId;
            if (boardScript.gotGold)
                return;
            active = true;
            homerTime = 0;
            homer.SetActive(true);
            ChangeColor();
            if (color == HomerColor.ORO)
            {
                boardScript.gotGold = true;
                foreach (HitScript hs in GameObject.FindObjectsOfType<HitScript>())
                    if (hs.homerId != homerId)
                    {
                        Debug.Log(hs.homerId);
                        hs.active = false;
                        hs.homer.SetActive(false);
                    }
            }
        }
        else if ((!alternate && boardScript.homerId != homerId) ||
            (alternate && (boardScript.homerId + 4) % 6 != homerId))
        {
            active = false;
            homer.SetActive(false);
        }
        
	}

}
