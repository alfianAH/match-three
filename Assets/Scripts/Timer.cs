using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    [SerializeField] private float defaultMinutes = 1f, 
        minutes = 1f,
        seconds = 59f,
        milliseconds;
    [SerializeField] private GameObject gameOver;
    
    private bool isCountingDown = true;
    private Text timerText;

    private void Start()
    {
        timerText = GetComponent<Text>();
        minutes = defaultMinutes;
    }

    private void Update()
    {
        if (isCountingDown)
        {
            milliseconds += Time.deltaTime;

            // If milliseconds is greater or equals to 1, ...
            if (milliseconds >= 1.0f)
            {
                milliseconds -= 1.0f;

                // If time is not up, ...
                if (seconds > 0 || minutes > 0)
                {
                    seconds--; // Decrease seconds
                    if (seconds < 0.0f)
                    {
                        seconds = 59; // Repeat seconds
                        minutes--; // Decrease minutes;
                    }
                }
                else // If time is up, ...
                {
                    isCountingDown = false;
                    gameOver.SetActive(true);
                }
            }

            UpdateTimerText();
        }
    }

    private void UpdateTimerText()
    {
        timerText.text = $"{minutes:00} : {seconds:00}";
    }
}
