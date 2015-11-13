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

    public float changeTimerLenght, gameTimerLenght;
    public int homerId, counter;
    public TextMesh timeText, hitsText, endText, continueText, colorText;
    public GameObject trackText;
    public HomerColor previousColor, currentColor;

    private int prevHomerId;
    private System.Random random;
    private float changeTimer, gameTimer;
    private bool finishedGame;

	void Start ()
    {
        random = new System.Random();
        changeTimer = 0;
        gameTimer = gameTimerLenght;
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
        currentColor = (HomerColor)random.Next(7);
        while (currentColor == previousColor)
            currentColor = (HomerColor)random.Next(7);
        colorText.text = currentColor.ToString();
        previousColor = currentColor;
    }
	
	void FixedUpdate ()
    {
        changeTimer -= Time.fixedDeltaTime;
        if (changeTimer <= 0 && !finishedGame)
        {
            homerId = random.Next(0, 6);
            while (homerId == prevHomerId)
                homerId = random.Next(0, 6);
            changeTimer = changeTimerLenght;
            prevHomerId = homerId;
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
        }
		else if (Cardboard.SDK.Triggered)
        {
            Application.LoadLevel(Application.loadedLevel);
        }
    }

    public void OnTrackableStateChanged(TrackableBehaviour.Status previousStatus, TrackableBehaviour.Status newStatus)
    {
        trackText.SetActive(newStatus < TrackableBehaviour.Status.DETECTED);
    }
}
