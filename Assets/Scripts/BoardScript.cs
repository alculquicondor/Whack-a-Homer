using UnityEngine;
using System.Collections;

public class BoardScript : MonoBehaviour {

    public float timerLenght;
    public int prevHomerId, homerId;

    private System.Random random;
    private float timer;

	void Start ()
    {
        random = new System.Random();
        timer = 0;
        prevHomerId = -1;
	}
	
	void FixedUpdate ()
    {
        timer -= Time.fixedDeltaTime;
        if (timer <= 0)
        {
            homerId = random.Next(0, 6);
            while (homerId == prevHomerId)
                homerId = random.Next(0, 6);
            timer = timerLenght;
            prevHomerId = homerId;
        }
	}
}
