using UnityEngine;
using Vuforia;

public class BoardScript : MonoBehaviour, ITrackableEventHandler {

    public float changeTimerLenght, gameTimerLenght;
    public int homerId, winCounter, counter;
    public TextMesh timeText, hitsText, endText, continueText;
    public GameObject trackText;

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
        ObjectTracker tracker = TrackerManager.Instance.GetTracker<ObjectTracker>();
        Debug.Log(tracker.PersistExtendedTracking(true));
        GetComponent<TrackableBehaviour>().RegisterTrackableEventHandler(this);
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
            if (gameTimer <= 0 || counter >= winCounter)
            {
                finishedGame = true;
                homerId = -1;
                continueText.text = "Tap para reintentar";
                endText.text = counter >= winCounter ? "¡Ganaste!" : "¡Perdiste!";
            }

            int seconds = (int)gameTimer;
            timeText.text = string.Format("{0:00}:{1:00}", seconds / 60, seconds % 60);
            hitsText.text = string.Format("{0:00}", winCounter - counter);
        }
		else if (Cardboard.SDK.Triggered)
        {
            Application.LoadLevel(Application.loadedLevel);
        }
    }

    public void OnTrackableStateChanged(TrackableBehaviour.Status previousStatus, TrackableBehaviour.Status newStatus)
    {
        if (previousStatus < TrackableBehaviour.Status.DETECTED && newStatus >= TrackableBehaviour.Status.DETECTED)
        {
            trackText.SetActive(false);
        }
        else if (previousStatus >= TrackableBehaviour.Status.DETECTED && newStatus < TrackableBehaviour.Status.DETECTED) {
            trackText.SetActive(true);
        }
    }
}
