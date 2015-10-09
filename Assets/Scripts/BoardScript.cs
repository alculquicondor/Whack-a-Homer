using UnityEngine;

public class BoardScript : MonoBehaviour {

    public float changeTimerLenght, gameTimerLenght;
    public int homerId, winCounter, counter;
    public TextMesh timeText, hitsText, endText, continueText;

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
                continueText.text = "Hit trigger to restart";
                endText.text = counter >= winCounter ? "You win" : "You lose";
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
}
