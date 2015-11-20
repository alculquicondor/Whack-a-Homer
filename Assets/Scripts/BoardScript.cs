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
    public int homerId, counter;
    public TextMesh timeText, hitsText, endText, continueText;
    public GameObject trackText, border;
    public HomerColor previousColor, currentColor;

    private int prevHomerId;
    private System.Random random;
    private float changeHomerTimer, gameTimer, colorTimer;
    private bool finishedGame, newColor;
    private Color[] colors = new Color[]{ Color.yellow, Color.blue, Color.white,
        Color.magenta, Color.black, Color.red, Color.green };

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

        ChangeColor();

        ObjectTracker tracker = TrackerManager.Instance.GetTracker<ObjectTracker>();
        tracker.PersistExtendedTracking(true);
        GetComponent<TrackableBehaviour>().RegisterTrackableEventHandler(this);
	}

    public void ChangeColor()
    {
        homerId = -1;
        currentColor = (HomerColor)random.Next(7);
        while (currentColor == previousColor)
            currentColor = (HomerColor)random.Next(7);
        previousColor = currentColor;
        colorTimer = colorTimerLength;
        changeHomerTimer = 0;
        border.SetActive(true);
        foreach (Renderer rend in border.GetComponentsInChildren<Renderer>())
        {
            rend.material.SetColor("_TintColor", colors[(int)currentColor]);
        }
        newColor = true;
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
                continueText.text = "Tap para reintentar";
                endText.text = counter > 0 ?
                    string.Format("¡Obtuviste {0} puntos!", counter) :
                    "Continúa practicando";
            }

            int seconds = (int)gameTimer;
            timeText.text = string.Format("{0:00}:{1:00}", seconds / 60, seconds % 60);
            hitsText.text = string.Format("{0:00}", counter);

            if (newColor)
            {
                colorTimer -= Time.deltaTime;
                if (colorTimer <= 0)
                {
                    border.SetActive(false);
                    changeHomerTimer = 0;
                    newColor = false;
                }
            }
        }
		else if (Cardboard.SDK.Triggered)
        {
            Application.LoadLevel(Application.loadedLevel);
        }
    }

    public void OnTrackableStateChanged(TrackableBehaviour.Status previousStatus, TrackableBehaviour.Status newStatus)
    {
        trackText.SetActive(newStatus < TrackableBehaviour.Status.DETECTED);
        if (newStatus >= TrackableBehaviour.Status.DETECTED && previousStatus < TrackableBehaviour.Status.DETECTED)
            ChangeColor();
    }
}
