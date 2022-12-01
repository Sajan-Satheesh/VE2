
using TMPro;
using UnityEngine;


public class TimerGame : MonoBehaviour
{
    private TMP_Text gameTime;
    private int maxTime;
    [SerializeField] GameObject gameOver;
    int leftMinutes;
    int leftSeconds;
    int seconds;
    int timeElapsed;
    bool minuteElapsed;
    // Start is called before the first frame update
    void Awake()
    {
        maxTime = GameManager.GameManagerInstance.MaxGameTime-1;
        gameTime = GetComponent<TMP_Text>();
    }
    private void Start()
    {
        seconds = (int)Time.time;
        leftMinutes = maxTime;
        leftSeconds = 60;
        timeElapsed = 0;
        minuteElapsed = true;
    }

    // Update is called once per frame
    void Update()
    {
        Timer();
        CheckGameOver();

    }
     void CheckGameOver()
    {
        if(leftMinutes == 0 && leftSeconds == 0)
        {
            gameOver.SetActive(true);
        }
    }
    void Timer()
    {
        timeElapsed = (int)Time.time;

        if ((timeElapsed % 61) == 0)
        {
            minuteElapsed = true;
        }
        else if (((timeElapsed % 60) == 0) && minuteElapsed)
        {
            leftMinutes -= 1;
            Debug.Log(leftMinutes);
            leftSeconds = 60;
            minuteElapsed = false;
        }
        if (timeElapsed > seconds)
        {
            leftSeconds -= 1;
            seconds += 1;
            gameTime.text = "Time Left: " + leftMinutes.ToString() + " mins, " + leftSeconds.ToString() + " seconds";
        }
    }
}
