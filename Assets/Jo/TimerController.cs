using UnityEngine;
using TMPro;

public class TimerController : MonoBehaviour
{
    public TextMeshProUGUI timerText;
    private float timeElapsed = 0f;
    private bool isRunning = false;



    void Update()
    {
        if (isRunning)
        {
            timeElapsed += Time.deltaTime;
            timerText.text = FormatTime(timeElapsed);
        }
    }

    public void StartTimer()
    {
        isRunning = true;
    }

    public void PauseTimer()
    {
        isRunning = false;
    }

    public void ResetTimer()
    {
        isRunning = false;
        timeElapsed = 0f;
        timerText.text = "00:00";
    }

    private string FormatTime(float time)
    {
        int minutes = Mathf.FloorToInt(time / 60f);
        int seconds = Mathf.FloorToInt(time % 60f);
        return string.Format("{0:00}:{1:00}", minutes, seconds);
    }
}
