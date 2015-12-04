using UnityEngine;
using Vuforia;

public enum HomerColor
{
    AMARILLO,
    AZUL,
    BLANCO,
    MORADO,
    ROJO,
    VERDE,
    ORO
};

public class BoardScript : MonoBehaviour, ITrackableEventHandler {

    public float changeHomerTimerLenght, gameTimerLenght, colorTimerLength, goldTimerLength, messageTimerLength;
    public int homerId { get; private set; }
    public int noHits { get; private set; }
    public int maxNoHits;
    public TextMesh timeText, hitsText;
    public GameObject trackText, colorArea, ColorLight, message;
    public HomerColor previousColor;
    public HomerColor currentColor { get; private set; }
    public AudioSource mainSound;
    public AudioClip soundtrack, goldSoundtrack;

    private int prevHomerId;
    public int counter { get; private set; }
    private System.Random random;
    public float goldTimer { get; private set; }
    private float changeHomerTimer, gameTimer, colorTimer, messageTimer;
    private bool finishedGame, newColor, boardTracked;
    private Color[] colors = new Color[]{ Color.yellow, new Color(0, 0, .7f), Color.white,
        new Color(.34f, .07f, .44f), new Color(.7f, 0, 0), new Color(0, .7f, 0) };

	void Start ()
    {
        random = new System.Random();
        changeHomerTimer = 0;
        gameTimer = gameTimerLenght;
        colorTimer = colorTimerLength;
        prevHomerId = -1;
        counter = 0;
        finishedGame = false;
        previousColor = HomerColor.AMARILLO;
        noHits = 0;
        boardTracked = false;
        goldTimer = 0;

        ChangeColor();

        ObjectTracker tracker = TrackerManager.Instance.GetTracker<ObjectTracker>();
        tracker.PersistExtendedTracking(true);
        GetComponent<TrackableBehaviour>().RegisterTrackableEventHandler(this);
	}

    void SetMessage(string instruction)
    {
        message.SetActive(true);
        messageTimer = .01f;
        message.transform.Find("Instruction").gameObject.SetActive(true);
        message.transform.Find("Instruction").GetComponent<TextMesh>().text = instruction;
    }

    public void ChangeColor(int points = 0, bool gold = false)
    {
        if (points != 0)
        {
            counter += points;
            if (counter < 0)
                counter = 0;
            hitsText.color = points > 0 ? Color.white : new Color(.6f, .3f, 0);
        }
        homerId = -1;
        if (gold)
        {
            if (goldTimer == 0)
            {
                goldTimer = .1e-5f;
                mainSound.clip = goldSoundtrack;
                mainSound.Play();
            }
            colorArea.SetActive(true);
            colorArea.GetComponent<Renderer>().material.SetColor("_EmissionColor", colors[(int)HomerColor.AMARILLO]);
            ColorLight.SetActive(true);
            ColorLight.GetComponent<Light>().color = colors[(int)HomerColor.AMARILLO];
            changeHomerTimer = .2f;
        }
        else
        {
            changeHomerTimer = 0;
            colorTimer = colorTimerLength;
            if (points >= 0)
            {
                currentColor = (HomerColor)(random.Next() % 6);
                while (currentColor == previousColor)
                    currentColor = (HomerColor)(random.Next() % 6);
                previousColor = currentColor;
                colorArea.SetActive(false);
                colorArea.GetComponent<Renderer>().material.SetColor("_EmissionColor", colors[(int)currentColor]);
                ColorLight.SetActive(false);
                ColorLight.GetComponent<Light>().color = colors[(int)currentColor];
            }
            newColor = true;
        }
        noHits = 0;
        if (!boardTracked)
            return;
    }

    void FixedUpdate ()
    {
        if (!newColor)
        {
            changeHomerTimer -= Time.fixedDeltaTime;
            if (changeHomerTimer <= 0 && !finishedGame)
            {
                homerId = random.Next(0, 6);
                while (homerId == prevHomerId)
                    homerId = random.Next(0, 6);
                changeHomerTimer = changeHomerTimerLenght;
                prevHomerId = homerId;
                ++noHits;
            }
        }
	}

    void Update ()
    {
        if (!finishedGame && messageTimer == 0)
        {
            gameTimer -= Time.deltaTime;
            if (gameTimer <= 0)
            {
                finishedGame = true;
                homerId = -1;
                message.SetActive(true);
                message.transform.Find("Points").gameObject.SetActive(true);
                if (counter > 0)
                    message.transform.Find("Points").GetComponent<TextMesh>().text =
                        string.Format("¡Obtuviste {0} puntos!", counter);
                trackText.SetActive(false);
            }

            int seconds = (int)gameTimer;
            timeText.text = string.Format("{0:00}:{1:00}", seconds / 60, seconds % 60);
            hitsText.text = string.Format("{0:00}", counter);

            if (newColor)
            {
                colorTimer -= Time.deltaTime;
                if (colorTimer <= 0)
                {
                    ColorLight.SetActive(false);
                    if (counter >= 12)
                    {
                        colorArea.SetActive(false);
                    }
                    changeHomerTimer = 0;
                    newColor = false;
                    hitsText.color = new Color(.7f, .7f, .7f);
                } else if (colorTimer <= 0.7 * colorTimerLength)
                {
                    colorArea.SetActive(boardTracked);
                    ColorLight.SetActive(boardTracked);
                }
            }

            if (goldTimer != 0)
            {
                goldTimer += Time.deltaTime;
                if (goldTimer >= goldTimerLength)
                {
                    goldTimer = 0;
                    mainSound.clip = soundtrack;
                    mainSound.Play();
                    ChangeColor();
                }
            }
        }
        if (messageTimer != 0)
        {
            messageTimer += Time.deltaTime;
            if (messageTimer >= messageTimerLength)
            {
                message.transform.Find("Instruction").gameObject.SetActive(false);
                message.SetActive(false);
                messageTimer = 0;
            }
        }

		if (Cardboard.SDK.Triggered)
            Application.LoadLevel(Application.loadedLevel);
    }

    public void OnTrackableStateChanged(TrackableBehaviour.Status previousStatus, TrackableBehaviour.Status newStatus)
    {
        boardTracked = newStatus >= TrackableBehaviour.Status.DETECTED;
        trackText.SetActive(!boardTracked && !finishedGame);
        if (boardTracked && previousStatus < TrackableBehaviour.Status.DETECTED)
            ChangeColor();
    }
}
