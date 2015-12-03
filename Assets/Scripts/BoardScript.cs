using UnityEngine;
using Vuforia;

public enum HomerColor
{
    AMARILLO,
    AZUL,
    BLANCO,
    MORADO,
    NEGRO,
    ROJO,
    VERDE
};

public class BoardScript : MonoBehaviour, ITrackableEventHandler {

    public float changeHomerTimerLenght, gameTimerLenght, colorTimerLength;
    public int homerId { get; private set; }
    public int noHits { get; private set; }
    public int maxNoHits;
    public TextMesh timeText, hitsText;
    public GameObject trackText, colorArea, ColorLight, finishText;
    public HomerColor previousColor;
    public HomerColor currentColor { get; private set; }

    private int prevHomerId, counter;
    private System.Random random;
    private float changeHomerTimer, gameTimer, colorTimer;
    private bool finishedGame, newColor;
    private Color[] colors = new Color[]{ Color.yellow, new Color(0, 0, .7f), Color.white,
        new Color(.34f, .07f, .44f), Color.black, new Color(.7f, 0, 0), new Color(0, .7f, 0) };

	void Start ()
    {
        random = new System.Random();
        changeHomerTimer = 0;
        gameTimer = gameTimerLenght;
        colorTimer = colorTimerLength;
        prevHomerId = -1;
        counter = 0;
        finishedGame = false;
        previousColor = HomerColor.NEGRO;
        noHits = 0;

        ChangeColor();

        ObjectTracker tracker = TrackerManager.Instance.GetTracker<ObjectTracker>();
        tracker.PersistExtendedTracking(true);
        GetComponent<TrackableBehaviour>().RegisterTrackableEventHandler(this);
	}

    public void ChangeColor(int points = 0)
    {
        homerId = -1;
        currentColor = (HomerColor)(random.Next() % 7);
        while (currentColor == previousColor)
            currentColor = (HomerColor)(random.Next() % 7);
        previousColor = currentColor;
        colorTimer = colorTimerLength;
        changeHomerTimer = 0;
        colorArea.SetActive(false);
        colorArea.GetComponent<Renderer>().material.SetColor("_EmissionColor", colors[(int)currentColor]);
        ColorLight.SetActive(false);
        ColorLight.GetComponent<Light>().color = colors[(int)currentColor];
        newColor = true;
        noHits = 0;
        if (points != 0)
        {
            counter += points;
            if (counter < 0)
                counter = 0;
            hitsText.color = points > 0 ? new Color(0, .8f, 0) : new Color(.9f, .6f, 0);
        }
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
        if (!finishedGame)
        {
            gameTimer -= Time.deltaTime;
            if (gameTimer <= 0)
            {
                finishedGame = true;
                homerId = -1;
                finishText.SetActive(true);
                if (counter > 0)
                    finishText.transform.Find("Points").GetComponent<TextMesh>().text =
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
                    colorArea.SetActive(false);
                    ColorLight.SetActive(false);
                    changeHomerTimer = 0;
                    newColor = false;
                    hitsText.color = Color.white;
                } else if (colorTimer <= 0.7 * colorTimerLength)
                {
                    colorArea.SetActive(true);
                    ColorLight.SetActive(true);
                }
            }
        }

		if (Cardboard.SDK.Triggered)
            Application.LoadLevel(Application.loadedLevel);
    }

    public void OnTrackableStateChanged(TrackableBehaviour.Status previousStatus, TrackableBehaviour.Status newStatus)
    {
        trackText.SetActive(newStatus < TrackableBehaviour.Status.DETECTED && !finishedGame);
        if (newStatus >= TrackableBehaviour.Status.DETECTED && previousStatus < TrackableBehaviour.Status.DETECTED)
            ChangeColor();
    }
}
