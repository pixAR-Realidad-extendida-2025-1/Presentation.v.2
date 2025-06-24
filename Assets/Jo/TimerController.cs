using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class TimerController : MonoBehaviour
{
    public TextMeshProUGUI timerText;
    private float timeElapsed = 0f;
    private bool isRunning = false;

    [Header("Input Actions (XR)")]
    public InputActionReference startTimerAction;
    public InputActionReference pauseTimerAction;
    public InputActionReference resetTimerAction;

    void OnEnable()
    {
        if (startTimerAction != null)
            startTimerAction.action.performed += OnStartTimerAction;
        if (pauseTimerAction != null)
            pauseTimerAction.action.performed += OnPauseTimerAction;
        if (resetTimerAction != null)
            resetTimerAction.action.performed += OnResetTimerAction;
    }

    void OnDisable()
    {
        if (startTimerAction != null)
            startTimerAction.action.performed -= OnStartTimerAction;
        if (pauseTimerAction != null)
            pauseTimerAction.action.performed -= OnPauseTimerAction;
        if (resetTimerAction != null)
            resetTimerAction.action.performed -= OnResetTimerAction;
    }

    private void OnStartTimerAction(InputAction.CallbackContext ctx)
    {
        StartTimer();
    }

    private void OnPauseTimerAction(InputAction.CallbackContext ctx)
    {
        PauseTimer();
    }

    private void OnResetTimerAction(InputAction.CallbackContext ctx)
    {
        ResetTimer();
    }

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
